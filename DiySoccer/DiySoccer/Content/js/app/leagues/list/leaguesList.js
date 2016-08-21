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

        var tournament = _.findWhere(this.model.get('tournaments'), { id: id });
        if (tournament) {
            document.location.href = '#tournaments/' + id;
        } else {
            document.location.href = '#leagues/' + id;
        }
    },
    serializeData: function () {
        var model = this.model.toJSON();

        model.isAdmin = MyApp.Settings.isAdmin();

        return model;
    }
});

module.exports = {
    LeagueActions: LeagueActions,
    LeagueList: LeagueList
}