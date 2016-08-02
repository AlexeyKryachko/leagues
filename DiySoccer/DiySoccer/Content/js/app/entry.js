var MyApp = require("./app.js");
var template = require("./template.js");

var headerModule = require("./header/headerModule.js");
MyApp.module("header", headerModule);

MyApp.start();