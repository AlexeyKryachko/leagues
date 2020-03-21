var _ = require('underscore');
var $ = require('jquery');

var Calendar = Backbone.Model.extend({
    initialize: function () {
        this.leagueId = 0;
    },
    url: function () {
        return '/api/leagues/' + this.leagueId + '/calendar';
    },
    setLeagueId: function (leagueId) {
        this.leagueId = leagueId;
    }
});

var Event = Backbone.Model.extend({
    initialize: function () {
        this.leagueId = 0;
    },
    url: function () {
        return this.id
            ? '/api/leagues/' + this.leagueId + '/events/' + this.id
            : '/api/leagues/' + this.leagueId + '/events/';
    },
    changeGuestTeam: function (eventGameId, val) {
        var games = this.get('games');

        _.each(games, function (obj) {
            if (obj.id == eventGameId) {
                obj.guestTeamId = val;
            }
        });

        this.set('games', games);
        this.save();
    },
    changeHomeTeam: function (eventGameId, val) {
        var games = this.get('games');

        _.each(games, function (obj) {
            if (obj.id == eventGameId) {
                obj.homeTeamId = val;
            }
        });

        this.set('games', games);
        this.save();
    },
    createGame: function() {
        var self = this;

        $.ajax({
            type: "POST",
            url: '/api/leagues/' + self.leagueId + '/events/' + this.id,
            success: function (eventGame) {
                var games = self.get('games');
                games.push(eventGame);
                self.set('games', games);
                self.trigger('change:games');
            }
        });
    },
    deleteGame: function (eventGameId) {
        var self = this;

        $.ajax({
            type: "DELETE",
            url: '/api/leagues/' + self.leagueId + '/events/' + this.id + '/' + eventGameId,
            success: function () {
                var newGames = [];
                var games = self.get('games');

                _.each(games, function (obj) {
                    if (eventGameId != obj.id)
                        newGames.push(obj);
                });

                self.set('games', newGames);
                self.trigger('change:games');
            }
        });
    },
    setLeagueId: function (leagueId) {
        this.leagueId = leagueId;
    }
});

var Events = Backbone.Collection.extend({
    url: function () {
        return '/api/leagues/' + this.leagueId + '/events';
    },
    model: Event,
    setLeagueId: function (leagueId) {
        this.leagueId = leagueId;
    }
});

module.exports = {
    Calendar: Calendar,
    Event: Event,
    Events: Events
}