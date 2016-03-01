var teamView = Backbone.Marionette.ItemView.extend({
    tagName: 'tr',
    className: 'cursor-pointer',
    template: "#team-item",
    events:{
        'click': 'onRedirect'
    },
    onRedirect: function () {
        document.location.href = document.location.href + '/team/' + this.model.get('id');
    },
    onShow: function () {
        console.log('[teamView] shown')
    },
    serializeData: function () {
        return this.model.toJSON();
    }
});

var TableView = Backbone.Marionette.CompositeView.extend({
    template: "#team-list",    
    childViewContainer: "tbody",
    childView: teamView,
    onShow: function (options) {
    }
});