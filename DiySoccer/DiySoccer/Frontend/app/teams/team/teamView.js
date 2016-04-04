var TeamMemberListItemView = Backbone.Marionette.ItemView.extend({
    tagName: 'tr',
    template: "#team-member",
    ui: {
        'removeMember': '.remove-team-member'
    },
    events: {
        'click @ui.removeMember': 'removeMember'
    },
    removeMember: function () {
        this.model.set('id', null);
        this.model.destroy();
    },
    onShow: function () {
        
    },
    serializeData: function () {
        return this.model.toJSON();
    }
});

var TeamView = Backbone.Marionette.CompositeView.extend({
    template: "#team",    
    childViewContainer: "tbody",
    childView: TeamMemberListItemView,
    emptyView: EmptyListView,
    ui: {
        'createMember': '.create-team-member',
        'back': '.create-team-back',
        'memberName': '.member-name',
        'hidden': '.team-hidden',
        'teamName': '.team-name',
        'submit': '.create-team'
    },
    events: {
        'click @ui.createMember': 'createMember',
        'click @ui.back': 'back',
        'change @ui.teamName': 'writeTeamName',
        'change @ui.hidden': 'changeHidden'
    },
    triggers: {
        'click @ui.submit': 'submit',
        'click @ui.back': 'back'
    },
    writeTeamName: function() {
        this.model.set('name', this.ui.teamName.val());
    },
    changeHidden: function () {
        this.model.set('hidden', this.ui.hidden.prop('checked'));
    },
    createMember: function () {
        var name = this.ui.memberName.val();
        if (!name || name == '')
            return;

        this.collection.add({ name: name });
        this.ui.memberName.val('');
    },
    initialize: function (options) {
    },
    serializeData: function() {
        var model = this.model.toJSON();
        console.log('[TeamView] ', model);
        return model;
    }
});