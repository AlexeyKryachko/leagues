var MyApp = require("../../app.js");
var Layouts = require("../../shared/layouts.js");
var Views = require("./leaguesList.js");
var Models = require("../../models/leagues.js");

var leaguesModule = Backbone.Marionette.Module.extend({
    startWithParent: false,

    initialize: function (options, app, object) {
        this.app = app;

        this.leagues = new Models.LeaguesModel();
    },

    onStart: function (options) {
        var self = this;

        self.createViews();
        self.bindViews();

        self.app.mainRegion.show(self.layout);

        self.leagues.fetch();
    },
    createViews: function () {
        var self = this;

        self.layout = new Layouts.LayoutView();
        self.leagueListView = new Views.LeagueList({ model: self.leagues });
        self.leagueActionView = new Views.LeagueActions();
    },
    bindViews: function () {
        var self = this;

        self.listenTo(self.leagues, 'sync', function () {
            self.layout.up.show(self.leagueActionView);
            self.layout.center.show(self.leagueListView);
        });
    },
    onStop: function (options) {
        var self = this;

        self.leagueListView.destroy();
        self.leagueActionView.destroy();
        self.layout.destroy();
    }
});

MyApp.module("leagues", leaguesModule);