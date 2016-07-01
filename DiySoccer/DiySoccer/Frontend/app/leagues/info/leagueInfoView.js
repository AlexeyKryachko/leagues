var LeagueInfoView = Backbone.Marionette.ItemView.extend({
    template: "#league-info",
    serializeData: function () {
        var model = this.model.toJSON();
        return model;
    },
    modelEvents: {
        'sync': 'render'
    }
});