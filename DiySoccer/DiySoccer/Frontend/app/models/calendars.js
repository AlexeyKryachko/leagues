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

var Events = Backbone.Collection.extend({
    Model: Event
});

var Event = Backbone.Model.extend({
    initialize: function () {
        this.leagueId = 0;
    },
    url: function () {
        return '/api/leagues/' + this.leagueId + '/events';
    },
    setLeagueId: function (leagueId) {
        this.leagueId = leagueId;
    }
});
