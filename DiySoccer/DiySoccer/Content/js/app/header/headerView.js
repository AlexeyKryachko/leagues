var headerView = Backbone.Marionette.ItemView.extend({
    template: "#header",
    ui: {
        'vkLogin': '.btn-vk-login',
        'logout': '.btn-logout'
    },
    triggers: {
        'click @ui.vkLogin': 'vkLogin',
        'click @ui.logout': 'logout'
    },
    serializeData: function() {
        var model = this.model.toJSON();

        if (model.permissions.isAuthenticated) {
            model.logout = true;
            model.vk = false;
        } else {
            model.logout = false;
            model.vk = true;
        }

        model.url = document.location.hash.replace('#', '');

        return model;
    },
    modelEvents: {
        'change': 'render'
    }
});

module.exports = headerView;
