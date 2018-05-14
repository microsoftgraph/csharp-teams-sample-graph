const restify = require('restify');
const CookieParser = require('restify-cookies');

process.env.AUTH_CLIENT_ID = (process.env.AUTH_CLIENT_ID) ? process.env.AUTH_CLIENT_ID : 'bc92af75-4cdb-41ca-80e8-3afa0d4360ea';
process.env.AUTH_CLIENT_SECRET = (process.env.AUTH_CLIENT_SECRET) ? process.env.AUTH_CLIENT_SECRET : 'blES;rjahlTHHKG30274*:~';
process.env.BASE_URI = (process.env.BASE_URI) ? process.env.BASE_URI : 'http://localhost:55069';
process.env.PORT = '55069';


    //tyyJIOO252^-ywnpNVR11?{
//process.env.host = 'http://localhost:55069';
//process.env.ClientSecret = 'tyyJIOO252 ^ -ywnpNVR11 ? {';
//process.env.APPSETTING_ClientSecret = 'tyyJIOO252^-ywnpNVR11?{';
//process.env.WEBSITE_HOSTNAME = 'http://localhost:55069';



var server = restify.createServer();
server.use(restify.queryParser());
server.use(CookieParser.parse);
server.use(restify.bodyParser());


//server.use(function (req, res, next) {
//    res.header("Access-Control-Allow-Origin", "*");
//    res.header("Access-Control-Allow-Headers", "Origin, X-Requested-With, Content-Type, Accept");
//    next();
//});

server.listen(process.env.port || process.env.PORT || 55069, () => {
    console.log(`Started sample app for Microsoft Teams in Microsoft Graph`);
});

var auth = require('./auth/auth.js');
auth.init(server);
auth.start_listening();

var graph = require('./ImportantFiles/graphService.js');
graph.init(server);
graph.start_listening();