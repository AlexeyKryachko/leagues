var SplittedLayout = Backbone.Marionette.LayoutView.extend({
    template: "#splitted-layout",
    regions: {
        leftRegion: "#left-region",
        rightRegion: "#right-region"
    }
});
