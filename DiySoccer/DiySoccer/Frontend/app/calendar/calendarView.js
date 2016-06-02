var CalendarListItemView = Backbone.Marionette.ItemView.extend({
    tagName: 'tr',
    template: "#calendar-event",
    ui: {
        'deleteEvent': '.delete-event',
        'addEventGame': '.add-event-game',
        'deleteEventGame': '.delete-event-game',
        'homeTeamChange': '.home-team-change',
        'guestTeamChange': '.guest-team-change'
    },
    events: {
        'click @ui.deleteEvent': 'deleteEvent',
        'click @ui.addEventGame': 'addEventGame',
        'click @ui.deleteEventGame': 'deleteEventGame',
        'change @ui.homeTeamChange': 'homeTeamChange',
        'change @ui.guestTeamChange': 'guestTeamChange'
    },
    homeTeamChange: function (e) {
        var val = $(e.currentTarget).val();
        var eventGameId = $(e.currentTarget).data('id');

        _.each(model.games, function () {
            if (this.id == eventGameId) {
                this.homeTeamId = val;
            }
        });
    },
    guestTeamChange: function (e) {
        var val = $(e.currentTarget).val();
        var eventGameId = $(e.currentTarget).data('id');

        _.each(model.games, function () {
            if (this.id == eventGameId) {
                this.guestTeamId = val;
            }
        });
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
    onShow: function () {
        
    },
    setOptions: function(options) {
        this.options.teams = options.teams;
        this.options.leagueId = options.leagueId;
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
        
        console.log(model);

        return model;
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