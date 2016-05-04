var CalendarListItemView = Backbone.Marionette.ItemView.extend({
    tagName: 'tr',
    template: "#calendar-event",
    ui: {
        'deleteEvent': '.delete-event'
    },
    events: {
        'click @ui.deleteEvent': 'deleteEvent'
    },
    deleteEvent: function () {
        var self = this;

        $.ajax({
            type: "DELETE",
            url: '/api/leagues/' + self.options.leagueId + '/events/' + self.model.get('id'),
            success: function () {
                self.model.set('id', null);
                self.model.destroy();
            }
        });
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

        _.each(model.games, function() {
            var guestTeam = _.findWhere(self.teams.models, { id: this.guestTeamId });
            var homeTeam = _.findWhere(self.teams.models, { id: this.homeTeamId });
            this.guestTeamName = guestTeam.get('name');
            this.homeTeamName = homeTeam.get('name');
        });

        model.showAddEventGame = MyApp.Settings.isEditor(this.options.leagueId);
        model.showRemoveEvent = model.showAddEventGame;

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