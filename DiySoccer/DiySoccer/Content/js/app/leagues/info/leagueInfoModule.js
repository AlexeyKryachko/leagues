var MyApp = require("../../app.js");
var Models = require("../../models/leagues.js");
var Layouts = require("../../shared/layouts.js");
var Views = require("./leagueInfoView.js");

var leagueInfoModule = Backbone.Marionette.Module.extend({
    startWithParent: false,

    initialize: function (options, app, object) {
        var self = this;

        self.app = app;

        self.leagueInfo = new Models.LeagueInfo();
    },
    onStart: function (options) {
        console.log('[leagueInfoModule] Module has been changed.');
        var self = this;

        self.options = options;
        self.leagueInfo.clear();

        self.createViews();
        self.bindViews();

        if (options.leagueId) {
            self.leagueInfo.set('id', self.options.leagueId);
            self.leagueInfoView.setLeagueId(self.options.leagueId);
            self.leagueInfo.fetch();
        }

        self.app.mainRegion.show(self.layout);
    },
    createViews: function () {
        var self = this;

        self.layout = new Layouts.LayoutView();
        self.leagueInfoView = new Views.LeagueInfoView({ model: self.leagueInfo });
    },
    bindViews: function () {
        var self = this;
        
        self.listenTo(self.layout, 'show', function () {
            self.layout.center.show(self.leagueInfoView);
        });
    },
    onStop: function () {
        console.log('[leagueInfoModule] Module has been stopped.');
        var self = this;

        self.leagueInfoView.destroy();
        self.layout.destroy();
    }
});

MyApp.module("leagueInfo", leagueInfoModule);