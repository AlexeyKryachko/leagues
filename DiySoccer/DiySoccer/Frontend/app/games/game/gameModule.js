var gameModule = Marionette.Module.extend({
    startWithParent: false,

    initialize: function (options, app, object) {
        var self = this;

        self.app = app;

        self.teams = new Teams();

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
            success: function() {
                document.location.href = '#leagues/' + self.options.leagueId;
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
                document.location.href = '#leagues/' + self.options.leagueId;
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

        self.leftModel.clear();
        self.leftModel.set('disableScoreValue', true);
        self.leftScores.reset();
        self.rightModel.clear();
        self.rightModel.set('disableScoreValue', true);
        self.rightScores.reset();
        self.teams.setLeagueId(self.options.leagueId);

        self.createViews();
        self.bindViews();

        self.teams.fetch();

        self.app.mainRegion.show(self.layout);
    },
    _onEdit: function () {
        var self = this;

        $.get('/api/leagues/' + self.options.leagueId + '/games/' + self.options.gameId + '/info', function (response) {
            self.leftModel.set('id', response.homeTeam.id);
            self.leftModel.set('score', response.homeTeam.score);
            self.leftModel.set('bestId', response.homeTeam.bestId);
            self.leftScores.reset(response.homeTeam.members);
            self.rightModel.set('id', response.guestTeam.id);
            self.rightModel.set('score', response.guestTeam.score);
            self.rightModel.set('bestId', response.guestTeam.bestId);
            self.rightScores.reset(response.guestTeam.members);
            self.teams.setLeagueId(self.options.leagueId);
            self.optionsModel.set('customScores', response.customScores);

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

        self.layout = new SplittedLayout();
        self.leftGameView = new GameView({ model: self.leftModel, collection: self.leftScores, teams: self.teams, leagueId: self.options.leagueId });
        self.rightGameView = new GameView({ model: self.rightModel, collection: self.rightScores, teams: self.teams, leagueId: self.options.leagueId });
        self.optionsView = new GameOptionsView({ model: self.optionsModel });
        self.saveView = new SaveView();
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
            document.location.href = '#leagues/' + self.options.leagueId;
        });

        self.listenTo(self.teams, 'sync', function () {
            self.leftGameView.render();
            self.rightGameView.render();
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