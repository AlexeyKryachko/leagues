var _ = require('underscore');

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

var breadcrumpsView = Backbone.Marionette.ItemView.extend({
    template: "#breadcrumps",
    ui: {
    },
    triggers: {
    },
    serializeData: function () {
        var model = this.model.toJSON();

        var breadcrumps = [];
        var canonicalUrl = '';

        _.each(model.breadcrumps, function (obj, index) {
            if (obj.type === 0) {
                canonicalUrl += '#leagues/' + obj.id + '/';
            }

            breadcrumps.push({ url: canonicalUrl, name: obj.name });
        });

        return {
            breadcrumps: breadcrumps
        };
    }
});

module.exports = {
    SaveView: SaveView,
    EmptyListView: EmptyListView,
    CancelView: CancelView,
    breadcrumpsView: breadcrumpsView
};
