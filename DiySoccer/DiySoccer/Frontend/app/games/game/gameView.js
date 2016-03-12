var GameScoresView = Backbone.Marionette.ItemView.extend({
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
        this.trigger('score:changed');
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

var GameView = Backbone.Marionette.CompositeView.extend({
    template: "#game-new",    
    childViewContainer: "tbody",
    childView: GameScoresView,
    emptyView: EmptyListView,
    childEvents: {
        'score:changed': 'recalculateScore'
    },
    ui: {
        'changeTeam': '.change-team',
        'score': '.team-score'
    },
    events: {
        'change @ui.changeTeam': 'changeTeam',
        'change @ui.score': 'changeScore'
    },
    triggers: {
    },
    recalculateScore: function () {
        var score = 0;
        _.each(this.collection.models, function (obj) {
            score += parseInt(obj.get('score'));
        });
        this.model.set('score', score);
    },
    disableScore: function () {
        this.ui.score.prop('disabled', 'disabled');
        this.model.set('disableScoreValue', false);
    },
    enableScore: function () {
        this.ui.score.removeAttr('disabled');
        this.model.set('disableScoreValue', true);
    },
    changeTeam: function () {
        var team = _.findWhere(this.teams.models, { id: this.ui.changeTeam.val() });
        this.model.set('id', team.get('id'));
        this.collection.reset(team.get('members'));
    },
    changeScore: function () {
        this.model.set('score', this.ui.score.val());
    },
    initialize: function (options) {
        this.teams = options.teams;
    },
    onShow: function() {
        
    },
    serializeData: function() {
        var model = this.model.toJSON();

        if (this.teams.length == 0)
            return model;

        model.teams = [];
        _.each(this.teams.models, function (team) {
            model.teams.push({ id: team.get('id'), name: team.get('name'), selected: team.get('id') == model.id });
        });

        return model;
    },
    modelEvents: {
        'change': 'render'
    }
});