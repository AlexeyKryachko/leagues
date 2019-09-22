var SharedViews = require("../../shared/views.js");
var _ = require('underscore');
var $ = require('jquery');

var GameOptionsView = Backbone.Marionette.ItemView.extend({
    template: "#custom-game",
    ui: {
        'customScoring': '.custom-scoring',
        'eventChange': '.event-change'
    },
    events: {
        'change @ui.customScoring': 'customScoring',
        'change @ui.eventChange': 'eventChange'
    },
    initialize: function (options) {
        this.leagueId = options.leagueId;
    },
    eventChange: function () {
        var value = this.ui.eventChange.val();
        this.model.set('eventId', value);
    },
    customScoring: function () {
        var value = this.ui.customScoring.prop('checked');
        this.model.set('customScores', value);
        this.trigger('scoring:changed', value);
    },
    serializeData: function () {
        var self = this;
        var model = self.model.toJSON();

        if (!model.events)
            return model;

        _.each(model.events, function (obj) {
            if (obj.id === model.eventId)
                obj.selected = true;
        });

        return model;
    }
});

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
    },
    serializeData: function () {
        if (!this.model.get('score'))
            this.model.set('score', 0);
        if (!this.model.get('help'))
            this.model.set('help', 0);
        return this.model.toJSON();
    }
});

var GameView = Backbone.Marionette.CompositeView.extend({
    template: "#game-new",    
    childViewContainer: "tbody",
    childView: GameScoresView,
    emptyView: SharedViews.EmptyListView,
    childEvents: {
        'score:changed': 'recalculateScore'
    },
    ui: {
        'changeTeam': '.change-team',
        'score': '.team-score',
        'rent': '.add-custom-member',
        'addRent': '.add-custom-member-button',
        'bestMember': '.best-member'
    },
    events: {
        'change @ui.changeTeam': 'changeTeam',
        'change @ui.score': 'changeScore',
        'click @ui.addRent': 'addRent',
        'change @ui.bestMember': 'changeBestMember'
    },
    triggers: {
    },
    changeBestMember: function (e) {
        var val = this.ui.bestMember.val();
        this.model.set('bestId', val);
    },
    recalculateScore: function () {
        var score = 0;
        _.each(this.collection.models, function (obj) {
            score += parseInt(obj.get('score'));
        });
        this.model.set('score', score);
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
        this.leagueId = options.leagueId;
    },
    addRent: function () {
        var self = this;

        var val = self.ui.rent.val();
        if (!self.selectedRent || !val)
            return;

        self.collection.add(self.selectedRent);
        self.ui.rent.val('');
        self.selectedRent = null;
    },
    onRender: function () {
        var self = this;

        $(this.ui.rent).typeahead({
            source: function (query, process) {
                var url = '/api/league/' + self.leagueId + '/users?page=0&pageSize=10';
                if (self.model.get('id'))
                    url += '&exceptTeamIds=' + self.model.get('id');
                return $.get(url, { query: query }, function (response) {
                    return process(response);
                });
            },
            displayText: function (item) {
                return item.name;
            },
            updater: function (item) {
                self.selectedRent = item;
                return item.name;
            }
        });
    },
    serializeData: function() {
        var model = this.model.toJSON();

        if (this.teams.length == 0)
            return model;

        model.teamSelected = false;
        model.teams = [];
        _.each(this.teams.models, function (team) {
            var selected = team.get('id') == model.id;
            if (selected)
                model.teamSelected = true;

            model.teams.push({ id: team.get('id'), name: team.get('name'), selected: team.get('id') == model.id });
        });

        model.members = [];
        _.each(this.collection.models, function (member) {
            var memberJson = { id: member.get('id'), name: member.get('name') };

            var selected = member.get('id') == model.bestId;
            if (selected)
                memberJson.selected = true;

            model.members.push(memberJson);
        });

        return model;
    },
    modelEvents: {
        'change': 'render'
    }
});

module.exports = {
    GameView: GameView,
    GameOptionsView: GameOptionsView
}