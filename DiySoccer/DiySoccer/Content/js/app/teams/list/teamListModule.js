var MyApp = require("../../app.js");
var Layouts = require("../../shared/layouts.js");
var SharedViews = require("../../shared/views.js");
var Views = require("./teamListView.js");
var Models = require("../../models/teams.js");

var teamsModule = Backbone.Marionette.Module.extend({
    startWithParent: false,

    initialize: function (options, app, object) {
        var self = this;
        this.app = app;

        self.statistics = new Models.TeamsStatistic();
        self.teams = new Backbone.Collection();

        self.listenTo(self.statistics, 'sync', function() {
            self.teams.reset(self.statistics.get('teamStats'));
        });
    },

    onStart: function (options) {
        var self = this;

        self.teams.reset();

        self.options = options;

        self.createViews();
        self.bindViews();

        self.app.mainRegion.show(self.layout);

        self.statistics.setLeagueId(self.options.leagueId);
        self.statistics.fetch();
    },
    createViews: function () {
        var self = this;
        
        self.layout = new Layouts.LayoutView();
        self.tableView = new Views.TeamListView({ model: self.statistics, collection: self.teams, leagueId: self.options.leagueId });
        self.actions = new Views.TeamListActions({ leagueId: self.options.leagueId });
        self.bottomView = new SharedViews.CancelView();
    },
    bindViews: function () {
        var self = this;

        self.listenTo(self.layout, 'show', function () {
            self.layout.up.show(self.actions);
            self.layout.center.show(self.tableView);
            self.layout.down.show(self.bottomView);
        });

        self.listenTo(self.bottomView, 'cancel', function () {
            window.history.back();
        });
    },
    onStop: function (options) {
        var self = this;

        self.bottomView.destroy();
        self.actions.destroy();
        self.tableView.destroy();
        self.layout.destroy();
    }
});

MyApp.module("teamList", teamsModule);