var GameNewOptionsView = Backbone.Marionette.ItemView.extend({
    template: "#custom-game",
    ui: {
        'customScoring': '.custom-scoring',
        'homeTeamScore': '.home-team-score',
        'guestTeamScore': '.guest-team-score',
        'customScoringElements': '.custom-scoring-elements'
    },
    events: {
        'change @ui.customScoring': 'showScoring'
    },
    showScoring: function () {
        if (this.ui.customScoring.prop('checked'))
            this.ui.customScoringElements.show();
        else
            this.ui.customScoringElements.hide();
    }
});
