var MyApp = require("../../app.js");
var Layouts = require("../../shared/layouts.js");
var SharedViews = require("../../shared/views.js");
var Views = require("./gameInfoView.js");
var Models = require("../../models/games.js");

var gameInfoModule = Backbone.Marionette.Module.extend({
    startWithParent: false,

    initialize: function (options, app, object) {
        var self = this;

        self.app = app;
        
        self.leftModel = new Backbone.Model();
        self.leftScores = new Backbone.Collection();
        self.rightModel = new Backbone.Model();
        self.rightScores = new Backbone.Collection();

        self.gameInfo = new Models.GameInfo();
    },
    onStart: function (options) {
        var self = this;

        self.options = options;

        self.leftModel.clear();
        self.leftScores.reset();
        self.rightModel.clear();
        self.rightScores.reset();

        self.createViews();
        self.bindViews();

        self.gameInfo.setLeagueId(self.options.leagueId);
        self.gameInfo.setId(self.options.gameId);
        self.gameInfo.fetch();
        self.app.mainRegion.show(self.layout);
    },
    createViews: function () {
        var self = this;

        self.layout = new Layouts.SplittedLayout();
        self.leftGameView = new Views.GameInfoView({ model: self.leftModel, collection: self.leftScores, teams: self.teams, leagueId: self.options.leagueId });
        self.rightGameView = new Views.GameInfoView({ model: self.rightModel, collection: self.rightScores, teams: self.teams, leagueId: self.options.leagueId });
        self.cancelView = new SharedViews.CancelView();
    },
    bindViews: function () {
        var self = this;
        
        self.listenTo(self.layout, 'show', function () {
            self.layout.left.show(self.leftGameView);
            self.layout.right.show(self.rightGameView);
            self.layout.down.show(self.cancelView);
        });

        self.listenTo(self.cancelView, 'cancel', function () {
            window.history.back();
        });

        self.listenTo(self.gameInfo, 'sync', function () {
            self.leftModel.set('name', self.gameInfo.get('homeTeamName'));
            self.leftModel.set('score', self.gameInfo.get('homeTeamScore'));
            self.leftModel.set('mediaId', self.gameInfo.get('homeTeamMediaId'));
            self.leftModel.set('best', self.gameInfo.get('homeTeamBest'));
            self.leftScores.reset(self.gameInfo.get('homeTeamScores'));
            self.leftGameView.render();

            self.rightModel.set('name', self.gameInfo.get('guestTeamName'));
            self.rightModel.set('score', self.gameInfo.get('guestTeamScore'));
            self.rightModel.set('mediaId', self.gameInfo.get('guestTeamMediaId'));
            self.rightModel.set('best', self.gameInfo.get('guestTeamBest'));
            self.rightScores.reset(self.gameInfo.get('guestTeamScores'));
            self.rightGameView.render();
        });
    },
    onStop: function (options) {
        var self = this;

        self.cancelView.destroy();
        self.leftGameView.destroy();
        self.rightGameView.destroy();
        self.layout.destroy();
    }
});

MyApp.module("gameInfo", gameInfoModule);