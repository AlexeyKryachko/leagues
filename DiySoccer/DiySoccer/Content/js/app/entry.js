var MyApp = require("./app.js");
var template = require("./template.js");

require("./header/headerModule.js");

require("./calendar/calendarModule.js");

require("./leagues/list/leaguesModule.js");
require("./leagues/league/leagueModule.js");
require("./leagues/info/leagueinfoModule.js");

MyApp.start();