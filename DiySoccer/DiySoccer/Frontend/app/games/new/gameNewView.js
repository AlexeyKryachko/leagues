var GameNewScoresView = Backbone.Marionette.ItemView.extend({
    tagName: 'tr',
    template: "#game-new-member",
    ui: {
        'editable': '.editable',
        'score': '.score',
        'help': '.help'
    },
    events: {
        'change @ui.score': 'changeScore',
        'change @ui.help': 'changeHelp'
    },
    changeScore: function() {
        this.model.set('score', this.ui.score.val());
    },
    changeHelp: function () {
        this.model.set('help', this.ui.help.val());
    },
    onShow: function () {
        this.ui.editable.editable();
    },
    serializeData: function () {
        if (!this.model.get('score'))
            this.model.set('score', 0);
        if (!this.model.get('help'))
            this.model.set('help', 0);
        return this.model.toJSON();;
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
        var team = _.findWhere(this.teams.models, { id: this.ui.changeTeam.val() });
        this.model.set('id', team.get('id'));
        this.collection.reset(team.get('members'));
    },
    initialize: function (options) {
        this.teams = options.teams;
    },
    serializeData: function() {
        var model = this.model.toJSON();
        if (this.teams.length == 0)
            return model;

        model.teams = [];
        _.each(this.teams.models, function (team) {
            model.teams.push({ id: team.get('id'), name: team.get('name'), selected: team.get('id') == model.selected });
        });
        return model;
    },
    modelEvents: {
        'change': 'render'
    }
});