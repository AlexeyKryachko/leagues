var MyRouter = Marionette.AppRouter.extend({
    routes: {
        "": "defaultRoute",
        "leagues/:leagueId": "tableRoute",
        "leagues/:leagueId/teams/new": "newTeamRoute",
        "leagues/:leagueId/games/new": "newGameRoute"
    },
    defaultRoute: function () {
        this.changeModule(MyApp.submodules.leagues);
    },
    tableRoute: function (leagueId) {
        var options = { leagueId: leagueId };
        this.changeModule(MyApp.submodules.teamList, options);
    },
    newTeamRoute: function (leagueId) {
        var options = { leagueId: leagueId };
        this.changeModule(MyApp.submodules.teamNew, options);
    },
    newGameRoute: function (leagueId) {
        var options = { leagueId: leagueId };
        this.changeModule(MyApp.submodules.gameNew, options);
    },
    changeModule: function (module, options) {
        if (this.workingModule)
            this.workingModule.stop();

        this.workingModule = module;
        this.workingModule.start(options);
    }
});