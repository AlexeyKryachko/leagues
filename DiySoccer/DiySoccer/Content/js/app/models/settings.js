var $ = require('jquery');

var settings = Backbone.Model.extend({
    initialize: function() {
    },
    urlRoot: "/api/settings",
    loginVK: function () {
    },
    logout: function () {
        var self = this;
        self.deleteCookie('.AspNet.ExternalCookie');
        $.get('/api/logout', function() {
            self.trigger('needFetch');
        });
    },
    isEditor(leagueId) {
        var permission = this.get('permissions').relationships[leagueId];
        return permission == '2' || permission == '3';
    },
    isAdmin(leagueId) {
        var permission = this.get('permissions');
        return permission.isAdmin;
    },
    deleteCookie( name ) {
        document.cookie = "username=" + name + "; expires=Thu, 01 Jan 1970 00:00:00 UTC";
    }
});

module.exports = settings;