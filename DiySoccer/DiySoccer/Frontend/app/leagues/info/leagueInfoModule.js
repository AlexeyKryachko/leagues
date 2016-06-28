var leagueInfoModule = Marionette.Module.extend({
    startWithParent: false,

    initialize: function (options, app, object) {
        var self = this;

        self.app = app;

        self.leagueInfo = new LeagueInfo();
    },
    onStart: function (options) {
        var self = this;

        self.options = options;
        self.leagueInfo.clear();

        self.createViews();
        self.bindViews();

        if (options.leagueId) {
            self.leagueInfo.set('id', self.options.leagueId);
            self.leagueInfo.fetch();
        }

        self.app.mainRegion.show(self.layout);
    },
    createViews: function () {
        var self = this;

        self.layout = new LayoutView();
        self.leagueInfoView = new LeagueInfoView({ model: self.leagueInfo });
    },
    bindViews: function () {
        var self = this;
        
        self.listenTo(self.layout, 'show', function () {
            self.layout.center.show(self.leagueInfoView);
        });
    },
    onStop: function () {
        var self = this;

        self.leagueInfoView.destroy();
        self.layout.destroy();
    }
});

MyApp.module("leagueInfo", leagueInfoModule);