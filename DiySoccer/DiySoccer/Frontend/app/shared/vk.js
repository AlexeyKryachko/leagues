var vkModule = Marionette.Module.extend({
    startWithParent: false,

    initialize: function (options, app, object) {
        var vk = {
            data: {},
            appID: 5370269, // как веб сайт
            appPermissions: 16,
            //инициализация
            init: function () {
                VK.init({ apiId: vk.appID });
            },
            //метод входа
            login: function (callback) {
                function authInfo(response) {
                    if (response.session) { // Авторизация успешна
                        vk.data.user = response.session.user;
                        if (callback)
                            callback(vk.data.user);
                        console.log(response);
                    } else {
                        alert("Авторизоваться не удалось!");
                    }
                }
                VK.Auth.login(authInfo, vk.appPermissions);
            },
            //метод проверки доступа
            access: function (callback) {
                VK.Auth.getLoginStatus(function (response) {
                    if (response.session) { // Пользователь авторизован
                        callback(vk.data.user);
                    } else { // Пользователь не авторизован
                        vk.login(callback);
                    }
                })
            },
            logout: function () {
                VK.Auth.logout();
                this.data.user = {};
                alert('вы вышли');
            }
        }

        this.vk = vk;
    },

    onStart: function (options) {
        this.vk.init();
        this.vk.login();
    },
    onStop: function (options) {
    }
});

MyApp.module("vk", vkModule);