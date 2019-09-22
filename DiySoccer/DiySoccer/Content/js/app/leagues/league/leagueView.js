var Views = require("../../shared/views.js");
var $ = require('jquery');
require("bootstrap-3-typeahead");

var LeagueAdminListItemView = Backbone.Marionette.ItemView.extend({
    tagName: 'tr',
    template: "#league-admin",
    ui: {
        'removeAdmin': '.remove-league-admin'
    },
    events: {
        'click @ui.removeAdmin': 'removeAdmin'
    },
    removeAdmin: function () {
        this.model.set('id', null);
        this.model.destroy();
    },
    onShow: function () {

    },
    serializeData: function () {
        return this.model.toJSON();
    }
});

var LeagueView = Backbone.Marionette.CompositeView.extend({
    template: "#league",
    childViewContainer: "tbody",
    childView: LeagueAdminListItemView,
    emptyView: Views.EmptyListView,
    ui: {
        'addAdmin': '.add-league-admin',
        'name': '.league-name',
        'description': '.league-description',
        'information': '.league-information',
        'group': '.league-vkGroup',
        'upload': '#btUpload',
        'logoContainer': '.logo-container',
        'logoValue': '#logo-file-id',
        'type': '#league-type',
        'subName': '.league-sub-name'
    },
    events: {
        'change @ui.name': 'changeName',
        'change @ui.subName': 'changeSubName',
        'change @ui.description': 'changeDescription',
        'change @ui.information': 'changeInformation',
        'change @ui.group': 'changeGroup',
        'change @ui.type': 'changeType',
        'click @ui.upload': 'uploadImage'
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
                self.model.set('mediaId', data.id);
                document.getElementById("logo-upload-form").reset();
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                self.logoContainer.html('');
                document.getElementById("logo-upload-form").reset();
            }
        });
    },
    changeType: function () {
        this.model.set('type', this.ui.type.val());
    },
    changeSubName: function () {
        this.model.set('subName', this.ui.subName.val());
    },
    changeName: function () {
        this.model.set('name', this.ui.name.val());
    },
    changeDescription: function () {
        this.model.set('description', this.ui.description.val());
    },
    changeInformation: function () {
        this.model.set('information', this.ui.information.val());
    },
    changeGroup: function () {
        this.model.set('vkGroup', this.ui.group.val());
    },
    onRender: function () {
        var self = this;

        $(this.ui.addAdmin).typeahead({
            source: function (query, process) {
                var url = '/api/users?page=0&pageSize=10';
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
    serializeData: function() {
        var model = this.model.toJSON();
        model.types = [
            { value: 1, name: 'Лига', selected: model.type == '1' },
            { value: 2, name: 'Турнир', selected: model.type == '2' }
        ];
        return model;
    },
    modelEvents: {
        'sync': 'render'
    }
});

module.exports = { LeagueView: LeagueView }