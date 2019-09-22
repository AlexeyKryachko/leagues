var MyApp = require("../../app.js");
var _ = require('underscore');
var $ = require('jquery');

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
        'union': '.dashboard-list-item',
        'deleteLeague': '.delete-league',
        'editLeague': '.edit-league'
    },
    events: {
        'click @ui.union': 'unionRedirect',
        'click @ui.editLeague': 'editLeague',
        'click @ui.deleteLeague': 'deleteLeague'
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
    editLeague: function (e) {
        var id = $(e.currentTarget).data('id');

        document.location.href = '#leagues/' + id + '/edit';
    },
    deleteLeague: function (e) {
        var id = $(e.currentTarget).data('id');

        $.ajax({
            url: "/api/leagues/" + id,
            method: "DELETE"
        });
    },
    serializeData: function () {
        var model = this.model.toJSON();

        model.isAdmin = MyApp.Settings.isAdmin();

        _.each(model.leagues, function (obj) {
            obj.isAdmin = model.isAdmin;
        });

        return model;
    }
});

module.exports = {
    LeagueActions: LeagueActions,
    LeagueList: LeagueList
}