/*
 * Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license.
 * See LICENSE in the project root for license information.
 */

// This sample uses an open source OAuth 2.0 library that is compatible
// with the Azure AD v2.0 endpoint.
// Microsoft does not provide fixes or direct support for this library.
// Refer to the library’s repository to file issues or for other support.
// For more information about auth libraries see: https://azure.microsoft.com/documentation/articles/active-directory-v2-libraries/
// Library repo: https://github.com/ciaranj/node-oauth

var OAuth = require('oauth');
var uuid = require('node-uuid');

// The application registration (must match Azure AD config)
// If running in cloud, configure details as env variables, otherwise configure them locally.
var credentials = {
  authority: 'https://login.microsoftonline.com/common',
  authorize_endpoint: '/oauth2/v2.0/authorize',
  token_endpoint: '/oauth2/v2.0/token',
  client_id: process.env.AUTH_CLIENT_ID,
  client_secret: process.env.AUTH_CLIENT_SECRET,
  redirect_uri: `${process.env.BASE_URI}/login`,
  scope: 'User.ReadWrite.All Group.ReadWrite.All offline_access'
};

/**
 * Generate a fully formed uri to use for authentication based on the supplied resource argument
 * @return {string} a fully formed uri with which authentication can be completed
 */
function getAuthUrl(redirectUrl) {
  var authUrl = credentials.authority + credentials.authorize_endpoint +
    '?client_id=' + credentials.client_id +
    '&response_type=code' +
    '&redirect_uri=' + credentials.redirect_uri  +
    '&scope=' + credentials.scope +
    '&response_mode=query' +
    '&nonce=' + uuid.v4();
    return authUrl;
}

/**
 * Gets a token for a given resource.
 * @param {string} code An authorization code returned from a client.
 * @param {AcquireTokenCallback} callback The callback function.
 */
function getTokenFromCode(code, callback) {
  var OAuth2 = OAuth.OAuth2;
  var oauth2 = new OAuth2(
    credentials.client_id,
    credentials.client_secret,
    credentials.authority,
    credentials.authorize_endpoint,
    credentials.token_endpoint
  );

  oauth2.getOAuthAccessToken(
    code,
    {
      grant_type: 'authorization_code',
      redirect_uri: credentials.redirect_uri,
      response_mode: 'form_post',
      nonce: uuid.v4(),
      state: 'abcd'
    },
    function (e, accessToken, refreshToken) {
      callback(e, accessToken, refreshToken);
    }
  );
}

/**
 * Gets a new access token via a previously issued refresh token.
 * @param {string} refreshToken A refresh token returned in a token response
 *                       from a previous result of an authentication flow.
 * @param {AcquireTokenCallback} callback The callback function.
 */
function getTokenFromRefreshToken(refreshToken, callback) {
  var OAuth2 = OAuth.OAuth2;
  var oauth2 = new OAuth2(
    credentials.client_id,
    credentials.client_secret,
    credentials.authority,
    credentials.authorize_endpoint,
    credentials.token_endpoint
  );

  oauth2.getOAuthAccessToken(
    refreshToken,
    {
      grant_type: 'refresh_token',
      redirect_uri: credentials.redirect_uri,
      response_mode: 'form_post',
      nonce: uuid.v4(),
      state: 'abcd'
    },
    function (e, accessToken) {
      callback(e, accessToken);
    }
  );
}

function hasAccessTokenExpired(e) {
  var expired;
  if (!e.innerError) {
    expired = false;
  } else {
    expired = e.code === 401 &&
      e.innerError.code === 'InvalidAuthenticationToken' &&
      e.innerError.message === 'Access token has expired.';
  }
  return expired;
}

function clearCookies(res) {
  res.clearCookie('ACCESS_TOKEN_CACHE_KEY');
  res.clearCookie('REFRESH_TOKEN_CACHE_KEY');
}

exports.credentials = credentials;
exports.getAuthUrl = getAuthUrl;
exports.getTokenFromCode = getTokenFromCode;
exports.getTokenFromRefreshToken = getTokenFromRefreshToken;
exports.hasAccessTokenExpired = hasAccessTokenExpired;
exports.clearCookies = clearCookies;
exports.ACCESS_TOKEN_CACHE_KEY = 'ACCESS_TOKEN_CACHE_KEY';
exports.REFRESH_TOKEN_CACHE_KEY = 'REFRESH_TOKEN_CACHE_KEY';
