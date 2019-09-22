var Handlebars = require('handlebars');
var _ = require('underscore');
var $ = require('jquery');

Backbone.Marionette.TemplateCache.preloadTemplate = function (templateId, context) {
    // preload a single template
    var loader = $.Deferred();
    var that = this;
    var msg;
    var err;
    if (!templateId || templateId.length == 0) {
        err = new Error('No templateId was specified - please provide a valid template id or filename.');
        err.name = "NoTemplateSpecified";
        throw err;
    }
    var hasHasTag = templateId[0] == '#';
    var template = hasHasTag ? $(templateId).html() : null;
    if (template && template.length > 0) {
        // the template was found in the DOM, so just use it; no need to load a file
        Backbone.Marionette.TemplateCache.storeTemplate(templateId, template);
        loader.resolveWith(context);
    } else {
        // load the specified template as an .htm file.
        var fileName = hasHasTag ? templateId.substr(1) : templateId;
        var url = '/Content/templates/' + fileName + '.html';
        $.get(url, function (serverTemplate) {
            if (!serverTemplate || serverTemplate.length == 0) {
                msg = "Could not find template: '" + templateId + "'";
                err = new Error(msg);
                err.name = "NoTemplateError";
                throw err;
            }

            Backbone.Marionette.TemplateCache.storeTemplate(templateId, serverTemplate);
            loader.resolveWith(context);

        });
        return loader;
    }


};

Backbone.Marionette.TemplateCache.preloadTemplates = function (templateIds, context) {
    var loadAllTemplates = $.Deferred();
    var loadTemplatePromises = [];
    var that = this;
    _.each(templateIds, function (templateId, index) {
        loadTemplatePromises[index] = Backbone.Marionette.TemplateCache.preloadTemplate(templateIds[index], that);
    });
    var templatesRemainingToLoad = loadTemplatePromises.length;
    _.each(loadTemplatePromises, function (aLoadPromise) {
        $.when(aLoadPromise).done(function () {
            templatesRemainingToLoad--;
            if (templatesRemainingToLoad == 0)
                loadAllTemplates.resolveWith(context); // 'this' context is the module
        });
    });
    return loadAllTemplates;
};

Backbone.Marionette.TemplateCache.storeTemplate = function (templateId, template) {
    // compile template and store in cache
    template = Handlebars.compile(template);
    if (templateId[0] != "#") templateId = "#" + templateId;
    var cachedTemplate = new Backbone.Marionette.TemplateCache(templateId);
    cachedTemplate.compiledTemplate = template;
    Backbone.Marionette.TemplateCache.templateCaches[templateId] = cachedTemplate;
};