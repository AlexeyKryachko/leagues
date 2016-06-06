var GameScoresView = Backbone.Marionette.ItemView.extend({
    tagName: 'tr',
    template: "#game-info-scores",
    ui: {
    },
    events: {
    },
    changeScore: function() {
    },
    changeHelp: function () {
    },
    onShow: function () {
    }
});

var GameView = Backbone.Marionette.CompositeView.extend({
    template: "#game-info",    
    childViewContainer: "tbody",
    childView: GameScoresView,
    emptyView: EmptyListView,
    ui: {
    },
    events: {
    },
    triggers: {
    },
    initialize: function () {
    },
    onRender: function () {
        
    }
});