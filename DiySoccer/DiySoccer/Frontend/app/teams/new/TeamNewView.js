var TeamNewMemberListItemView = Backbone.Marionette.ItemView.extend({
    tagName: 'tr',
    className: 'cursor-pointer',
    template: "#team-new-member",
    ui: {
        edit: '.editable'
    },
    onShow: function () {
        $(this.ui.edit).editable(this.edit.bind(this));
    },
    edit: function(value, settings) {
        this.model.set('name', value);
        return value;
    },
    serializeData: function () {
        return this.model.toJSON();
    }
});

var TeamNewView = Backbone.Marionette.CompositeView.extend({
    template: "#team-new",    
    childViewContainer: "tbody",
    childView: TeamNewMemberListItemView,
    emptyView: EmptyListView,
    ui: {
        'createNewMember': '.create-new-member'
    },
    events: {
        'click @ui.createNewMember': 'createNewMember'
    },
    createNewMember: function() {
        this.collection.add({});
    },
    initialize: function (options) {
    }
});