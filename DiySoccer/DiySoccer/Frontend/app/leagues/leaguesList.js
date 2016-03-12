var leagueView = Backbone.Marionette.ItemView.extend({
    template: "#leagues-item",
    onShow: function () {
    }
});

var LeagueList = Backbone.Marionette.CompositeView.extend({
    template: "#leagues-list",
    childViewContainer: ".leagues-container",
    childView: leagueView,
    initialize: function (options) {        
    }
});