var TeamListItemView = Backbone.Marionette.ItemView.extend({
    tagName: 'tr',
    className: 'cursor-pointer',
    template: "#team-item",
    events:{
        'click': 'onRedirect',
        'click .edit-team': 'editTeam'
    },
    editTeam: function () {
        document.location.href = document.location.href + '/teams/' + this.model.get('id') + '/edit';
    },
    onRedirect: function () {
        document.location.href = document.location.href + '/teams/' + this.model.get('id');
    },
    onShow: function () {
    },
    serializeData: function () {
        var model = this.model.toJSON();
        model.goals = model.scores + '-' + model.missed + '(' + (model.scores - model.missed) + ')';
        return model;
    }
});

var TeamListView = Backbone.Marionette.CompositeView.extend({
    template: "#team-list",    
    childViewContainer: "tbody",
    childView: TeamListItemView,
    emptyView: EmptyListView,
    initialize: function (options) {
        this.options = {
            leagueId: options.leagueId
        }
    }
});

var TeamListActions = Backbone.Marionette.CompositeView.extend({
    template: "#team-list-actions",
    ui: {
        'addBtn': '.add-new-team',
        'addGame': '.add-new-game'
    },
    events: {
        'click @ui.addBtn': 'redirectAddTeam',
        'click @ui.addGame': 'redirectAddGame'
    },
    initialize: function (options) {
        this.options = {
            leagueId: options.leagueId
        }
    },
    redirectAddTeam: function (e) {
        document.location.href = "#leagues/" + this.options.leagueId + "/teams/new";
    },
    redirectAddGame: function (e) {
        document.location.href = "#leagues/" + this.options.leagueId + "/games/new";
    }
});