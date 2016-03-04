var LayoutView = Backbone.Marionette.LayoutView.extend({
    template: "#layout",
    regions: {
        up: "#up-region",
        center: "#center-region",
        down: "#down-region"
    }
});
