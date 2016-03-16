var TeamNewMemberListItemView = Backbone.Marionette.ItemView.extend({
    tagName: 'tr',
    className: 'cursor-pointer',
    template: "#team-new-member",
    ui: {
        'removeMember': '.remove-new-member'
    },
    events: {
        'click @ui.removeMember': 'removeMember'
    },
    removeMember: function() {
        this.model.destroy();
    },
    onShow: function () {
        
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
        'createNewMember': '.create-new-member',
        'back': '.create-new-team-back',
        'memberName': '.new-member-name',
        'teamName': '.new-team-name',
        'submit': '.create-new-team'
    },
    events: {
        'click @ui.createNewMember': 'createNewMember',
        'click @ui.back': 'back',
        'change @ui.teamName': 'writeTeamName'
    },
    triggers: {
        'click @ui.submit': 'submit',
        'click @ui.back': 'back'
    },
    writeTeamName: function() {
        this.model.set('name', this.ui.teamName.val());
    },
    createNewMember: function () {
        var name = this.ui.memberName.val();
        if (!name || name == '')
            return;

        this.collection.add({ name: name });
        this.ui.memberName.val('');
    },
    initialize: function (options) {
    }
});