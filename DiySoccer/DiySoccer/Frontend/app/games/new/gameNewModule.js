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

        self.createViews();
        self.bindViews();

        self.teams.setLeagueId(self.options.leagueId);
        self.teams.fetch();

        self.app.mainRegion.show(self.layout);
    },
    createViews: function () {
        var self = this;

        self.layout = new SplittedLayout();
        self.leftGameView = new GameNewView({ model: self.leftModel, collection: self.leftScores, teams: self.teams });
        self.rightGameView = new GameNewView({ model: self.rightModel, collection: self.rightScores, teams: self.teams });
        self.saveView = new SaveView();
    },
    bindViews: function () {
        var self = this;

        self.listenTo(self.layout, 'show', function () {

            self.layout.left.show(self.leftGameView);
            self.layout.right.show(self.rightGameView);
            self.layout.down.show(self.saveView);
        });

        self.listenTo(self.saveView, 'save', function () {

            var homeTeam = self.leftModel.toJSON();
            homeTeam.members = self.leftScores.toJSON();

            var guestTeam = self.rightModel.toJSON();
            guestTeam.members = self.rightScores.toJSON();

            var data = {
                leagueId: self.options.leagueId,
                homeTeam: homeTeam,
                guestTeam: guestTeam
            }

            console.log('Created game: ', data);
            $.ajax({
                type: "POST",
                url: "/api/games",
                data: data,
                success: function () {
                    document.location.href = '#leagues/' + self.options.leagueId;
                }
            });
        });

        self.listenTo(self.teams, 'sync', function () {
            self.leftModel.clear();
            self.leftModel.trigger('change');
            self.leftScores.reset();
            self.rightModel.clear();
            self.rightModel.trigger('change');
            self.rightScores.reset();
        });
    },
    onStop: function (options) {
        var self = this;

        self.saveView.destroy();
        self.leftGameView.destroy();
        self.rightGameView.destroy();
    }
});

MyApp.module("gameNew", gameNewModule);