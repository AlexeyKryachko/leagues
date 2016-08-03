var MyApp = require("./app.js");
var template = require("./template.js");

require("./header/headerModule.js");

require("./calendar/calendarModule.js");

require("./games/game/gameModule.js");
require("./games/gameInfo/gameInfoModule.js");

require("./teams/info/teamInfoModule.js");
require("./teams/list/teamListModule.js");
require("./teams/team/teamModule.js");

require("./leagues/info/leagueinfoModule.js");
require("./leagues/list/leaguesModule.js");
require("./leagues/league/leagueModule.js");

require("./tournaments/info/tournamentsInfoModule.js");

MyApp.start();