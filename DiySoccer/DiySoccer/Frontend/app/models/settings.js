var Settings = Backbone.Model.extend({
    initialize: function() {
    },
    urlRoot: "/api/settings",
    loginVK: function () {
    },
    logout: function () {
        var self = this;
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
    }
});