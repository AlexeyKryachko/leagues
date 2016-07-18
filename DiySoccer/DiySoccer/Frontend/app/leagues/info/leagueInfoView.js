var LeagueInfoView = Backbone.Marionette.ItemView.extend({
    template: "#league-info",
    serializeData: function () {
        var model = this.model.toJSON();

        _.each(model.teams, function (obj, index) {
            obj.number = index + 1 + '.';
        });

        return model;
    },
    modelEvents: {
        'sync': 'render'
    }
});