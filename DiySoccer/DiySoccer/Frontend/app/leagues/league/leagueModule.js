var leagueModule = Marionette.Module.extend({
    startWithParent: false,

    initialize: function (options, app, object) {
        var self = this;

        self.app = app;

        self.league = new League();
    },
    onStart: function (options) {
        var self = this;

        self.options = options;

        if (options.leagueId) {
            self._onEdit();
        } else {
            self._onNew();
        }

    },
    _onNew: function () {
        var self = this;

        self.league.clear();
        
        self.createViews();
        self.bindViews();
        
        self.app.mainRegion.show(self.layout);
    },
    _onEdit: function () {
        var self = this;

        self.league.clear();

        self.createViews();
        self.bindViews();

        self.league.set('id', self.options.leagueId);
        self.league.fetch();

        self.app.mainRegion.show(self.layout);
    },
    createViews: function () {
        var self = this;

        self.layout = new LayoutView();
        self.leagueView = new LeagueView({ model: self.league });
        self.saveView = new SaveView();
    },
    bindViews: function () {
        var self = this;
        
        self.listenTo(self.layout, 'show', function () {
            self.layout.center.show(self.leagueView);
            self.layout.down.show(self.saveView);
        });

        self.listenTo(self.saveView, 'save', function () {
            self.listenToOnce(self.league, 'sync', function() {
                document.location.href = '#leagues/';
            });
            self.league.save();
        });

        self.listenTo(self.saveView, 'cancel', function () {
            document.location.href = '#leagues/';
        });
    },
    onStop: function (options) {
        var self = this;

        self.leagueView.destroy();
        self.saveView.destroy();
        self.layout.destroy();
    }
});

MyApp.module("league", leagueModule);