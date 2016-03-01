var teamNewModule = Marionette.Module.extend({
    startWithParent: false,

    initialize: function (options, app, object) {
        var self = this;

        self.app = app;

        self.model = new Backbone.Model();
        self.members = new Backbone.Collection();

        self.layout = new LayoutView();
        self.newTeamView = new TeamNewView({ model: this.model, collection: this.members });

        self.listenTo(self.layout, 'show', function () {
            self.layout.bigRegion.show(self.newTeamView);
        });

        self.listenTo(self.newTeamView, 'submit', this.createTeam);
    },
    createTeam: function () {
        var data = {
            league: this.options.leagueId,
            name: this.model.get('name'),
            members: this.members.toJSON()
        }

        console.log(data);
        $.ajax({
            type: "POST",
            url: "/api/teams",
            data: data,
            success: function() {
                document.location.href = '#leagues/' + this.options.leagueId;
            }
        });
    },
    onStart: function (options) {
        var self = this;

        self.options = options;
        self.members.reset();

        self.app.mainRegion.show(self.layout);
    },

    onStop: function (options) {
        console.log('[teamNewModule] stopped');
    }
});

MyApp.module("teamNew", teamNewModule);