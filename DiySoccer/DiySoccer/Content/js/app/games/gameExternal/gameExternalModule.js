var MyApp = require("../../app.js");
var Models = require("../../models/games.js");
var Layouts = require("../../shared/layouts.js");
var Views = require("./gameExternalView.js");
var ListViews = require("./gameExternalListView.js");
var SharedViews = require("../../shared/views.js");

var gameExternalModule = Backbone.Marionette.Module.extend({
    startWithParent: false,

    initialize: function (options, app, object) {
        var self = this;

        self.app = app;

        self.gameExternalModel = new Models.gameExternalInfo();
        self.gameExternalList = new Models.gameExternalList();
        self.saveView = new SharedViews.SaveView();
    },
    onStart: function (options) {
        
        var self = this;

        self.options = options;
        self.gameExternalModel.clear();
        self.gameExternalList.clear();

        var isEditor = MyApp.Settings.isEditor(this.options.leagueId);

        self.createViews();
        self.bindViews(isEditor);

        if (options.leagueId) {
            if (isEditor) {
                self.gameExternalList.setLeagueId(self.options.leagueId);
                self.gameExternalList.fetch();
                self.gameExternalListView.setLeagueId(self.options.leagueId);
            } else {
                self.gameExternalModel.setLeagueId(self.options.leagueId);
                self.gameExternalModel.fetch();
            }
        }

        self.app.mainRegion.show(self.layout);
    },
    createViews: function () {
        var self = this;

        self.layout = new Layouts.LayoutView();
        self.gameExternalView = new Views.gameExternalView({ model: self.gameExternalModel });
        self.gameExternalListView = new ListViews.gameExternalListView({ model: self.gameExternalList });
    },
    bindViews: function (isEditor) {
        var self = this;

        if (isEditor) {
            self.listenTo(self.layout,
                'show',
                function() {
                    self.layout.center.show(self.gameExternalListView);
                });

        } else {
            self.listenTo(self.layout, 'show', function () {
                self.layout.center.show(self.gameExternalView);
                self.layout.down.show(self.saveView);
            });

            self.listenTo(self.saveView, 'save', function () {
                self.gameExternalModel.save(null, {
                    success: function (model, response) {
                        window.location.href = '#/leagues/' + self.options.leagueId;
                    },
                    error: function (model, error) {
                        window.location.href = '#/leagues/' + self.options.leagueId;
                    }
                });
            });

            self.listenTo(self.saveView, 'cancel', function () {
                window.location.href = '#/leagues/' + self.options.leagueId;
            });
        }
        
    },
    onStop: function () {

        var self = this;

        self.gameExternalView.destroy();
        self.gameExternalListView.destroy();
        self.saveView.destroy();
        self.layout.destroy();
    }
});

MyApp.module("gameExternal", gameExternalModule);