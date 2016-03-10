var teamsModule = Marionette.Module.extend({
    startWithParent: false,

    initialize: function (options, app, object) {
        var self = this;
        this.app = app;

        self.teams = new TeamsStatistic();
    },

    onStart: function (options) {
        var self = this;

        self.options = options;

        self.createViews();
        self.bindViews();

        self.app.mainRegion.show(self.layout);

        self.teams.setLeagueId(self.options.leagueId);
        self.teams.fetch();
    },
    createViews: function () {
        var self = this;
        
        self.layout = new LayoutView();
        self.tableView = new TeamListView({ collection: self.teams, leagueId: self.options.leagueId });
        self.actions = new TeamListActions({ leagueId: self.options.leagueId });
    },
    bindViews: function () {
        var self = this;

        self.listenTo(self.layout, 'show', function () {
            self.layout.up.show(self.actions);
            self.layout.center.show(self.tableView);
        });
    },
    onStop: function (options) {
    }
});

MyApp.module("teamList", teamsModule);