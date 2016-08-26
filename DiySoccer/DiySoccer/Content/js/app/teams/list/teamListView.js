var MyApp = require("../../app.js");
var SharedViews = require("../../shared/views.js");

var TeamListItemView = Backbone.Marionette.ItemView.extend({
    initialize: function() {
        this.options = {};
    },
    tagName: 'tr',
    className: 'cursor-pointer',
    template: "#team-item",
    events:{
        'click': 'onRedirect',
        'click .edit-team': 'editTeam'
    },
    editTeam: function (e) {
        e.preventDefault();
        if (e.stopPropagation)
            e.stopPropagation();
        document.location.href = '#leagues/' + this.options.leagueId + '/teams/' + this.model.get('id') + '/edit';
    },
    onRedirect: function () {
        document.location.href = '#leagues/' + this.options.leagueId + '/teams/' + this.model.get('id');
    },
    onShow: function () {
    },
    serializeData: function () {
        var model = this.model.toJSON();

        model.goals = model.scores + '-' + model.missed + ' (' + (model.scores - model.missed) + ')';
        model.showEdit = MyApp.Settings.isEditor(this.options.leagueId);
        
        return model;
    },
    setLeagueId: function(leagueId) {
        this.options.leagueId = leagueId;
    }
});

var TeamListView = Backbone.Marionette.CompositeView.extend({
    template: "#team-list",    
    childViewContainer: "tbody",
    childView: TeamListItemView,
    emptyView: SharedViews.EmptyListView,
    className: 'page',
    initialize: function (options) {
        this.options = options;
    },
    onBeforeAddChild: function(childView) {
        childView.setLeagueId(this.options.leagueId);
    },
    modelEvents: {
        'sync': 'render'
    },
    collectionEvents: {
        'sync': 'render'
    },
    serializeData: function() {
        var model = this.model.toJSON();

        model.showEditColumn = MyApp.Settings.isEditor(this.options.leagueId);
        
        return model;
    }
});

var TeamListActions = Backbone.Marionette.CompositeView.extend({
    template: "#team-list-actions",
    ui: {
        'addBtn': '.add-new-team',
        'addGame': '.add-new-game',
        'calendarLink': '.calendar-link'
    },
    events: {
        'click @ui.addBtn': 'redirectAddTeam',
        'click @ui.addGame': 'redirectAddGame',
        'click @ui.calendarLink': 'redirectCalendar'
    },
    initialize: function (options) {
        this.options = options;
    },
    redirectAddTeam: function (e) {
        document.location.href = "#leagues/" + this.options.leagueId + "/teams/new";
    },
    redirectAddGame: function (e) {
        document.location.href = "#leagues/" + this.options.leagueId + "/games/new";
    },
    redirectCalendar: function (e) {
        document.location.href = "#leagues/" + this.options.leagueId + "/calendar";
    },
    serializeData: function() {
        var model = {};

        model.isEditor = MyApp.Settings.isEditor(this.options.leagueId);

        return model;
    }
});

module.exports = {
    TeamListActions: TeamListActions,
    TeamListView: TeamListView
}