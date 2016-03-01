var leaguesModule = Marionette.Module.extend({
    startWithParent: false,

    initialize: function (options, app, object) {
        this.app = app;
    },

    onStart: function (options) {
        console.log('[leagues] started')
        if (!this.layout)
            this.layout = new leaguesLayoutView();

        var leagues = new Backbone.Collection([{ id: 1, name: 'SPB DIY League 2015-2016', description: 'Our league' }]);

        var leagueList = new LeagueList({ collection: leagues })

        this.listenTo(this.layout, 'show', function () {
            this.layout.bigRegion.show(leagueList)
        });

        this.app.mainRegion.show(this.layout);        
    },

    onStop: function (options) {
        console.log('[leagues] stopped');
    },
});

MyApp.module("leagues", leaguesModule);