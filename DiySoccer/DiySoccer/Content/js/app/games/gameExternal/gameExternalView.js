var _ = require('underscore');
var $ = require('jquery');

var gameExternalView = Backbone.Marionette.ItemView.extend({
    template: "#game-external-info",
    ui: {
        'changeEvent': '.change-event-js',
        'changeGame': '.change-game-js',
        'changeScore': '.score-js',
        'changeHelp': '.help-js',
        'changeHomeBest': '.change-home-best-js',
        'changeGuestBest': '.change-guest-best-js',
    },
    events: {
        'change @ui.changeEvent': 'changeEvent',
        'change @ui.changeGame': 'changeGame',
        'change @ui.changeScore': 'changeScore',
        'change @ui.changeHelp': 'changeHelp',
        'change @ui.changeHomeBest': 'changeHomeBest',
        'change @ui.changeGuestBest': 'changeGuestBest'
    },
    setLeagueId: function (leagueId) {
        this.leagueId = leagueId;
    },
    changeEvent: function() {
        var value = this.ui.changeEvent.val();

        console.log('gameExternalView:event:' + value);

        var events = this.model.get('events');
        _.each(events, function (obj) {
            if (obj.id === value) {
                obj.selected = true;
            } else {
                obj.selected = false;
            }
        });

        this.model.set('events', events);
        this.model.trigger('view:change');
    },
    changeGame: function () {
        var value = this.ui.changeGame.val();

        console.log('gameExternalView:game:' + value);

        var events = this.model.get('events');
        _.each(events, function (obj) {

            _.each(obj.games, function (game) {
                if (game.value === value) {
                    game.selected = true;
                } else {
                    game.selected = false;
                }
            });
        });
        
        this.model.set('events', events);
        this.model.trigger('view:change');
    },
    changeHomeBest: function (event) {
        var value = $(event.currentTarget).val();
        
        var events = this.model.get('events');
        _.each(events, function (obj) {
            _.each(obj.games, function (game) {
                if (game.selected === true) {
                    _.each(game.homeTeam.members, function (member) {
                        if (member.id === value) {
                            member.selected = true;
                            game.homeTeam.bestId = value;
                        } else {
                            member.selected = false;
                        }
                    });
                }
            });
        });

        this.model.set('events', events);
    },
    changeGuestBest: function (event) {
        var value = $(event.currentTarget).val();

        var events = this.model.get('events');
        _.each(events, function (obj) {
            _.each(obj.games, function (game) {
                if (game.selected === true) {
                    _.each(game.guestTeam.members, function (member) {
                        if (member.id === value) {
                            member.selected = true;
                            game.guestTeam.bestId = value;
                        } else {
                            member.selected = false;
                        }
                    });
                }
            });
        });

        this.model.set('events', events);
    },
    changeScore: function (event) {
        var value = $(event.currentTarget).val();
        var id = $(event.currentTarget).data('id');

        var events = this.model.get('events');
        _.each(events, function (obj) {
            _.each(obj.games, function (game) {
                if (game.selected === true) {
                    _.each(game.homeTeam.members, function (member) {
                        if (member.id === id) {
                            member.score = value;
                        }
                    });
                    _.each(game.guestTeam.members, function (member) {
                        if (member.id === id) {
                            member.score = value;
                        }
                    });
                }
            });
        });
        this.model.set('events', events);
        this.model.trigger('view:change');
    },
    changeHelp: function (event) {
        var value = $(event.currentTarget).val();
        var id = $(event.currentTarget).data('id');

        var events = this.model.get('events');
        _.each(events, function (obj) {
            _.each(obj.games, function (game) {
                if (game.selected === true) {
                    _.each(game.homeTeam.members, function (member) {
                        if (member.id === id) {
                            member.help = value;
                        }
                    });
                    _.each(game.guestTeam.members, function (member) {
                        if (member.id === id) {
                            member.help = value;
                        }
                    });
                }
            });
        });
        this.model.set('events', events);
        this.model.trigger('view:change');
    },
    serializeData: function () {
        var model = this.model.toJSON();

        console.log('gameExternalView:model:', model);

        return model;
    },
    modelEvents: {
        'sync': 'render',
        'view:change': 'render'
    }
});

module.exports = { gameExternalView: gameExternalView }