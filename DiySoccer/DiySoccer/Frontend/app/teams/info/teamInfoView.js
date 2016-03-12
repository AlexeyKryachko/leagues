﻿var TeamGameView = Backbone.Marionette.ItemView.extend({
    tagName: 'tr',
    template: "#team-game",
    ui: {
        'edit': '.edit-game',
        'delete': '.delete-game'
    },
    events: {
        'click @ui.edit': 'editRedirect',
        'click @ui.delete': 'deleteGame'
    },
    editRedirect: function() {
        this.trigger('game:edit', this.model);
    },
    deleteGame: function () {
        this.trigger('game:delete', this.model);
    },
    onShow: function () {
    },
    serializeData: function () {
        return this.model.toJSON();
    }
});

var TeamGamesView = Backbone.Marionette.CompositeView.extend({
    template: "#team-games",    
    childViewContainer: "tbody",
    childView: TeamGameView,
    emptyView: EmptyListView,
    childEvents: {
        'game:delete': 'deleteGame',
        'game:edit': 'editGame'
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
