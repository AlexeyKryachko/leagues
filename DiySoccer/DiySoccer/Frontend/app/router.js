var MyRouter = Marionette.AppRouter.extend({
    routes: {
        "": "defaultRoute",
        "leagues/:id": "tableRoute"
    },
    defaultRoute: function () {
        console.log('[Router][defaultRoute]')
        this.changeModule(MyApp.submodules.leagues)
    },
    tableRoute: function () {
        this.changeModule(MyApp.submodules.table)
    },
    changeModule: function (module) {
        if (this.workingModule)
            this.workingModule.stop();

        this.workingModule = module;
        this.workingModule.start();
    }
});