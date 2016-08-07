var MyApp = require("../../app.js");

var LeagueActions = Backbone.Marionette.ItemView.extend({
    template: "#leagues-actions",
    ui: {
        'addLeague': '.add-league'
    },
    events: {
        'click @ui.addLeague': 'addLeague'
    },
    addLeague: function () {
        document.location.href = '#leagues/new';
    },
    serializeData: function () {
        var model = {};

        model.showAdd = MyApp.Settings.isAdmin();

        return model;
    }
});

var LeagueList = Backbone.Marionette.ItemView.extend({
    template: "#leagues-list",
    initialize: function (options) {
        this.options = options;
    },
    ui: {
        'union': '.dashboard-list-item'
    },
    events: {
        'click @ui.union': 'unionRedirect'
    },
    unionRedirect: function(e) {
        var id = $(e.currentTarget).data('id');
        document.location.href = '#leagues/' + id;
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

module.exports = {
    LeagueActions: LeagueActions,
    LeagueList: LeagueList
}