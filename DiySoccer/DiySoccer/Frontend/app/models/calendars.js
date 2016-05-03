var Calendar = Backbone.Collection.extend({
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
