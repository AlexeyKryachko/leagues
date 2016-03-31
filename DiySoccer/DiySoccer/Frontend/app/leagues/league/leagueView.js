var LeagueView = Backbone.Marionette.ItemView.extend({
    template: "#league",
    ui: {
        'name': '.league-name',
        'description': '.league-description',
        'group': '.league-vkGroup'
    },
    events: {
        'change @ui.name': 'changeName',
        'change @ui.description': 'changeDescription',
        'change @ui.group': 'changeGroup'
    },
    changeName: function () {
        this.model.set('name', this.ui.name.val());
    },
    changeDescription: function () {
        this.model.set('description', this.ui.description.val());
    },
    changeGroup: function () {
        this.model.set('vkGroup', this.ui.group.val());
    },
    modelEvents: {
        'sync': 'render'
    }
});