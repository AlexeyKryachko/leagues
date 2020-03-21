var _ = require('underscore');

function parseQueryString(queryString) {
    var params = {};
    if (queryString) {
        _.each(
            _.map(decodeURI(queryString).split(/&/g), function (el, i) {
                var aux = el.split('='), o = {};
                if (aux.length >= 1) {
                    var val = undefined;
                    if (aux.length == 2)
                        val = aux[1];
                    o[aux[0]] = val;
                }
                return o;
            }),
            function (o) {
                _.extend(params, o);
            }
        );
    }
    return params;
}

var MyRouter = Backbone.Marionette.AppRouter.extend({
    initialize: function (app) {
        var self = this;

        self.app = app;

        self.app.listenTo(self.app.Settings, 'sync', function () {
            console.log('Settings: ', self.app.Settings.toJSON());
            self.changeModule(self.workingModule, self.workingOptions);
        });
    },
    routes: {
        "": "defaultRoute",
        "#": "defaultRoute",
        "leagues/": "defaultRoute",
        "leagues/new": "newLeagueRoute",
        "tournaments/:tournamentId": "tournamentInfoRoute",
        "leagues/:leagueId/edit": "editLeagueRoute",
        "leagues/:leagueId": "leagueInfoRoute",
        "leagues/:leagueId/": "leagueInfoRoute",
        "leagues/:leagueId/table": "tableRoute",
        "leagues/:leagueId/statistics": "leagueStatisticsRoute",
        "leagues/:leagueId/teams/new": "newTeamRoute",
        "leagues/:leagueId/teams/:teamId/edit": "editTeamRoute",
        "leagues/:leagueId/teams/:teamId": "infoTeamRoute",

        "leagues/:leagueId/games/external": "gameExternalRoute",
        "leagues/:leagueId/games/new?*queryString": "newGameRoute",
        "leagues/:leagueId/games/:gameId": "gameInfoRoute",
        "leagues/:leagueId/games/:gameId/": "gameInfoRoute",
        "leagues/:leagueId/games/:gameId/edit": "editGameRoute",
        "leagues/:leagueId/games/:gameId/edit/": "editGameRoute",

        "leagues/:leagueId/calendar": "calendarRoute"
    },
    defaultRoute: function () {
        this.changeModule(this.app.submodules.leagues);
    },
    leagueStatisticsRoute: function (leagueId) {
        var options = { leagueId: leagueId };
        this.changeModule(this.app.submodules.statistics, options);
    },
    calendarRoute: function(leagueId) {
        var options = { leagueId: leagueId };
        this.changeModule(this.app.submodules.calendar, options);
    },
    newLeagueRoute: function (leagueId) {
        var options = { leagueId: leagueId };
        this.changeModule(this.app.submodules.league, options);
    },
    editLeagueRoute: function (leagueId) {
        var options = { leagueId: leagueId };
        this.changeModule(this.app.submodules.league, options);
    },
    tournamentInfoRoute: function (tournamentId) {
        var options = { tournamentId: tournamentId };
        this.changeModule(this.app.submodules.tournamentsInfo, options);
    },
    leagueInfoRoute: function (leagueId) {
        var options = { leagueId: leagueId };
        this.changeModule(this.app.submodules.leagueInfo, options);
    },
    tableRoute: function (leagueId) {
        var options = { leagueId: leagueId };
        this.changeModule(this.app.submodules.teamList, options);
    },
    newTeamRoute: function (leagueId) {
        var options = { leagueId: leagueId };
        this.changeModule(this.app.submodules.team, options);
    },
    editTeamRoute: function (leagueId, teamId) {
        var options = { leagueId: leagueId, teamId: teamId };
        this.changeModule(this.app.submodules.team, options);
    },
    infoTeamRoute: function (leagueId, teamId) {
        var options = { leagueId: leagueId, teamId: teamId };
        this.changeModule(this.app.submodules.teamInfo, options);
    },
    newGameRoute: function (leagueId, queryString) {
        var params = parseQueryString(queryString);
        var options = { leagueId: leagueId, eventId: params.event, homeTeamId: params.home, guestTeamId: params.guest };

        this.changeModule(this.app.submodules.game, options);
    },
    gameInfoRoute: function (leagueId, gameId) {
        var options = { leagueId: leagueId, gameId: gameId };
        this.changeModule(this.app.submodules.gameInfo, options);
    },
    gameExternalRoute: function (leagueId) {

        console.log('[router] GameExternalRoute: leagueId' + leagueId);

        var options = { leagueId: leagueId };
        this.changeModule(this.app.submodules.gameExternal, options);
    },
    editGameRoute: function (leagueId, gameId) {
        var options = { leagueId: leagueId, gameId: gameId };
        this.changeModule(this.app.submodules.game, options);
    },
    changeModule: function (module, options) {
        console.log('[router] Module has been changed: ', module);

        if (this.workingModule)
            this.workingModule.stop();

        this.workingModule = module;
        this.workingOptions = options;
        this.workingModule.start(this.workingOptions);
    },
    onRoute: function (name, path, args) {
        //console.log('[router] Route has been changed.');
    }
});

module.exports = MyRouter;