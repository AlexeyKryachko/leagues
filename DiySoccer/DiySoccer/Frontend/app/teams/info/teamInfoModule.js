var teamInfoModule = Marionette.Module.extend({
    startWithParent: false,

    initialize: function (options, app, object) {
        var self = this;

        self.app = app;

        self.games = new Backbone.Collection();
    },
    onStart: function (options) {
        var self = this;

        $.get('/api/leagues/' + options.leagueId + '/teams/' + options.teamId + '/info', function (response) {
            self.games.reset(response.games);

            self.createViews();
            self.bindViews();

            self.app.mainRegion.show(self.layout);
        });
    },
    
    createViews: function () {
        var self = this;

        self.layout = new LayoutView();
        self.teamInfoView = new TeamGamesView({ collection: self.games });
    },
    bindViews: function () {
        var self = this;

        self.listenTo(self.layout, 'show', function () {
            self.layout.center.show(self.teamInfoView);
        });
    },
    onStop: function (options) {
        var self = this;

        self.newTeamView.destroy();
        self.layout.destroy();
    }
});

MyApp.module("teamInfo", teamInfoModule);