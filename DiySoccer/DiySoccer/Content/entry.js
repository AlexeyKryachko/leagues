//Styles
//require('./scss/default.scss');
//require('./scss/bootstrap/stylesheets/_bootstrap.scss');
//require('./scss/datetimepicker/jquery.datetimepicker.min.css');

//Scripts
var MyApp = require("./js/app/app.js");
var template = require("./js/app/template.js");

require("./js/app/header/headerModule.js");

require("./js/app/calendar/calendarModule.js");

require("./js/app/games/game/gameModule.js");
require("./js/app/games/gameInfo/gameInfoModule.js");

require("./js/app/teams/info/teamInfoModule.js");
require("./js/app/teams/list/teamListModule.js");
require("./js/app/teams/team/teamModule.js");

require("./js/app/leagues/info/leagueinfoModule.js");
require("./js/app/leagues/list/leaguesModule.js");
require("./js/app/leagues/statistic/statisticModule.js");
require("./js/app/leagues/league/leagueModule.js");

require("./js/app/tournaments/info/tournamentsInfoModule.js");

MyApp.start();