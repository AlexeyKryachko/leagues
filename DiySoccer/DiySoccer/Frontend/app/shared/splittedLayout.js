var SplittedLayout = Backbone.Marionette.LayoutView.extend({
    template: "#splitted-layout",
    regions: {
        up: "#up-region",
        left: "#left-region",
        right: "#right-region",
        down: "#down-region"
    }
});
