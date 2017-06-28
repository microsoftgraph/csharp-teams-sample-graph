/*
 * Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license.
 * See LICENSE in the project root for license information.
 */
var https = require('https');
var authHelper = require('../auth/authHelper.js');

/** 
 * Execute HTTPS request to the specified path, handling errors
 * @param req Incoming user request context
 * @param res Outgoing response context
 * @param next Next route handler context
 * @param {string} requestType the HTTPS request type
 * @param {string} requestPath the resource path to which to send the request
 * @param {callback} callback with data successfully retrieved.
 */
function executeRequestWithErrorHandling(req, res, next, requestType, requestPath, callback) {
		if (req.cookies.REFRESH_TOKEN_CACHE_KEY === undefined) {
			res.redirect(`/login?${process.env.host}&${process.env.ClientSecret}&${process.env.APPSETTING_ClientSecret}&${process.env.WEBSITE_HOSTNAME}`, next);
    }
    else
    {
      //Step 1. Attempt the request
      executeHttpsRequest(
        requestType,
        requestPath,
        req.body,
        req.cookies.ACCESS_TOKEN_CACHE_KEY,
        function (firstRequestError, data) {
          if (!firstRequestError) {
            //Success.  Return data.
            callback(data);   
          } else if (authHelper.hasAccessTokenExpired(firstRequestError)) {
            //Step 2. Request didn't work because access token expired.  Handle the refresh flow
            authHelper.getTokenFromRefreshToken(
              req.cookies.REFRESH_TOKEN_CACHE_KEY,
              function (refreshError, accessToken) {
                res.setCookie(authHelper.ACCESS_TOKEN_CACHE_KEY, accessToken);
                if (accessToken !== null) {
                  //Step 3. Got a new access token.  Attempt the request again.
                  executeHttpsRequest(
                    requestType,
                    requestPath,
                    req.body,
                    accessToken,
                    function (secondRequestError, data) {
                      if (!secondRequestError) {
                        //Success.  Return data.
                        callback(data);
                      } else {
                        authHelper.clearCookies(res);
                        renderError(res, secondRequestError);  //Step 3 failed.
                      }
                    }
                  );
                } else {
                  renderError(res, refreshError);  //Step 2 failed.
                }
              });
          } else {
            renderError(res, firstRequestError);  //Step 1 failed.
          }
        }
      );
        
    }

}

/**
 * Execute HTTPS request to the specified endpoint
 * @param {string} requestType the HTTPS request type
 * @param {string} requestPath the resource path from which to retrieve data
 * @param {object} requestBody the data to be 'POST'ed
 * @param {string} accessToken the access token with which the request should be authenticated
 * @param {callback} callback
 */
function executeHttpsRequest(requestType, requestPath, requestBody, accessToken, callback) {
  var requestBodyAsString = JSON.stringify(requestBody);
  var options = {
    host: 'graph.microsoft.com',
    path: requestPath,
    method: requestType,
    headers: {
      'Content-Type': 'application/json',
      Authorization: 'Bearer ' + accessToken,
      'Content-Length': (requestBody == null) ? 0 : requestBodyAsString.length
    }
  };

  var request = https.request(options, function (response) {
    var body = '';
    response.on('data', function (d) {
      body += d;
    });
    response.on('end', function () {
      var error;
      if (response.statusCode === 200) {
        callback(null, JSON.parse(body));
      } else if (response.statusCode === 202) {
        callback(null, null);
      } else if (response.statusCode === 204) {
        callback(null, null);
      } else {
        error = new Error();
        error.code = response.statusCode;
        error.message = response.statusMessage;
        body = body.trim();
        error.innerError = JSON.parse(body).error;
        callback(error, null);
      }
    });
  });

  // write the outbound data to it
  if (requestBody != null) {
    request.write(requestBodyAsString);
  }
  // we're done!
  request.end();

  request.on('error', function (e) {
    callback(e, null);
  });
}

function renderError(res, e) {
  res.send({
    message: e.message,
    error: e
  });
  res.end();
}

exports.executeRequestWithErrorHandling = executeRequestWithErrorHandling;