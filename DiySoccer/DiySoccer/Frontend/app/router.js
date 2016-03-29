var MyRouter = Marionette.AppRouter.extend({
    initialize: function () {
        var self = this;

        MyApp.listenTo(MyApp.Settings, 'sync', function () {
            self.changeModule(self.workingModule, self.workingOptions);
        });
    },
    routes: {
        "": "defaultRoute",
        "leagues/": "defaultRoute",
        "leagues/new": "newLeagueRoute",
        "leagues/:leagueId/edit": "editLeagueRoute",
        "leagues/:leagueId": "tableRoute",
        "leagues/:leagueId/teams/new": "newTeamRoute",
        "leagues/:leagueId/teams/:teamId/edit": "editTeamRoute",
        "leagues/:leagueId/teams/:teamId": "infoTeamRoute",
        "leagues/:leagueId/games/new": "newGameRoute",
        "leagues/:leagueId/games/:gameId/edit": "editGameRoute"
    },
    defaultRoute: function () {
        this.changeModule(MyApp.submodules.leagues);
    },
    newLeagueRoute: function (leagueId) {
        var options = { leagueId: leagueId };
        this.changeModule(MyApp.submodules.league, options);
    },
    editLeagueRoute: function (leagueId) {
        var options = { leagueId: leagueId };
        this.changeModule(MyApp.submodules.league, options);
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