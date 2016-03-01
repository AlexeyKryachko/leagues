var tableModule = Marionette.Module.extend({
    startWithParent: false,

    initialize: function (options, app, object) {
        this.app = app;
    },

    onStart: function (options) {
        console.log('[table] started')
        if (!this.layout)
            this.layout = new leaguesLayoutView();

        var teams = new Backbone.Collection([
            { id: 1, name: 'нюен', matchesCount: 13, wins: 10, draw: 3, lose: 0, goals: '135-38(+97)', points: 33 },
            { id: 2, name: 'борисенки', matchesCount: 13, wins: 11, draw: 0, lose: 2, goals: '133-62(+71)', points: 33 },
            { id: 3, name: 'гидраер', matchesCount: 13, wins: 11, draw: 0, lose: 2, goals: '112-42(+70)', points: 33 },
        ]);

        var tableView = new TableView({ collection: teams });

        this.listenTo(this.layout, 'show', function () {
            this.layout.bigRegion.show(tableView)
        });

        this.app.mainRegion.show(this.layout);        
    },

    onStop: function (options) {
        console.log('[table] stopped');
    },
});

MyApp.module("table", tableModule);