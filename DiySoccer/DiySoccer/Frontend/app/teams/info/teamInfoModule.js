var teamInfoModule = Marionette.Module.extend({
    startWithParent: false,

    initialize: function (options, app, object) {
        var self = this;

        self.app = app;

        self.teamInfo = new TeamInfo();
        self.games = new Backbone.Collection();
    },
    onStart: function (options) {
        var self = this;
        self.options = options;

        self.teamInfo.clear();
        self.teamInfo.setId(self.options.teamId);
        self.teamInfo.setLeagueId(self.options.leagueId);
        self.games.reset();

        self.createViews();
        self.bindViews();

        self.teamInfo.fetch();
    },
    createViews: function () {
        var self = this;

        self.layout = new LayoutView();
        self.teamInfoView = new TeamGamesView({ model: self.teamInfo, collection: self.games, leagueId: self.options.leagueId });
        self.bottomView = new CancelView();
    },
    bindViews: function () {
        var self = this;

        self.listenTo(self.layout, 'show', function () {
            self.layout.center.show(self.teamInfoView);
            self.layout.down.show(self.bottomView);
        });

        self.listenTo(self.bottomView, 'cancel', function () {
            document.location.href = '#leagues/' + self.options.leagueId;
        });

        self.listenTo(self.teamInfo, 'sync', function () {
            self.games.reset(self.teamInfo.get('games'));
            self.app.mainRegion.show(self.layout);
        });
    },
    onStop: function (options) {
        var self = this;
        
        self.bottomView.destroy();
        self.teamInfoView.destroy();
        self.layout.destroy();
    }
});

MyApp.module("teamInfo", teamInfoModule);