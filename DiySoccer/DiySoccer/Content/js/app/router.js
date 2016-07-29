var MyApp = require("./app.js");

var MyRouter = Backbone.Marionette.AppRouter.extend({
    initialize: function () {
        var self = this;

        MyApp.listenTo(MyApp.Settings, 'sync', function () {
            console.log('Settings: ', MyApp.Settings.toJSON());
            self.changeModule(self.workingModule, self.workingOptions);
        });
    },
    routes: {
        "": "defaultRoute",
        "leagues/": "defaultRoute",
        "leagues/new": "newLeagueRoute",
        "tournaments/:tournamentId": "tournamentInfoRoute",
        "leagues/:leagueId/edit": "editLeagueRoute",
        "leagues/:leagueId": "leagueInfoRoute",
        "leagues/:leagueId/table": "tableRoute",
        "leagues/:leagueId/teams/new": "newTeamRoute",
        "leagues/:leagueId/teams/:teamId/edit": "editTeamRoute",
        "leagues/:leagueId/teams/:teamId": "infoTeamRoute",
        "leagues/:leagueId/games/new": "newGameRoute",
        "leagues/:leagueId/games/:gameId": "gameInfoRoute",
        "leagues/:leagueId/games/:gameId/edit": "editGameRoute",
        "leagues/:leagueId/calendar": "calendarRoute"
    },
    defaultRoute: function () {
        this.changeModule(MyApp.submodules.leagues);
    },
    calendarRoute: function(leagueId) {
        var options = { leagueId: leagueId };
        this.changeModule(MyApp.submodules.calendar, options);
    },
    newLeagueRoute: function (leagueId) {
        var options = { leagueId: leagueId };
        this.changeModule(MyApp.submodules.league, options);
    },
    editLeagueRoute: function (leagueId) {
        var options = { leagueId: leagueId };
        this.changeModule(MyApp.submodules.league, options);
    },
    tournamentInfoRoute: function (tournamentId) {
        var options = { tournamentId: tournamentId };
        this.changeModule(MyApp.submodules.tournamentsInfo, options);
    },
    leagueInfoRoute: function (leagueId) {
        var options = { leagueId: leagueId };
        this.changeModule(MyApp.submodules.leagueInfo, options);
    },
    tableRoute: function (leagueId) {
        var options = { leagueId: leagueId };
        this.changeModule(MyApp.submodules.teamList, options);
    },
    newTeamRoute: function (leagueId) {
        var options = { leagueId: leagueId };
        this.changeModule(MyApp.submodules.team, options);
    },
    editTeamRoute: function (leagueId, teamId) {
        var options = { leagueId: leagueId, teamId: teamId };
        this.changeModule(MyApp.submodules.team, options);
    },
    infoTeamRoute: function (leagueId, teamId) {
        var options = { leagueId: leagueId, teamId: teamId };
        this.changeModule(MyApp.submodules.teamInfo, options);
    },
    newGameRoute: function (leagueId) {
        var options = { leagueId: leagueId };
        this.changeModule(MyApp.submodules.game, options);
    },
    gameInfoRoute: function (leagueId, gameId) {
        var options = { leagueId: leagueId, gameId: gameId };
        this.changeModule(MyApp.submodules.gameInfo, options);
    },
    editGameRoute: function (leagueId, gameId) {
        var options = { leagueId: leagueId, gameId: gameId };
        this.changeModule(MyApp.submodules.game, options);
    },
    changeModule: function (module, options) {
        if (this.workingModule)
            this.workingModule.stop();

        this.workingModule = module;
        this.workingOptions = options;
        this.workingModule.start(this.workingOptions);
    }
});

module.exports = MyRouter;