const fs = require('fs-extra');

function sendFile(path, res){
	var data = fs.readFileSync(path, 'utf-8');
	res.writeHead(200, {
		'Content-Length': Buffer.byteLength(data),
  		'Content-Type': 'text/html'
	});

	res.write(data);
	res.end();
}
module.exports.sendFile = sendFile;

module.exports.sendFileOrLogin = function sendFileOrLogin(path, req, res, next){
	if (req.params.auth && (req.cookies.REFRESH_TOKEN_CACHE_KEY === undefined)) {
		var redirectUrl = '/login?';
		if (req.params.web) {
			redirectUrl += 'web=' + req.params.web +'&';
		}
		redirectUrl += 'redirectUrl=' + encodeURIComponent(req.url);
		res.redirect(redirectUrl, next);
	} else {
		sendFile(path, res);
	}
}

