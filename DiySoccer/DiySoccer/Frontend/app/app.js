MyApp = new Backbone.Marionette.Application();

MyApp.addRegions({
    mainRegion: "#main-container"
});

MyApp.on('start', function () {
    console.log('[Application] started')

    var templateIds = ['leagues-template', 'leagues-list', 'leagues-item', 'team-list', 'team-item' ];
    var preloading = Backbone.Marionette.TemplateCache.preloadTemplates(templateIds, this);
    $.when(preloading).done(function () {
        new MyRouter();
        if (Backbone.history) {
            Backbone.history.start();
        }
    });
    
});