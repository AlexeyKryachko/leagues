var LeagueList = Backbone.Marionette.ItemView.extend({
    template: "#leagues-list",
    initialize: function (options) {
        this.options = options;
    },
    serializeData: function () {
        var model = this.model.toJSON();
        model.isAdmin = MyApp.Settings.isAdmin();

        if (!model.isAdmin)
            return model;

        if (model.leagues) {
            _.each(model.leagues, function (obj) {
                obj.href = '#leagues/' + obj.id + '/edit';
            });
        }
        if (model.tournaments) {
            _.each(model.tournaments, function (obj) {
                obj.href = '#leagues/' + obj.id + '/edit';
            });
        }
        return model;
    }
});