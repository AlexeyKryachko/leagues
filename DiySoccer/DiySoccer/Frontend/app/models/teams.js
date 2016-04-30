var TeamInfo = Backbone.Model.extend({
    initialize: function () {
        this.leagueId = 0;
    },
    url: function() {
        return '/api/leagues/' + this.leagueId + '/teams/' + this.id;
    },
    setLeagueId: function (leagueId) {
        this.leagueId = leagueId;
    },
    setId: function (id) {
        this.id = id;
    }
});

var Teams = Backbone.Collection.extend({
    initialize: function() {
        this.leagueId = 0;
    },
    url: function() {
        return '/api/leagues/' + this.leagueId + '/teams/';
    },
    setLeagueId: function(leagueId) {
        this.leagueId = leagueId;
    }
});

var TeamsStatistic = Backbone.Model.extend({
    initialize: function() {
        this.leagueId = 0;
    },
    url: function() {
        return '/api/leagues/' + this.leagueId + '/statistic/';
    },
    setLeagueId: function(leagueId) {
        this.leagueId = leagueId;
    }
});