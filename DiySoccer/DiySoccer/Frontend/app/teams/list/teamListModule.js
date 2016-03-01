var teamsModule = Marionette.Module.extend({
    startWithParent: false,

    initialize: function (options, app, object) {
        this.app = app;

        this.teams = new Backbone.Collection([
            { id: 1, name: 'нюен', matchesCount: 13, wins: 10, draw: 3, lose: 0, goals: '135-38(+97)', points: 33 },
            { id: 2, name: 'борисенки', matchesCount: 13, wins: 11, draw: 0, lose: 2, goals: '133-62(+71)', points: 33 },
            { id: 3, name: 'гидраер', matchesCount: 13, wins: 11, draw: 0, lose: 2, goals: '112-42(+70)', points: 33 },
        ]);
    },

    onStart: function (options) {
        var self = this;

        console.log('[table] started');
        if (!self.layout)
            self.layout = new LayoutView();
        
        var tableView = new TeamListView({ collection: self.teams, leagueId: options.leagueId });

        self.listenTo(self.layout, 'show', function () {
            self.layout.bigRegion.show(tableView);
        });

        self.app.mainRegion.show(self.layout);
    },

    onStop: function (options) {
        console.log('[table] stopped');
    },
});

MyApp.module("teamList", teamsModule);