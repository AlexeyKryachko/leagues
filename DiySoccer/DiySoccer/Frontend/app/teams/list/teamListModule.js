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
        self.permissions = MyApp.Settings.get('permissions');

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
        self.actions = new TeamListActions(self.options);
        self.bottomView = new CancelView();
    },
    bindViews: function () {
        var self = this;

        self.listenTo(self.layout, 'show', function () {
            if (self.permissions.relationships[self.options.leagueId] &&
                self.permissions.relationships[self.options.leagueId] == '2') {
                self.layout.up.show(self.actions);
            }
            self.layout.center.show(self.tableView);
            self.layout.down.show(self.bottomView);
        });

        self.listenTo(self.bottomView, 'cancel', function () {
            document.location.href = '#leagues/';
        });
    },
    onStop: function (options) {
        var self = this;

        self.bottomView.destroy();
        self.actions.destroy();
        self.tableView.destroy();
        self.layout.destroy();
    }
});

MyApp.module("teamList", teamsModule);