﻿headerModule = Marionette.Module.extend({
    startWithParent: false,

    initialize: function (options, app, object) {
        this.app = app;
    },

    onStart: function (options) {
        var self = this;

        self.options = options;

        self.createViews();
        self.bindViews();

        self.app.headerRegion.show(self.headerView);
    },
    createViews: function () {
        var self = this;

        self.headerView = new HeaderView({ model: MyApp.Settings });
    },
    bindViews: function () {
        var self = this;

        self.listenTo(self.headerView, 'vkLogin', function() {
            MyApp.Settings.loginVK();
        });

        self.listenTo(self.headerView, 'logout', function () {
            MyApp.Settings.logout();
        });

    },
    onStop: function (options) {
        this.headerView.destroy();
    }
});

MyApp.module("header", headerModule);