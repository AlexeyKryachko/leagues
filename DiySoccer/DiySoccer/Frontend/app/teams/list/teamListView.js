var TeamListItemView = Backbone.Marionette.ItemView.extend({
    tagName: 'tr',
    className: 'cursor-pointer',
    template: "#team-item",
    events:{
        'click': 'onRedirect'
    },
    onRedirect: function () {
        document.location.href = document.location.href + '/teams/' + this.model.get('id');
    },
    onShow: function () {
    },
    serializeData: function () {
        return this.model.toJSON();
    }
});

var TeamListView = Backbone.Marionette.CompositeView.extend({
    template: "#team-list",    
    childViewContainer: "tbody",
    childView: TeamListItemView,
    ui: {
        'addBtn': '.add-new-team',
        'addGame': '.add-new-game'
    },
    events: {
        'click @ui.addBtn': 'redirectAddTeam',
        'click @ui.addGame': 'redirectAddGame'
    },
    initialize: function (options) {
        this.options = {
            leagueId: options.leagueId
        }
    },
    redirectAddTeam: function (e) {
        document.location.href = "#leagues/" + this.options.leagueId + "/teams/new";
    },
    redirectAddGame: function (e) {
        document.location.href = "#leagues/" + this.options.leagueId + "/games/new";
    }
});