var MyApp = require("../../app.js");
var Layouts = require("../../shared/layouts.js");
var Views = require("./statisticViews.js");
var Models = require("../../models/leagues.js");

var leagueStatisticsModule = Backbone.Marionette.Module.extend({
    startWithParent: false,

    initialize: function (options, app, object) {
        this.app = app;

        this.statistics = new Models.LeagueStatisticsModel();
    },

    onStart: function (options) {
        var self = this;

        self.createViews();
        self.bindViews();

        self.app.mainRegion.show(self.layout);

        if (options.leagueId) {
            self.statistics.set('id', options.leagueId);
        }

        self.statistics.fetch();
    },
    createViews: function () {
        var self = this;

        self.layout = new Layouts.LayoutView();
        self.StatisticsView = new Views.StatisticsView({ model: self.statistics });
    },
    bindViews: function () {
        var self = this;

        self.listenTo(self.statistics, 'sync', function () {
            self.layout.center.show(self.StatisticsView);
        });
    },
    onStop: function (options) {
        var self = this;

        self.StatisticsView.destroy();
        self.layout.destroy();
    }
});

MyApp.module("statistics", leagueStatisticsModule);