var MyApp = require("../../app.js");
var SharedViews = require("../../shared/views.js");
var $ = require('jquery');

var TeamGameView = Backbone.Marionette.ItemView.extend({
    tagName: 'tr',
    template: "#team-game",
    className: 'cursor-pointer',
    ui: {
        'edit': '.edit-game',
        'delete': '.delete-game'
    },
    events: {
        'click @ui.edit': 'editRedirect',
        'click @ui.delete': 'deleteGame',
        'click': 'viewRedirect'
    },
    viewRedirect: function() {
        document.location.href = '#leagues/' + this.options.leagueId + '/games/' + this.model.get('id');
    },
    editRedirect: function (e) {
        e.preventDefault();
        if (e.stopPropagation)
            e.stopPropagation();

        this.trigger('game:edit', this.model);
    },
    deleteGame: function (e) {
        e.preventDefault();
        if (e.stopPropagation)
            e.stopPropagation();

        this.trigger('game:delete', this.model);
    },
    setLeagueId: function(leagueId) {
        this.options.leagueId = leagueId;
    },
    onShow: function () {
    },
    serializeData: function () {
        var model = this.model.toJSON();

        model.isEditor = MyApp.Settings.isEditor(this.options.leagueId);

        return model;
    }
});

var TeamGamesView = Backbone.Marionette.CompositeView.extend({
    template: "#team-info",    
    childViewContainer: ".games-container",
    childView: TeamGameView,
    emptyView: SharedViews.EmptyListView,
    childEvents: {
        'game:delete': 'deleteGame',
        'game:edit': 'editGame'
    },
    onBeforeAddChild: function (childView) {
        childView.setLeagueId(this.options.leagueId);
    },
    editGame: function (view, model) {
        document.location.href = '#leagues/' + this.options.leagueId + '/games/' + model.get('id') + '/edit';
    },
    deleteGame: function (view, model) {
        var self = this;

        $.ajax({
            method: "DELETE",
            url: '/api/leagues/' + self.options.leagueId + '/games/' + model.get('id')
        }).done(function (response) {
            self.collection.remove(model);
        });
    },
    ui: {
    },
    initialize: function (options) {
        this.options = options;
    }
});

module.exports = {
    TeamGamesView: TeamGamesView
}