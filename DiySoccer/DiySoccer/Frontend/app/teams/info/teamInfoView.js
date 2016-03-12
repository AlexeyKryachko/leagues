var TeamGameView = Backbone.Marionette.ItemView.extend({
    tagName: 'tr',
    template: "#team-game",
    ui: {
    },
    events: {
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
    ui: {
    },
    initialize: function (options) {
    }
});