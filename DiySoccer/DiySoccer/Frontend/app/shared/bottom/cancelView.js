var CancelView = Backbone.Marionette.ItemView.extend({
    template: "#cancel",
    ui: {
        'cancel': '.cancel'
    },
    triggers: {
        'click @ui.cancel': 'cancel'
    }
});
