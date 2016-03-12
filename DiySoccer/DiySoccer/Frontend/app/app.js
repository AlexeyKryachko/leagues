MyApp = new Backbone.Marionette.Application();

MyApp.addRegions({
    mainRegion: "#main-container"
});

MyApp.on('start', function () {
    var templateIds = ['layout', 'splitted-layout', 'leagues-list', 'leagues-item', 'team-list', 'team-item', 'team-new', 'team-new-member', 'empty-list-view',
        'game-new', 'game-new-member', 'save', 'team-list-actions', 'custom-game', 'team-games', 'team-game'];
    var preloading = Backbone.Marionette.TemplateCache.preloadTemplates(templateIds, this);
    $.when(preloading).done(function () {
        new MyRouter();
        if (Backbone.history) {
            Backbone.history.start();
        }
    });
    
});