﻿var CalendarListItemView = Backbone.Marionette.ItemView.extend({
    tagName: 'tr',
    template: "#calendar-event",
    ui: {
        'deleteEvent': '.delete-event',
        'addEventGame': '.add-event-game',
        'deleteEventGame': '.delete-event-game',
        'homeTeamChange': '.home-team-change',
        'guestTeamChange': '.guest-team-change',
        'nameTeam': '.name-event',
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
        'change @ui.nameTeam': 'nameChange'
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
    onRender: function () {
        var self = this;

        this.ui.startDate.datetimepicker({
                defaultDate: self.model.get('startDate')
            })
            .on('dp.change', function (e) {
                var date = new Date(e.date);
                console.log(date);
                self.model.set('startDate', date);
            });
    },
    setOptions: function(options) {
        this.options.teams = options.teams;
        this.options.leagueId = options.leagueId;
        this.model.setLeagueId(options.leagueId);
    },
    serializeData: function () {
        var self = this;
        var model = this.model.toJSON();
        
        model.isEditor = MyApp.Settings.isEditor(this.options.leagueId);

        if (!model.isEditor)
            return model;

        _.each(model.games, function (obj) {
            var guestTeam = _.findWhere(self.options.teams.models, { id: obj.guestTeamId });
            var homeTeam = _.findWhere(self.options.teams.models, { id: obj.homeTeamId });
            obj.guestTeamName = guestTeam ? guestTeam.get('name') : '';
            obj.homeTeamName = homeTeam ? homeTeam.get('name') : '';
            obj.teams = self.options.teams.toJSON();
        });
        
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
    emptyView: EmptyListView,
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