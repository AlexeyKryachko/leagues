var MyApp = require("../app.js");
var Views = require("../shared/views.js");
require('jquery-datetimepicker');

var CalendarListItemView = Backbone.Marionette.ItemView.extend({
    tagName: 'tr',
    template: "#calendar-event",
    ui: {
        'deleteEvent': '.delete-event',
        'addEventGame': '.add-event-game',
        'deleteEventGame': '.delete-event-game',
        'homeTeamChange': '.home-team-change',
        'guestTeamChange': '.guest-team-change',
        'nameTeam': '.name-event',
        'minorTeam': '.minor-event',
        'groupTeam': '.group-event',
        'startDate': '.date',
        'startDateInput': '.start-date'
    },
    events: {
        'click @ui.deleteEvent': 'deleteEvent',
        'click @ui.addEventGame': 'addEventGame',
        'click @ui.deleteEventGame': 'deleteEventGame',
        'change @ui.homeTeamChange': 'homeTeamChange',
        'change @ui.guestTeamChange': 'guestTeamChange',
        'change @ui.startDateInput': 'timeChange',
        'change @ui.nameTeam': 'nameChange',
        'change @ui.groupTeam': 'groupTeam',
        'change @ui.minorTeam': 'minorChange'
    },
    homeTeamChange: function (e) {
        var val = $(e.currentTarget).val();
        var eventGameId = $(e.currentTarget).data('id');

        this.model.changeHomeTeam(eventGameId, val);
    },
    guestTeamChange: function (e) {
        var val = $(e.currentTarget).val();
        var eventGameId = $(e.currentTarget).data('id');

        this.model.changeGuestTeam(eventGameId, val);
    },
    deleteEventGame: function (e) {
        var eventGameId = $(e.currentTarget).data('id');

        this.model.deleteGame(eventGameId);
    },
    addEventGame: function() {
        this.model.createGame();
    },
    deleteEvent: function () {
        this.model.destroy();
    },
    nameChange: function(e) {
        var val = this.ui.nameTeam.val();

        this.model.set('name', val);
        this.model.save();
    },
    groupChange: function (e) {
        var val = this.ui.groupTeam.val();

        this.model.set('group', val);
        this.model.save();
    },
    minorChange: function (e) {
        var val = this.ui.minorTeam.prop('checked');

        this.model.set('minor', val);
        this.model.save();
    },
    onRender: function () {
        var self = this;

        var startDate = new Date(self.model.get('startDate'));

        $(this.ui.startDate).datetimepicker({
            timepicker: false,
            startDate: startDate,
            onChangeDateTime: function (dp, $input) {
                var newDate = new Date($input.val());
                self.model.set('startDate', $input.val());
                $('input', self.ui.startDate).val(newDate.toLocaleDateString());
            }
        });

        $('input', self.ui.startDate).val(startDate.toLocaleDateString());
    },
    setOptions: function(options) {
        this.options.teams = options.teams;
        this.options.leagueId = options.leagueId;
        this.model.setLeagueId(options.leagueId);
    },
    serializeData: function () {
        var self = this;
        var model = this.model.toJSON();
        
        _.each(model.games, function (obj) {
            var guestTeam = _.findWhere(self.options.teams.models, { id: obj.guestTeamId });
            var homeTeam = _.findWhere(self.options.teams.models, { id: obj.homeTeamId });
            obj.guestTeamName = guestTeam ? guestTeam.get('name') : '';
            obj.homeTeamName = homeTeam ? homeTeam.get('name') : '';
            obj.teams = self.options.teams.toJSON();
        });
        
        model.isEditor = MyApp.Settings.isEditor(this.options.leagueId);

        if (model.isEditor)
            return model;
        
        if (model.startDate) {
            var date = new Date(model.startDate);
            model.startDate = date.toLocaleDateString();
        }

        return model;
    },
    modelEvents: {
        'change:games': 'render'
    }
});

var CalendarView = Backbone.Marionette.CompositeView.extend({
    template: "#calendar",    
    childViewContainer: "tbody",
    childView: CalendarListItemView,
    emptyView: Views.EmptyListView,
    ui: {
        'addEvent': '.add-event'
    },
    events: {
        'click @ui.addEvent': 'addEvent',
        
    },
    triggers: {
        'click @ui.submit': 'submit',
        'click @ui.back': 'back'
    },
    onBeforeAddChild: function (childView) {
        childView.setOptions(this.options);
    },
    addEvent: function() {
        this.trigger('event:add');
    },
    
    onShow: function () {
    },
    onRender: function () {
    },
    initialize: function (options) {
        this.options = options;
    },
    serializeData: function() {
        var model = this.model.toJSON();
        console.log('[CalendarView] ', model);
        model.showAddEvent = MyApp.Settings.isEditor(this.options.leagueId);

        return model;
    }
});

module.exports = { CalendarView: CalendarView }