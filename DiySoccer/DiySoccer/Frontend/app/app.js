MyApp = new Backbone.Marionette.Application();

MyApp.addRegions({
    mainRegion: "#main-container",
    headerRegion: "#main-header"
});

MyApp.on('start', function () {
    var templateIds = ['layout', 'splitted-layout', 'leagues-actions', 'leagues-list', 'leagues-item', 'team-list', 'team-item', 'team', 'team-member', 'empty-list-view',
        'game-new', 'game-new-member', 'save', 'team-list-actions', 'custom-game', 'team-info', 'team-game', 'cancel', 'header',
        'league', 'league-admin'];
    var preloading = Backbone.Marionette.TemplateCache.preloadTemplates(templateIds, this);
    MyApp.Settings = new Settings();

    MyApp.listenToOnce(MyApp.Settings, 'sync', function () {
        MyApp.submodules.header.start();
        var router = new MyRouter();
        
        if (Backbone.history) {
            Backbone.history.start();
        }
    });

    $.when(preloading).done(function () {
        MyApp.Settings.fetch();
    });
    
});