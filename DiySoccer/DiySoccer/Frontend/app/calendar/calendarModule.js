var calendarModule = Marionette.Module.extend({
    startWithParent: false,

    initialize: function(options, app, object) {
        var self = this;

        self.app = app;

        self.model = new Calendar();
        self.events = new Events();
        self.teams = new Backbone.Collection();
    },
    onSubmit: function() {
        var self = this;

        self.model.save(null, {
            success: function(model, response) {
                document.location.href = '#leagues/' + self.options.leagueId;
            }
        });
    },
    onCancel: function() {
        document.location.href = '#leagues/' + this.options.leagueId;
    },
    onStart: function(options) {
        var self = this;

        self.model.clear();
        self.events.reset();
        self.teams.reset();

        self.options = options;
        self.model.setLeagueId(self.options.leagueId);

        self.listenToOnce(self.model, 'sync', self._onStart);
        self.model.fetch();
    },
    _onStart: function() {
        var self = this;

        self.events.reset(self.model.get('events'));
        self.teams.reset(self.model.get('teams'));

        self.createViews();
        self.bindViews();

        self.app.mainRegion.show(self.layout);
    },
    createViews: function() {
        var self = this;

        self.layout = new LayoutView();

        var viewOptions = {
            leagueId: self.options.leagueId,
            teams: self.teams,
            model: self.model,
            collection: self.events
        };
        self.calendarView = new CalendarView(viewOptions);
    },
    bindViews: function() {
        var self = this;

        self.listenTo(self.layout, 'show', function() {
            self.layout.center.show(self.calendarView);
        });

        self.listenTo(self.calendarView, 'submit', this.onSubmit);
        self.listenTo(self.calendarView, 'back', this.onCancel);
        self.listenTo(self.calendarView, 'event:add', this.addEvent);

    },
    addEvent: function () {
        var self = this;
        var event = new Event();
        event.setLeagueId(self.options.leagueId);

        event.save({}, {
            success: function () {
                self.events.add(event);
            }
        });
    },
    onStop: function (options) {
        var self = this;

        self.calendarView.destroy();
        self.layout.destroy();
    }
});

MyApp.module("calendar", calendarModule);