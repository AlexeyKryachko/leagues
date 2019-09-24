var LayoutView = Backbone.Marionette.LayoutView.extend({
    template: "#layout",
    regions: {
        breadcrumbs: "#breadcrumbs-region",
        up: "#up-region",
        center: "#center-region",
        down: "#down-region"
    }
});

var SplittedLayout = Backbone.Marionette.LayoutView.extend({
    template: "#splitted-layout",
    regions: {
        up: "#up-region",
        left: "#left-region",
        right: "#right-region",
        down: "#down-region"
    }
});

module.exports = {
    LayoutView: LayoutView,
    SplittedLayout: SplittedLayout
}