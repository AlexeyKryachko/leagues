var LeagueActions = Backbone.Marionette.ItemView.extend({
    template: "#leagues-actions",
    ui: {
        'addLeague': '.add-league'
    },
    events: {
        'click @ui.addLeague': 'addLeague'
    },
    addLeague: function () {
        document.location.href = '#leagues/new';
    }
});
