var TeamInfo = Backbone.Model.extend({
    initialize: function () {
        this.leagueId = 0;
    },
    url: function () {
        return '/api/leagues/' + this.leagueId + '/teams/' + this.id + '/info';
    },
    setLeagueId: function (leagueId) {
        this.leagueId = leagueId;
    },
    setId: function (id) {
        this.id = id;
    }
});

var Team = Backbone.Model.extend({
    initialize: function () {
        this.leagueId = 0;
    },
    url: function() {
        return this.id 
            ? '/api/leagues/' + this.leagueId + '/teams/' + this.id
            : '/api/leagues/' + this.leagueId + '/teams/';
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
        return '/api/leagues/' + this.leagueId + '/table/';
    },
    setLeagueId: function(leagueId) {
        this.leagueId = leagueId;
    }
});

module.exports = {
    TeamInfo: TeamInfo,
    Team: Team,
    Teams: Teams,
    TeamsStatistic: TeamsStatistic
}