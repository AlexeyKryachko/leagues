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
                appPermissions: 16,
                init: function () {
                    VK.init({ apiId: vk.appID });
                },
                login: function (callback) {
                    function authInfo(response) {
                        if (response.session) {
                            vk.data.user = response.session.user;
                            if (callback)
                                callback(vk.data.user.id, 'vk');
                            console.log(response);
                        } else {
                            alert("Авторизоваться не удалось!");
                        }
                    }
                    VK.Auth.login(authInfo, vk.appPermissions);
                },
                access: function (callback) {
                    VK.Auth.getLoginStatus(function(response) {
                        if (response.session) {
                            callback(vk.data.user);
                        } else {
                            vk.login(callback);
                        }
                    });
                },
                logout: function () {
                    VK.Auth.logout();
                    this.data.user = {};
                }
        }

        return vk;
    }
});