var League = Backbone.Model.extend({
    url: function() {
        return this.id 
            ? '/api/leagues/' + this.id
            : '/api/leagues';
    },
    create: function() {
        
    },
    update: function() {
        
    }
});

var Leagues = Backbone.Collection.extend({
    url: function() {
        return '/api/leagues';
    }
});