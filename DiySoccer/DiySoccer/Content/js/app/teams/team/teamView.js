require("bootstrap-3-typeahead");
var SharedViews = require("../../shared/views.js");
var $ = require('jquery');

var TeamMemberListItemView = Backbone.Marionette.ItemView.extend({
    tagName: 'tr',
    template: "#team-member",
    ui: {
        'removeMember': '.remove-team-member'
    },
    events: {
        'click @ui.removeMember': 'removeMember'
    },
    removeMember: function () {
        this.model.set('id', null);
        this.model.destroy();
    },
    onShow: function () {
        
    },
    serializeData: function () {
        return this.model.toJSON();
    }
});

var TeamView = Backbone.Marionette.CompositeView.extend({
    template: "#team",    
    childViewContainer: "tbody",
    childView: TeamMemberListItemView,
    emptyView: SharedViews.EmptyListView,
    ui: {
        'createMember': '.create-team-member',
        'existMember': '.exist-member-name',
        'back': '.create-team-back',
        'memberName': '.member-name',
        'hidden': '.team-hidden',
        'teamName': '.team-name',
        'submit': '.create-team',
        'upload': '#btUpload',
        'logoContainer': '.logo-container',
        'logoValue': '#logo-file-id',
        'description': '#description'
    },
    events: {
        'click @ui.createMember': 'createMember',
        'click @ui.back': 'back',
        'change @ui.teamName': 'writeTeamName',
        'change @ui.description': 'changeDescription',
        'change @ui.hidden': 'changeHidden',
        'click @ui.upload': 'uploadImage'
    },
    triggers: {
        'click @ui.submit': 'submit',
        'click @ui.back': 'back'
    },
    uploadImage: function () {
        var self = this;

        var data = new FormData($('#logo-upload-form')[0]);
        $.ajax({
            type: "POST",
            url: '/api/upload/logo/',    // CALL WEB API TO SAVE THE FILES.
            enctype: 'multipart/form-data',
            contentType: false,
            processData: false,         // PREVENT AUTOMATIC DATA PROCESSING.
            cache: false,
            data: data, 		        // DATA OR FILES IN THIS CONTEXT.
            success: function (data, textStatus, xhr) {
                self.ui.logoContainer.html('<img src="/api/image/' + data.id + '" />');
                self.model.set('media', data.id);
                document.getElementById("logo-upload-form").reset();
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                self.logoContainer.html('');
                document.getElementById("logo-upload-form").reset();
            }
        });
    },
    writeTeamName: function() {
        this.model.set('name', this.ui.teamName.val());
    },
    changeHidden: function () {
        this.model.set('hidden', this.ui.hidden.prop('checked'));
    },
    createMember: function () {
        var name = this.ui.memberName.val();
        if (!name || name == '')
            return;

        this.collection.add({ name: name });
        this.ui.memberName.val('');
    },
    onShow: function () {
        var self = this;

        /*tinymce.init({
            selector: '#description',
            height: 300,
            toolbar: 'undo redo | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image',
            setup: function (ed) {
                ed.on('change', function(e) {
                    self.changeDescription(ed.getContent());
                });
            }
        });*/
        this.ui.description.val(this.model.get('description'));
    },
    changeDescription: function(content) {
        this.model.set('description', content);
    },
    onRender: function () {
        var self = this;
        
        $(this.ui.existMember).typeahead({
            source: function (query, process) {
                var url = '/api/league/' + self.options.leagueId + '/users?page=0&pageSize=10';
                if (self.model.get('id'))
                    url += '&exceptTeamIds=' + self.model.get('id');
                return $.get(url, { query: query }, function (response) {
                    return process(response);
                });
            },
            displayText: function (item) {
                return item.name;
            },
            updater: function (item) {
                self.collection.add(item);
                return '';
            }
        });
    },
    initialize: function (options) {
        this.options = options;
    },
    serializeData: function() {
        var model = this.model.toJSON();
        return model;
    }
});

module.exports = {
    TeamView: TeamView
}