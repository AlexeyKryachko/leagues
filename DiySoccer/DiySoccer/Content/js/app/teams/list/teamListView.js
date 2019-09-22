var MyApp = require("../../app.js");
var SharedViews = require("../../shared/views.js");
var $ = require('jquery');

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
        'calendarLink': '.calendar-link',
        'findTeam': '.import-team-input',
        'importTeam': '.import-team-button'
    },
    events: {
        'click @ui.addBtn': 'redirectAddTeam',
        'click @ui.calendarLink': 'redirectCalendar',
        'click @ui.importTeam': 'importTeam'
    },
    initialize: function (options) {
        this.options = options;
    },
    importTeam: function () {
        var val = $(this.ui.importTeam).data('teamId');

        if (!val || val == '')
            return;

        var self = this;
        $.ajax({
            type: "POST",
            url: '/api/unions/' + this.options.leagueId + '/teams/' + val + '/copy/',
            success: function (data, textStatus, xhr) {
                self.ui.findTeam.val('');
            }
        });
    },
    redirectAddTeam: function (e) {
        document.location.href = "#leagues/" + this.options.leagueId + "/teams/new";
    },
    redirectCalendar: function (e) {
        document.location.href = "#leagues/" + this.options.leagueId + "/calendar";
    },
    onRender: function () {
        var self = this;

        $(this.ui.findTeam).typeahead({
            source: function (query, process) {
                var url = '/api/teams/search?page=0&pageSize=10';
                return $.get(url, { query: query }, function (response) {
                    return process(response);
                });
            },
            displayText: function (item) {
                return item.name;
            },
            updater: function (item) {
                $(self.ui.importTeam).data('teamId', item.id);
                return item.name;
            }
        });
    },
    serializeData: function() {
        var model = {};

        model.isEditor = MyApp.Settings.isEditor(this.options.leagueId);

        return model;
    },
    modelEvents: {
        'sync': 'render'
    }
});

module.exports = {
    TeamListActions: TeamListActions,
    TeamListView: TeamListView
}