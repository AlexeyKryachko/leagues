var SaveView = Backbone.Marionette.ItemView.extend({
    template: "#save",
    ui: {
        'save': '.save'
    },
    triggers: {
        'click @ui.save': 'save'
    }
});
