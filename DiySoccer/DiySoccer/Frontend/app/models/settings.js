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
    }
});