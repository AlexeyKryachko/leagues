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
            var array = self.leagues.get('leagues');

            for (var index = 0; index < array.length; index++) {
                var league = array[index];
                self.imageLoading(self, array, index);
            }
        });
    },
    imageLoading: function(self, array, index) {
        var league = array[index];

        var tester = new Image();
        tester.onload = function() {
            console.log('Image loaded: ' + league.mediaId);
            league.loaded = true;
            self.showAfterLoad(self, array);
        };
        tester.onerror = function() {
            console.log('Image error: ' + league.mediaId);
            league.mediaId = null;
            league.loaded = true;
            self.showAfterLoad(self, array);
        };

        tester.src = '/api/image/' + league.mediaId + '?width=70&height=70';
    },
    showAfterLoad: function (self, array) {
        for (var index = 0; index < array.length; index++) {
            var league = array[index];
            if (!league.loaded)
                return;
        }

        self.layout.up.show(self.leagueActionView);
        self.layout.center.show(self.leagueListView);
    },
    onStop: function (options) {
        var self = this;

        self.leagueListView.destroy();
        self.leagueActionView.destroy();
        self.layout.destroy();
    }
});

MyApp.module("leagues", leaguesModule);