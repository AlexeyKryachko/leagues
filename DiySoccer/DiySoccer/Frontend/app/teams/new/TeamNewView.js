var TeamNewMemberListItemView = Backbone.Marionette.ItemView.extend({
    tagName: 'tr',
    className: 'cursor-pointer',
    template: "#team-new-member",
    ui: {
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
        'memberName': '.new-member-name',
        'teamName': '.new-team-name',
        'submit': '.create-new-team'
    },
    events: {
        'click @ui.createNewMember': 'createNewMember',
        'change @ui.teamName': 'writeTeamName'
    },
    triggers: {
        'click @ui.submit': 'submit'
    },
    writeTeamName: function() {
        this.model.set('name', this.ui.teamName.val());
    },
    createNewMember: function () {
        var name = this.ui.memberName.val();
        if (!name || name == '')
            return;

        this.collection.add({ value: name });
    },
    initialize: function (options) {
    }
});