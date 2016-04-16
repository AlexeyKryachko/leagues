var LeagueAdminListItemView = Backbone.Marionette.ItemView.extend({
    tagName: 'tr',
    template: "#league-admin",
    ui: {
        'removeAdmin': '.remove-league-admin'
    },
    events: {
        'click @ui.removeAdmin': 'removeAdmin'
    },
    removeAdmin: function () {
        this.model.set('id', null);
        this.model.destroy();
    },
    onShow: function () {

    },
    serializeData: function () {
        return this.model.toJSON();
    }
});

var LeagueView = Backbone.Marionette.CompositeView.extend({
    template: "#league",
    childViewContainer: "tbody",
    childView: LeagueAdminListItemView,
    emptyView: EmptyListView,
    ui: {
        'addAdmin': '.add-league-admin',
        'name': '.league-name',
        'description': '.league-description',
        'group': '.league-vkGroup'
    },
    events: {
        'change @ui.name': 'changeName',
        'change @ui.description': 'changeDescription',
        'change @ui.group': 'changeGroup'
    },
    changeName: function () {
        this.model.set('name', this.ui.name.val());
    },
    changeDescription: function () {
        this.model.set('description', this.ui.description.val());
    },
    changeGroup: function () {
        this.model.set('vkGroup', this.ui.group.val());
    },
    onRender: function () {
        var self = this;

        $(this.ui.addAdmin).typeahead({
            source: function (query, process) {
                var url = '/api/users?page=0&pageSize=10';
                return $.get(url, { query: query }, function (response) {
                    return process(response);
                });
            },
            displayText: function (item) {
                return item.name;
            },
            updater: function (item) {
                self.collection.add(item);
                return '';
            }
        });
    },
    modelEvents: {
        'sync': 'render'
    }
});