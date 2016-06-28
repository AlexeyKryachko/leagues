var LeagueInfoView = Backbone.Marionette.ItemView.extend({
    template: "#league-info",
    serializeData: function () {
        return this.model.toJSON();
    },
    modelEvents: {
        'sync': 'render'
    }
});