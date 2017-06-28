/*
 * Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license.
 * See LICENSE in the project root for license information.
 */
const files = require('../utils/files.js');
const httpsrequesthelper = require('./httpsrequesthelper.js');

var server;

function start_listening() {

	this.server.get('/', (req, res, next) => {
		files.sendFileOrLogin('./graph/sendmessage.html', req, res, next);
	});
 
	this.server.get(/^\/graph/, (req, res, next) => {
		// Proxy requests onto the Microsoft Graph
		var url = req.url.substring('/graph'.length);
		httpsrequesthelper.executeRequestWithErrorHandling(req, res, next, 'GET', url, (data) => {
			res.send(data);
			res.end();
		});
	});

	this.server.post(/^\/graph/, (req, res, next) => {
		// Proxy requests onto the Microsoft Graph
		var url = req.url.substring('/graph'.length);
		httpsrequesthelper.executeRequestWithErrorHandling(req, res, next, 'POST', url, (data) => {
			if (data) { res.send(data); }
			res.end();
		});
	});
}

module.exports.init = function(server) {
	this.server = server;
	return this;
}

module.exports.start_listening = start_listening;