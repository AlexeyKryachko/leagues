var GameOptionsView = Backbone.Marionette.ItemView.extend({
    template: "#custom-game",
    ui: {
        'customScoring': '.custom-scoring'
    },
    events: {
        'change @ui.customScoring': 'customScoring'
    },
    customScoring: function () {
        var value = this.ui.customScoring.prop('checked');
        this.model.set('customScores', value);
        this.trigger('scoring:changed', value);
    }
});
