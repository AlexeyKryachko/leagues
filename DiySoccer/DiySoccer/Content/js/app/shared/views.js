var CancelView = Backbone.Marionette.ItemView.extend({
    template: "#cancel",
    ui: {
        'cancel': '.cancel'
    },
    triggers: {
        'click @ui.cancel': 'cancel'
    }
});


var EmptyListView = Backbone.Marionette.ItemView.extend({
    tagName: 'tr',
    template: "#empty-list-view"
});

var SaveView = Backbone.Marionette.ItemView.extend({
    template: "#save",
    ui: {
        'save': '.save',
        'cancel': '.cancel'

    },
    triggers: {
        'click @ui.save': 'save',
        'click @ui.cancel': 'cancel'
    }
 });

 module.exports = {
     SaveView: SaveView,
     EmptyListView: EmptyListView,
     CancelView: CancelView
 }
