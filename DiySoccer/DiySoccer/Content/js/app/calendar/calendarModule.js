var MyApp = require("../app.js");
var Layouts = require("../shared/layouts.js");
var Views = require("./calendarView.js");
var Models = require("../models/calendars.js");
var sharedViews = require("../shared/views.js");

var calendarModule = Backbone.Marionette.Module.extend({
    startWithParent: false,

    initialize: function(options, app, object) {
        var self = this;

        self.app = app;

        self.model = new Models.Calendar();
        self.events = new Models.Events();
        self.teams = new Backbone.Collection();
    },
    onSubmit: function() {
        var self = this;

        self.model.save(null, {
            success: function(model, response) {
                window.history.back();
            }
        });
    },
    onStart: function (options) {
        console.log('[calendarModule] Module has been started.');
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

        self.layout = new Layouts.LayoutView();

        var viewOptions = {
            leagueId: self.options.leagueId,
            teams: self.teams,
            model: self.model,
            collection: self.events
        };
        self.calendarView = MyApp.Settings.isEditor(self.options.leagueId)
            ? new Views.EditorCalendarView(viewOptions)
            : new Views.UserCalendarView(viewOptions);
        self.breadcrumpsView = new sharedViews.breadcrumpsView({ model: self.model });
    },
    bindViews: function() {
        var self = this;

        self.listenTo(self.layout, 'show', function() {
            self.layout.center.show(self.calendarView);
            self.layout.breadcrumbs.show(self.breadcrumpsView);
        });

        self.listenTo(self.calendarView, 'submit', this.onSubmit);
        self.listenTo(self.calendarView, 'event:add', this.addEvent);

    },
    addEvent: function () {
        var self = this;
        var event = new Models.Event();
        event.setLeagueId(self.options.leagueId);

        event.save({}, {
            success: function () {
                self.events.add(event);
            }
        });
    },
    onStop: function (options) {
        console.log('[calendarModule] Module has been stopped.');
        var self = this;

        if (self.bottomView)
            self.bottomView.destroy();
        if (self.calendarView)
            self.calendarView.destroy();
        if (self.layout)
            self.layout.destroy();
    }
});

MyApp.module("calendar", calendarModule);