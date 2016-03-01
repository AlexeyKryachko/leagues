var teamNewModule = Marionette.Module.extend({
    startWithParent: false,

    initialize: function (options, app, object) {
        this.app = app;
    },

    onStart: function (options) {
        var self = this;

        console.log('[teamNewModule] started');
        if (!self.layout)
            self.layout = new LayoutView();

        var model = new Backbone.Model();
        var members = new Backbone.Collection();

        var newTeamView = new TeamNewView({ model: model, collection: members });

        self.listenTo(self.layout, 'show', function () {
            self.layout.bigRegion.show(newTeamView);
        });

        self.app.mainRegion.show(self.layout);
    },

    onStop: function (options) {
        console.log('[teamNewModule] stopped');
    }
});

MyApp.module("teamNew", teamNewModule);