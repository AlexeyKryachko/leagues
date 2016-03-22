var HeaderView = Backbone.Marionette.ItemView.extend({
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
        model.vk = true;

        if (model.provider == 'vk') {
            model.logout = true;
            model.vk = false;
        }

        return model;
    },
    modelEvents: {
        'change': 'render'
    }
});
