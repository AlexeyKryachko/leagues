var calendarModule = Marionette.Module.extend({
    startWithParent: false,

    initialize: function (options, app, object) {
        var self = this;

        self.app = app;

        self.model = new Calendar();
        self.events = new Backbone.Collection();
    },
    onSubmit: function () {
        var self = this;

        self.model.save(null, {
            success: function(model, response) {
                document.location.href = '#leagues/' + self.options.leagueId;
            }
        });
    },
    onCancel: function () {
        document.location.href = '#leagues/' + this.options.leagueId;
    },
    onStart: function (options) {
        var self = this;

        self.model.clear();
        self.events.reset();

        self.options = options;
        self.model.setLeagueId(self.options.leagueId);

        self.listenToOnce(self.model, 'sync', self._onStart);
        self.model.fetch();
    },
    _onStart: function () {
        var self = this;

        self.events.reset(self.model.get('events'));

        self.createViews();
        self.bindViews();

        self.app.mainRegion.show(self.layout);
    },
    createViews: function () {
        var self = this;

        self.layout = new LayoutView();
        self.calendarView = new CalendarView({ model: self.model, collection: self.events, leagueId: self.options.leagueId });
    },
    bindViews: function () {
        var self = this;

        self.listenTo(self.layout, 'show', function () {
            self.layout.center.show(self.calendarView);
        });

        self.listenTo(self.teamView, 'submit', this.onSubmit);
        self.listenTo(self.teamView, 'back', this.onCancel);
    },

    onStop: function (options) {
        var self = this;

        self.calendarView.destroy();
        self.layout.destroy();
    }
});

MyApp.module("calendar", calendarModule);