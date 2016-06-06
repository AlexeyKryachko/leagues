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
