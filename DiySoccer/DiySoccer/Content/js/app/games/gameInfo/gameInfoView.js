var SharedViews = require("../../shared/views.js");

var GameInfoScoresView = Backbone.Marionette.ItemView.extend({
    tagName: 'tr',
    template: "#game-info-scores"
});

var GameInfoView = Backbone.Marionette.CompositeView.extend({
    template: "#game-info",    
    childViewContainer: "tbody",
    childView: GameInfoScoresView,
    className: 'page',
    emptyView: SharedViews.EmptyListView
});

module.exports = {
    GameInfoView: GameInfoView
}