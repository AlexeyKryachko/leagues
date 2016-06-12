var GameOptionsView = Backbone.Marionette.ItemView.extend({
    template: "#custom-game",
    ui: {
        'customScoring': '.custom-scoring',
        'eventChange': '.event-change'
    },
    events: {
        'change @ui.customScoring': 'customScoring',
        'change @ui.eventChange': 'eventChange'
    },
    eventChange: function() {
        var value = this.ui.eventChange.val();
        this.model.set('eventId', value);
    },
    customScoring: function () {
        var value = this.ui.customScoring.prop('checked');
        this.model.set('customScores', value);
        this.trigger('scoring:changed', value);
    },
    serializeData: function () {
        var model = this.model.toJSON();

        if (!model.events)
            return model;

        _.each(model.events, function(obj) {
            if (obj.id == model.eventId)
                obj.selected = true;
        });

        return model;
    }
});
