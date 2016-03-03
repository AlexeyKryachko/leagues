﻿var Teams = Backbone.Collection.extend({
    initialize: function() {
        this.leagueId = 0;
    },
    url: function() {
        return '/api/teams/getTeamsByLeague/' + this.leagueId;
    },
    setLeagueId: function(leagueId) {
        this.leagueId = leagueId;
    }
})