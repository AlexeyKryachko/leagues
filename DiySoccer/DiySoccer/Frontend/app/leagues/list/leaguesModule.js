﻿var leaguesModule = Marionette.Module.extend({
    startWithParent: false,

    initialize: function (options, app, object) {
        this.app = app;

        this.leagues = new Leagues();
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

        self.layout = new LayoutView();
        self.leagueListView = new LeagueList({ collection: self.leagues });
        self.leagueActionView = new LeagueActions();
    },
    bindViews: function () {
        var self = this;

        self.listenTo(self.layout, 'show', function () {
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