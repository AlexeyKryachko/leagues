require("bootstrap-3-typeahead");

var TournamentsInfoActions = Backbone.Marionette.ItemView.extend({
    template: "#tournament-info-actions",
    ui: {
        'findTeam': '.import-team-input',
        'importTeam': '.import-team-button'
    },
    events: {
        'click @ui.importTeam': 'importTeam'
    },
    importTeam: function () {
        var val = $(this.ui.importTeam).data('teamId');

        if (!val || val == '')
            return;

        var self = this;
        $.ajax({
            type: "POST",
            url: '/api/unions/' + this.model.get('id') + '/teams/' + val + '/copy/',
            success: function (data, textStatus, xhr) {
                self.ui.findTeam.val('');
            }
        });
    },
    onRender: function () {
        var self = this;

        $(this.ui.findTeam).typeahead({
            source: function (query, process) {
                var url = '/api/teams/search?page=0&pageSize=10';
                return $.get(url, { query: query }, function (response) {
                    return process(response);
                });
            },
            displayText: function (item) {
                return item.name;
            },
            updater: function (item) {
                $(self.ui.importTeam).data('teamId', item.id);
                return item.name;
            }
        });
    },
    modelEvents: {
        'sync': 'render'
    }
});

var TournamentsInfoView = Backbone.Marionette.ItemView.extend({
    template: "#tournament-info",
    serializeData: function () {
        var model = this.model.toJSON();

        if (!model.events)
            return model;

        _.each(model.events, function (obj) {
            var col = obj.length;
            _.each(obj, function(group) {
                group.col = 12/col;
            });
        });

        return model;
    }
});

module.exports = {
    TournamentsInfoActions: TournamentsInfoActions,
    TournamentsInfoView: TournamentsInfoView
}