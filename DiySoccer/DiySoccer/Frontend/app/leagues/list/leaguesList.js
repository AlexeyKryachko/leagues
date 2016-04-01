var leagueView = Backbone.Marionette.ItemView.extend({
    template: "#leagues-item",
    ui: {
        'edit': '.league-edit'
    },
    events: {
        'click @ui.edit': 'editLeague'
    },
    editLeague: function (e) {
        e.preventDefault();
        e.stopPropagation();

        document.location.href = '#leagues/' + this.model.id + '/edit';
    },
    serializeData: function() {
        var model = this.model.toJSON();
        model.showEdit = MyApp.Settings.isAdmin();
        return model;
    }
});

var LeagueList = Backbone.Marionette.CompositeView.extend({
    template: "#leagues-list",
    childViewContainer: ".leagues-container",
    childView: leagueView,
    initialize: function (options) {        
    }
});