var GameInfo = Backbone.Model.extend({
    initialize: function () {
        this.leagueId = 0;
    },
    url: function () {
        return '/api/leagues/' + this.leagueId + '/games/' + this.id;
    },
    setLeagueId: function (leagueId) {
        this.leagueId = leagueId;
    },
    setId: function (id) {
        this.id = id;
    }
});

var gameExternalInfo = Backbone.Model.extend({
    initialize: function () {
        this.leagueId = 0;
    },
    url: function () {
        return '/api/leagues/' + this.leagueId + '/games/external';
    },
    setLeagueId: function (leagueId) {
        this.leagueId = leagueId;
    }
});

var gameExternalList = Backbone.Model.extend({
    initialize: function () {
        this.leagueId = 0;
    },
    url: function () {
        return '/api/leagues/' + this.leagueId + '/games/approval';
    },
    setLeagueId: function (leagueId) {
        this.leagueId = leagueId;
    }
});

module.exports = {
    GameInfo: GameInfo,
    gameExternalInfo: gameExternalInfo,
    gameExternalList: gameExternalList
};
