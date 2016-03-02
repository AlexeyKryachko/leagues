var GameNewScoresView = Backbone.Marionette.ItemView.extend({
    tagName: 'tr',
    className: 'cursor-pointer',
    template: "#game-new-member",
    ui: {
    },
    onShow: function () {
        
    },
    serializeData: function () {
        return this.model.toJSON();
    }
});

var GameNewView = Backbone.Marionette.CompositeView.extend({
    template: "#game-new",    
    childViewContainer: "tbody",
    childView: GameNewScoresView,
    emptyView: EmptyListView,
    ui: {
        'changeTeam': '.change-team'
    },
    events: {
        'change @ui.changeTeam': 'changeTeam'
    },
    triggers: {
        'click @ui.submit': 'submit'
    },
    changeTeam: function () {
        var team = _.findWhere(this.teams, { id: this.ui.changeTeam.val() });
        this.collection.reset(team.members);
    },
    initialize: function (options) {
        this.teams = options.teams;
    },
    serializeData: function() {
        var model = this.model.toJSON();
        model.teams = [];
        _.each(this.teams, function (team) {
            model.teams.push({ id: team.id, name: team.name, selected: id == model.selected });
        });
        return model;
    }
});