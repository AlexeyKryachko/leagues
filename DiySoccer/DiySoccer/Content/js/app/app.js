var MyApp = new Backbone.Marionette.Application();

var MyRouter = require("./router.js");
var Settings = require("./models/settings.js");
var $ = require('jquery');

MyApp.addRegions({
    mainRegion: "#main-container",
    headerRegion: "#main-header"
});

MyApp.on('start', function () {
    var templateIds = ['layout',
        'breadcrumps',
        'splitted-layout',
        'leagues-actions',
        'leagues-list',
        'team-list',
        'team-item',
        'team',
        'team-member',
        'empty-list-view',
        'game-new',
        'game-new-member',
        'game-external-info',
        'game-external-list',
        'save',
        'team-list-actions',
        'custom-game',
        'team-info',
        'team-game',
        'cancel',
        'header',
        'league',
        'league-admin',
        'calendar',
        'calendar-user',
        'calendar-event',
        'calendar-user-event',
        'game-info',
        'game-info-scores',
        'league-info',
        'tournament-info',
        'tournament-info-actions',
        'leagues-statistics'];
    var preloading = Backbone.Marionette.TemplateCache.preloadTemplates(templateIds, this);
    MyApp.Settings = new Settings();

    MyApp.listenToOnce(MyApp.Settings, 'sync', function () {
        MyApp.submodules.header.start();
        var router = new MyRouter(MyApp);
        
        if (Backbone.history) {
            Backbone.history.start();
        }
    });

    $.when(preloading).done(function () {
        MyApp.Settings.fetch();
    });
    
});

module.exports = MyApp;