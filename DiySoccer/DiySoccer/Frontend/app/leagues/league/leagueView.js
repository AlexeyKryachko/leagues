var LeagueView = Backbone.Marionette.ItemView.extend({
    template: "#league",
    ui: {
        'name': '.league-name',
        'description': '.league-description'
    },
    events: {
        'change @ui.name': 'changeName',
        'change @ui.description': 'changeDescription'
    },
    changeName: function () {
        this.model.set('name', this.ui.name.val());
    },
    changeDescription: function () {
        this.model.set('description', this.ui.description.val());
    },
    modelEvents: {
        'sync': 'render'
    }
});