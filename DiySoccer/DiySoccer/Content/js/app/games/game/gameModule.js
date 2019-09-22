var MyApp = require("../../app.js");
var Layouts = require("../../shared/layouts.js");
var SharedViews = require("../../shared/views.js");
var Views = require("./gameView.js");
var TeamModels = require("../../models/teams.js");
var EventModels = require("../../models/calendars.js");
var $ = require('jquery');

var gameModule = Backbone.Marionette.Module.extend({
    startWithParent: false,

    initialize: function (options, app, object) {
        var self = this;

        self.app = app;

        self.teams = new TeamModels.Teams();
        self.events = new EventModels.Events();

        self.optionsModel = new Backbone.Model();
        self.leftModel = new Backbone.Model();
        self.leftScores = new Backbone.Collection();
        self.rightModel = new Backbone.Model();
        self.rightScores = new Backbone.Collection();
    },
    _formData: function () {
        var self = this;

        var homeTeam = self.leftModel.toJSON();
        homeTeam.score = self.leftModel.get('score');
        homeTeam.bestId = self.leftModel.get('bestId');
        homeTeam.members = self.leftScores.toJSON();

        var guestTeam = self.rightModel.toJSON();
        guestTeam.score = self.rightModel.get('score');
        guestTeam.bestId = self.rightModel.get('bestId');
        guestTeam.members = self.rightScores.toJSON();

        return {
            customScores: self.optionsModel.get('customScores'),
            eventId: self.optionsModel.get('eventId'),
            homeTeam: homeTeam,
            guestTeam: guestTeam
        }
    },
    _createGame: function () {
        var self = this;
        var data = self._formData();

        $.ajax({
            type: "POST",
            url: '/api/leagues/' + self.options.leagueId + '/games',
            data: data,
            success: function () {
                window.history.back();
            }
        });
    },
    _updateGame: function () {
        var self = this;
        var data = self._formData();

        $.ajax({
            type: "PUT",
            url: '/api/leagues/' + self.options.leagueId + '/games/' + self.options.gameId,
            data: data,
            success: function () {
                window.history.back();
            }
        });
    },
    onStart: function (options) {
        var self = this;

        self.options = options;

        if (options.gameId) {
            self._onEdit();
        } else {
            self._onNew();
        }

    },
    _onNew: function () {
        var self = this;
        
        self.optionsModel.clear();
        if (self.options.homeTeamId)
            self.optionsModel.set('eventId', self.options.eventId);

        self.leftModel.clear();
        self.leftModel.set('disableScoreValue', true);
        if (self.options.homeTeamId) {
            self.leftModel.set('id', self.options.homeTeamId);
            $.get('/api/leagues/' + self.options.leagueId + '/teams/' + self.options.homeTeamId + '/info', function (response) {
                self.leftScores.reset(response.players);
            });
        }

        self.rightModel.clear();
        self.rightModel.set('disableScoreValue', true);
        if (self.options.guestTeamId) {
            self.rightModel.set('id', self.options.guestTeamId);
            $.get('/api/leagues/' + self.options.leagueId + '/teams/' + self.options.guestTeamId + '/info', function (response) {
                self.rightScores.reset(response.players);
            });
        }

        self.rightScores.reset();
        self.leftScores.reset();

        self.teams.setLeagueId(self.options.leagueId);
        self.events.setLeagueId(self.options.leagueId);

        self.createViews();
        self.bindViews();

        self.teams.fetch();
        self.events.fetch();

        self.app.mainRegion.show(self.layout);
    },
    _onEdit: function () {
        var self = this;

        $.get('/api/leagues/' + self.options.leagueId + '/games/' + self.options.gameId + '/info', function (response) {
            self.leftModel.set('id', response.homeTeam.id);
            self.leftModel.set('score', response.homeTeam.score);
            self.leftModel.set('bestId', response.homeTeam.bestId);

            self.rightModel.set('id', response.guestTeam.id);
            self.rightModel.set('score', response.guestTeam.score);
            self.rightModel.set('bestId', response.guestTeam.bestId);

            self.leftScores.reset(response.homeTeam.members);
            self.rightScores.reset(response.guestTeam.members);

            self.teams.setLeagueId(self.options.leagueId);

            self.optionsModel.set('customScores', response.customScores);
            self.optionsModel.set('eventId', response.eventId);
            self.optionsModel.set('events', response.events);

            self.rightModel.set('disableScoreValue', !response.customScores);
            self.leftModel.set('disableScoreValue', !response.customScores);

            self.createViews();
            self.bindViews();

            self.teams.fetch();

            self.app.mainRegion.show(self.layout);

        });
    },
    createViews: function () {
        var self = this;

        self.layout = new Layouts.SplittedLayout();
        self.leftGameView = new Views.GameView({
            model: self.leftModel,
            collection: self.leftScores,
            teams: self.teams,
            leagueId: self.options.leagueId
        });
        self.rightGameView = new Views.GameView({
            model: self.rightModel,
            collection: self.rightScores,
            teams: self.teams,
            leagueId: self.options.leagueId
        });
        self.optionsView = new Views.GameOptionsView({
            model: self.optionsModel,
            leagueId: self.options.leagueId
        });
        self.saveView = new SharedViews.SaveView();
    },
    bindViews: function () {
        var self = this;
        
        self.listenTo(self.layout, 'show', function () {
            self.layout.up.show(self.optionsView);
            self.layout.left.show(self.leftGameView);
            self.layout.right.show(self.rightGameView);
            self.layout.down.show(self.saveView);
        });

        self.listenTo(self.optionsView, 'scoring:changed', self.scoringShowing);

        self.listenTo(self.saveView, 'save', function () {
            if (self.options.gameId) {
                self._updateGame();
            } else {
                self._createGame();
            }
        });

        self.listenTo(self.saveView, 'cancel', function () {
            window.history.back();
        });

        self.listenTo(self.teams, 'sync', function () {
            self.leftGameView.render();
            self.rightGameView.render();
        });

        self.listenTo(self.events, 'sync', function () {
            self.optionsModel.set('events', self.events.toJSON());
            self.optionsView.render();
        });
    },
    scoringShowing: function (disabled) {
        var self = this;
        self.rightModel.set('disableScoreValue', !disabled);
        self.leftModel.set('disableScoreValue', !disabled);
    },
    onStop: function (options) {
        var self = this;

        self.optionsView.destroy();
        self.saveView.destroy();
        self.leftGameView.destroy();
        self.rightGameView.destroy();
        self.layout.destroy();
    }
});

MyApp.module("game", gameModule);