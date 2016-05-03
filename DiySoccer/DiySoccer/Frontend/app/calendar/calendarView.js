var CalendarListItemView = Backbone.Marionette.ItemView.extend({
    tagName: 'tr',
    template: "#calendar-event",
    ui: {
        
    },
    events: {
        
    },
    removeMember: function () {
    },
    onShow: function () {
        
    },
    serializeData: function () {
        return this.model.toJSON();
    }
});

var CalendarView = Backbone.Marionette.CompositeView.extend({
    template: "#calendar",    
    childViewContainer: "tbody",
    childView: CalendarListItemView,
    emptyView: EmptyListView,
    ui: {
    },
    events: {
    },
    triggers: {
        'click @ui.submit': 'submit',
        'click @ui.back': 'back'
    },
    onShow: function () {
        
    },
    
    onRender: function () {
        
    },
    initialize: function (options) {
        this.options = options;
    },
    serializeData: function() {
        var model = this.model.toJSON();
        console.log('[CalendarView] ', model);
        return model;
    }
});