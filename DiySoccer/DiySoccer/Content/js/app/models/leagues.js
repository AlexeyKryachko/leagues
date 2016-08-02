﻿var LeagueInfo = Backbone.Model.extend({
    url: function () {
        return '/api/leagues/' + this.id + '/info/';
    }
});

var League = Backbone.Model.extend({
    url: function() {
        return this.id 
            ? '/api/leagues/' + this.id
            : '/api/leagues';
    },
    create: function() {
        
    },
    update: function() {
        
    }
});

var LeaguesModel = Backbone.Model.extend({
    url: function() {
        return '/api/leagues';
    }
});

module.exports = {
    LeagueInfo: LeagueInfo,
    League: League,
    LeaguesModel: LeaguesModel
}