var MyApp = require("../../app.js");
var Layouts = require("../../shared/layouts.js");
var Views = require("./tournamentsInfoViews.js");
var Models = require("../../models/tournaments.js");

var tournamentsInfoModule = Backbone.Marionette.Module.extend({
    startWithParent: false,

    initialize: function (options, app, object) {
        var self = this;

        self.app = app;

        self.tournamentsInfo = new Models.TournamentsInfo();
    },
    onStart: function (options) {
        var self = this;

        self.options = options;
        self.tournamentsInfo.clear();

        if (options.tournamentId) {
            self.tournamentsInfo.set('id', self.options.tournamentId);
        }

        self.createViews();
        self.bindViews();

        if (options.tournamentId) {
            self.tournamentsInfoView.setTournamentId(options.tournamentId);
            self.tournamentsInfo.fetch();
        }

        self.app.mainRegion.show(self.layout);
    },
    createViews: function () {
        var self = this;

        var isEditor = MyApp.Settings.isEditor(this.options.tournamentId);

        self.layout = new Layouts.LayoutView();
        self.tournamentsInfoView = new Views.TournamentsInfoView({ model: self.tournamentsInfo });

        if (isEditor)
            self.tournamentsInfoActions = new Views.TournamentsInfoActions({ model: self.tournamentsInfo });
    },
    bindViews: function () {
        var self = this;

        if (self.tournamentsInfoActions) {
            self.listenTo(self.layout, 'show', function () {
                self.layout.up.show(self.tournamentsInfoActions);
            });
        }

        self.listenTo(self.tournamentsInfo, 'sync', function () {
            self.layout.center.show(self.tournamentsInfoView);
        });
    },
    onStop: function () {
        var self = this;

        self.tournamentsInfoView.destroy();
        if (self.tournamentsInfoActions)
            self.tournamentsInfoActions.destroy();
        self.layout.destroy();
    }
});

MyApp.module("tournamentsInfo", tournamentsInfoModule);