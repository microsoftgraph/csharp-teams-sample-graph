const restify = require('restify');
const CookieParser = require('restify-cookies');

process.env.AUTH_CLIENT_ID = (process.env.AUTH_CLIENT_ID) ? process.env.AUTH_CLIENT_ID : '[auth client ID]';
process.env.AUTH_CLIENT_SECRET = (process.env.AUTH_CLIENT_SECRET) ? process.env.AUTH_CLIENT_SECRET : '[auth client secret]';
process.env.BASE_URI = (process.env.BASE_URI) ? process.env.BASE_URI : '[the host name for your application]';

var server = restify.createServer();
server.use(restify.queryParser());
server.use(CookieParser.parse);
server.use(restify.bodyParser());

server.listen(process.env.port || process.env.PORT || 55065, () => {
	console.log(`Started sample app for Microsoft Teams in Microsoft Graph`);
});

var auth = require('./auth/auth.js');
auth.init(server);
auth.start_listening();

var graph = require('./graph/graph.js');
graph.init(server);
graph.start_listening();
