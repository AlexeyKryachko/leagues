var MyApp = require("../../app.js");
var Models = require("../../models/games.js");
var Layouts = require("../../shared/layouts.js");
var Views = require("./gameExternalView.js");
var SharedViews = require("../../shared/views.js");

var gameExternalModule = Backbone.Marionette.Module.extend({
    startWithParent: false,

    initialize: function (options, app, object) {
        var self = this;

        self.app = app;

        self.gameExternalModel = new Models.gameExternalInfo();
        self.saveView = new SharedViews.SaveView();
    },
    onStart: function (options) {
        
        var self = this;

        self.options = options;
        self.gameExternalModel.clear();

        self.createViews();
        self.bindViews();

        if (options.leagueId) {
            self.gameExternalModel.setLeagueId(self.options.leagueId);
            self.gameExternalModel.fetch();
        }

        self.app.mainRegion.show(self.layout);
    },
    createViews: function () {
        var self = this;

        self.layout = new Layouts.LayoutView();
        self.gameExternalView = new Views.gameExternalView({ model: self.gameExternalModel });
    },
    bindViews: function () {
        var self = this;

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
    },
    onStop: function () {

        var self = this;

        self.gameExternalView.destroy();
        self.saveView.destroy();
        self.layout.destroy();
    }
});

MyApp.module("gameExternal", gameExternalModule);