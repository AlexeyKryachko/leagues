var Settings = Backbone.Model.extend({
    initialize: function() {
        this.VKProvider().init();
    },
    urlRoot: "/api/settings",
    url: function () {
        var url = this.urlRoot;

        url = url + "?provider=" + this.get("provider");
        url = url + "?userId=" + this.get("userId");
        
        return url;
    },
    loginVK: function () {
        this.VKProvider().login(this._login.bind(this));
    },
    _login: function (userId, provider) {
        this.set('provider', provider, { silent: true });
        this.set('userId', userId, { silent: true });

        $.ajaxSetup({
            headers: {
                'x-login-provider': 'vk',
                'x-login-user-id': 'userId'
            }
        });

        this.fetch();
    },
    logout: function () {
        if (this.get('provider') == 'vk') {
            this.VKProvider().logout();
        }
        this.set('provider', '', { silent: true });
        this.set('userId', '', { silent: true });
        this.trigger('change');
    },
    VKProvider: function() {
        var vk = {
                data: {},
                appID: 5370269,
                appPermissions: 262144,
                init: function () {
                },
                login: function (callback) {
                    $.get('https://oauth.vk.com/authorize?client_id=' + vk.appID + '&scope=' + vk.appPermissions + '&redirect_uri=' +
                        'http://diysoccer.azurewebsites.net/api/authVk' + '&response_type=code', function () {
                            alert('Управление отдано.');
                        });
                },
                logout: function () {
                }
        }

        return vk;
    }
});