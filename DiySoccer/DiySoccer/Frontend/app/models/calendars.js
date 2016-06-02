var Calendar = Backbone.Model.extend({
    initialize: function () {
        this.leagueId = 0;
    },
    url: function () {
        return '/api/leagues/' + this.leagueId + '/calendar';
    },
    setLeagueId: function (leagueId) {
        this.leagueId = leagueId;
    }
});

var Event = Backbone.Model.extend({
    initialize: function () {
        this.leagueId = 0;
    },
    url: function () {
        return '/api/leagues/' + this.leagueId + '/events';
    },
    createGame: function () {
        var self = this;

        $.ajax({
            type: "POST",
            url: '/api/leagues/' + self.leagueId + '/events/' + this.id,
            success: function (eventGame) {
                var games = self.get('games');
                games.push(eventGame);
                self.set('games', games);
            }
        });
    },
    deleteGame: function (eventGameId) {
        $.ajax({
            type: "DELETE",
            url: '/api/leagues/' + self.leagueId + '/events/' + this.id + '/' + eventGameId,
            success: function () {
                var newGames = [];
                var games = self.get('games');

                _.each(games, function (obj) {
                    if (id != obj.id)
                        newGames.push(obj);
                });

                self.set('games', newGames);
            }
        });
    },
    setLeagueId: function (leagueId) {
        this.leagueId = leagueId;
    }
});

var Events = Backbone.Collection.extend({
    model: Event
});