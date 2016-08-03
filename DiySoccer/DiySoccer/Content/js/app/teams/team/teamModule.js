var MyApp = require("../../app.js");
var Layouts = require("../../shared/layouts.js");
var Views = require("./teamView.js");
var Models = require("../../models/teams.js");


var teamModule = Backbone.Marionette.Module.extend({
    startWithParent: false,

    initialize: function (options, app, object) {
        var self = this;

        self.app = app;

        self.model = new Models.Team();
        self.members = new Backbone.Collection();
    },
    onSubmit: function () {
        var self = this;

        self.model.set('members', self.members.toJSON());

        self.model.save(null, {
            success: function(model, response) {
                window.history.back();
            }
        });
    },
    onCancel: function () {
        window.history.back();
    },
    onStart: function (options) {
        var self = this;

        self.model.clear();
        self.members.reset();

        self.options = options;
        self.model.setLeagueId(self.options.leagueId);

        if (options.teamId) {
            self.model.setId(self.options.teamId);
            self.listenToOnce(self.model, 'sync', self._onStart);
            self.model.fetch();
        } else {
            self._onStart();
        }
    },
    _onStart: function () {
        var self = this;

        self.members.reset(self.model.get('members'));

        self.createViews();
        self.bindViews();

        self.app.mainRegion.show(self.layout);
    },
    createViews: function () {
        var self = this;

        self.layout = new Layouts.LayoutView();
        self.teamView = new Views.TeamView({ model: this.model, collection: this.members, leagueId: this.options.leagueId });
    },
    bindViews: function () {
        var self = this;

        self.listenTo(self.layout, 'show', function () {
            self.layout.center.show(self.teamView);
        });

        self.listenTo(self.teamView, 'submit', this.onSubmit);
        self.listenTo(self.teamView, 'back', this.onCancel);
    },

    onStop: function (options) {
        var self = this;

        self.teamView.destroy();
        self.layout.destroy();
    }
});

MyApp.module("team", teamModule);