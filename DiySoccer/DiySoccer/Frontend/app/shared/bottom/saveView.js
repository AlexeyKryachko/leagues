var SaveView = Backbone.Marionette.ItemView.extend({
    template: "#save",
    ui: {
        'save': '.save',
        'cancel': '.cancel'

    },
    triggers: {
        'click @ui.save': 'save',
        'click @ui.cancel': 'cancel'
    }
});
