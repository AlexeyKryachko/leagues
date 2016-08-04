var LeagueInfoView = Backbone.Marionette.ItemView.extend({
    template: "#league-info",
    serializeData: function () {
        var model = this.model.toJSON();

        _.each(model.teams, function (obj, index) {
            obj.number = index + 1 + '.';
        });

        _.each(model.events, function (obj, index) {
            if (!obj.date)
                return;

            var date = new Date(obj.date);
            obj.date = date.toLocaleDateString();
        });

        return model;
    },
    modelEvents: {
        'sync': 'render'
    }
});

module.exports = { LeagueInfoView: LeagueInfoView }