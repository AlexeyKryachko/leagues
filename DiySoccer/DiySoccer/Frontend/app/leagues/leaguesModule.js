var leaguesModule = Marionette.Module.extend({
    startWithParent: false,

    initialize: function (options, app, object) {
        this.app = app;
    },

    onStart: function (options) {
        var self = this;

        if (!self.layout)
            self.layout = new LayoutView();

        var leagues = new Backbone.Collection([{ id: 1, name: 'SPB DIY League 2015-2016', description: 'Our league' }]);

        var leagueList = new LeagueList({ collection: leagues });

        this.listenTo(self.layout, 'show', function () {
            self.layout.bigRegion.show(leagueList);
        });

        self.app.mainRegion.show(self.layout);
    },

    onStop: function (options) {
    },
});

MyApp.module("leagues", leaguesModule);