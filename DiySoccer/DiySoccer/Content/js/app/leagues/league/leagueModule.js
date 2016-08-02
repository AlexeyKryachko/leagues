var MyApp = require("../../app.js");
var Layouts = require("../../shared/layouts.js");
var SharedViews = require("../../shared/views.js");
var Views = require("./leagueView.js");
var Models = require("../../models/leagues.js");

var leagueModule = Backbone.Marionette.Module.extend({
    startWithParent: false,

    initialize: function (options, app, object) {
        var self = this;

        self.app = app;

        self.league = new Models.League();
        self.admins = new Backbone.Collection();
    },
    onStart: function (options) {
        var self = this;

        self.options = options;
        self.league.clear();
        self.admins.reset();

        self.createViews();
        self.bindViews();

        if (options.leagueId) {
            self.league.set('id', self.options.leagueId);
            self.league.fetch();
        }

        self.app.mainRegion.show(self.layout);
    },
    createViews: function () {
        var self = this;

        self.layout = new Layouts.LayoutView();
        self.leagueView = new Views.LeagueView({ model: self.league, collection: self.admins });
        self.saveView = new SharedViews.SaveView();
    },
    bindViews: function () {
        var self = this;
        
        self.listenTo(self.layout, 'show', function () {
            self.layout.center.show(self.leagueView);
            self.layout.down.show(self.saveView);
        });

        self.listenToOnce(self.league, 'sync', function () {
            self.admins.reset(self.league.get('admins'));
        });

        self.listenTo(self.saveView, 'save', function () {
            self.listenToOnce(self.league, 'sync', function() {
                document.location.href = '#leagues/';
            });

            self.league.set('admins', self.admins.toJSON());
            self.league.save();
        });

        self.listenTo(self.saveView, 'cancel', function () {
            window.history.back();
        });
    },
    onStop: function (options) {
        var self = this;

        self.leagueView.destroy();
        self.saveView.destroy();
        self.layout.destroy();
    }
});

MyApp.module("league", leagueModule);