var gameNewModule = Marionette.Module.extend({
    startWithParent: false,

    initialize: function (options, app, object) {
        var self = this;

        self.app = app;

        self.teams = new Teams();

        self.leftModel = new Backbone.Model();
        self.leftScores = new Backbone.Collection();
        self.rightModel = new Backbone.Model();
        self.rightScores = new Backbone.Collection();

        self.layout = new SplittedLayout();
        self.leftGameView = new GameNewView({ model: self.leftModel, collection: self.leftScores, teams: self.teams });
        self.rightGameView = new GameNewView({ model: self.rightModel, collection: self.rightScores, teams: self.teams });

        self.listenTo(self.layout, 'show', function () {
            self.layout.leftRegion.show(self.leftGameView);
            self.layout.rightRegion.show(self.rightGameView);
        });

        self.listenTo(self.teams, 'sync', function () {
            self.leftModel.clear();
            self.leftScores.reset();
            self.rightModel.clear();
            self.rightScores.reset();
        });
    },
    createGame: function () {
        $.ajax({
            type: "POST",
            url: "/api/teams",
            data: data,
            success: function() {
                document.location.href = '#leagues/' + this.options.leagueId;
            }
        });
    },
    onStart: function (options) {
        var self = this;

        self.options = options;

        self.teams.fetch();

        self.app.mainRegion.show(self.layout);
    },
    onStop: function (options) {
    }
});

MyApp.module("gameNew", gameNewModule);