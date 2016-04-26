var TeamGameView = Backbone.Marionette.ItemView.extend({
    tagName: 'tr',
    template: "#team-game",
    ui: {
        'edit': '.edit-game',
        'delete': '.delete-game'
    },
    events: {
        'click @ui.edit': 'editRedirect',
        'click @ui.delete': 'deleteGame'
    },
    editRedirect: function() {
        this.trigger('game:edit', this.model);
    },
    deleteGame: function () {
        this.trigger('game:delete', this.model);
    },
    setLeagueId: function(leagueId) {
        this.options.leagueId = leagueId;
    },
    onShow: function () {
    },
    serializeData: function () {
        var model = this.model.toJSON();

        model.showEdit = MyApp.Settings.isEditor(this.options.leagueId);
        model.showDelete = MyApp.Settings.isEditor(this.options.leagueId);

        return model;
    }
});

var TeamGamesView = Backbone.Marionette.CompositeView.extend({
    template: "#team-info",    
    childViewContainer: "tbody",
    childView: TeamGameView,
    emptyView: EmptyListView,
    childEvents: {
        'game:delete': 'deleteGame',
        'game:edit': 'editGame'
    },
    onBeforeAddChild: function (childView) {
        childView.setLeagueId(this.options.leagueId);
    },
    editGame: function (view, model) {
        document.location.href = '#leagues/' + this.options.leagueId + '/games/' + model.get('id') + '/edit';
    },
    deleteGame: function (view, model) {
        var self = this;

        $.ajax({
            method: "DELETE",
            url: '/api/leagues/' + self.options.leagueId + '/games/' + model.get('id')
        }).done(function (response) {
            self.collection.remove(model);
        });
    },
    ui: {
    },
    initialize: function (options) {
        this.options = options;
    }
});
