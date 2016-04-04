var teamModule = Marionette.Module.extend({
    startWithParent: false,

    initialize: function (options, app, object) {
        var self = this;

        self.app = app;

        self.model = new Backbone.Model();
        self.members = new Backbone.Collection();
    },
    onSubmit: function () {
        var self = this;
        
        if (self.options.teamId) {
            self._updateTeam();
        } else {
            self._createTeam();
        }
    },
    onCancel: function () {
        document.location.href = '#leagues/' + this.options.leagueId;
    },
    _updateTeam: function () {
        var self = this;

        var data = {
            name: self.model.get('name'),
            hidden: self.model.get('hidden'),
            members: self.members.toJSON()
        }

        $.ajax({
            type: "PUT",
            url: '/api/leagues/' + self.options.leagueId + '/teams/' + self.model.get('id'),
            data: data,
            success: function () {
                document.location.href = '#leagues/' + self.options.leagueId;
            }
        });
    },
    _createTeam: function () {
        var self = this;

        var data = {
            league: self.options.leagueId,
            hidden: self.model.get('hidden'),
            name: self.model.get('name'),
            members: self.members.toJSON()
        }

        $.ajax({
            type: "POST",
            url: '/api/leagues/' + self.options.leagueId + '/teams/',
            data: data,
            success: function () {
                document.location.href = '#leagues/' + self.options.leagueId;
            }
        });
    },
    onStart: function (options) {
        var self = this;

        self.options = options;
        if (options.teamId) {
            self._onEditStart(options);
        } else {
            self._onCreateStart(options);
        }
    },
    _onEditStart: function (options) {
        var self = this;

        self.options = options;

        $.get('/api/league/' + options.leagueId + '/teams/' + options.teamId, function (response) {
            self.model.clear();
            self.model.set('name', response.name);
            self.model.set('id', response.id);
            self.members.reset(response.members);

            self.createViews();
            self.bindViews();

            self.app.mainRegion.show(self.layout);
        });
    },
    _onCreateStart: function (options) {
        var self = this;

        self.options = options;

        self.model.clear();
        self.members.reset();

        self.createViews();
        self.bindViews();

        self.app.mainRegion.show(self.layout);
    },
    createViews: function () {
        var self = this;

        self.layout = new LayoutView();
        self.teamView = new TeamView({ model: this.model, collection: this.members });
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