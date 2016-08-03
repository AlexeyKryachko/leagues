var TournamentsInfo = Backbone.Model.extend({
    url: function () {
        return '/api/tournaments/' + this.id + '/info/';
    }
});

module.exports = {
    TournamentsInfo: TournamentsInfo
}