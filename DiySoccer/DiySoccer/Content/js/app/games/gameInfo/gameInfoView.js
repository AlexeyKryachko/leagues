var SharedViews = require("../../shared/views.js");

var GameInfoScoresView = Backbone.Marionette.ItemView.extend({
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

var GameInfoView = Backbone.Marionette.CompositeView.extend({
    template: "#game-info",    
    childViewContainer: "tbody",
    childView: GameInfoScoresView,
    className: 'page',
    emptyView: SharedViews.EmptyListView,
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

module.exports = {
    GameInfoView: GameInfoView
}