var _ = require('underscore');
var $ = require('jquery');

var gameExternalListView = Backbone.Marionette.ItemView.extend({
    template: "#game-external-list",
    ui: {
        'save': '.save-js',
        'delete': '.delete-js'
    },
    events: {
        'click @ui.save': 'save',
        'click @ui.delete': 'delete'
    },
    setLeagueId: function (leagueId) {
        this.leagueId = leagueId;

        console.log(' gameExternalListView:leagueId:' + leagueId);
    },
    save: function(event) {
        var id = $(event.currentTarget).data('id');
        var self = this;

        $.post('/api/leagues/' + this.leagueId + '/games/approve/' + id,
            function(e) {
                var approvals = self.model.get('approvals');

                approvals = _.without(approvals, _.findWhere(approvals, {
                    id: id
                }));

                self.model.set('approvals', approvals);
                self.model.trigger('view:reload');
            });
    },
    delete: function (event) {
        var id = $(event.currentTarget).data('id');
        var self = this;

        $.post('/api/leagues/' + this.leagueId + '/games/decline/' + id,
            function (e) {
                var approvals = self.model.get('approvals');

                approvals = _.without(approvals, _.findWhere(approvals, {
                    id: id
                }));

                self.model.set('approvals', approvals);
                self.model.trigger('view:reload');
            });
    },
    serializeData: function () {
        var model = this.model.toJSON();
        
        return model;
    },
    modelEvents: {
        'sync': 'render',
        'view:reload': 'render'
    }
});

module.exports = { gameExternalListView: gameExternalListView }