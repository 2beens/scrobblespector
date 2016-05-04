(function () {
    angular
        .module('scrobblespectorApp.global', [])
        .constant('app', {
            version: '0.1',
            config: {
                baseUrl: 'http://localhost:65231',
                ssApiPath: '/api/lastfm'
            }
        })
        .provider('utilizr', ['app', function (app) {
            var utilizr = (function () {
                var utilizr = {
                    getApiFullUrl: getApiFullUrl,
                    getArtistsApiPath: getArtistsApiPath,
                    getUsersApiPath: getUsersApiPath
                };

                return utilizr;

                function getApiFullUrl() {
                    return app.config.baseUrl + app.config.ssApiPath;
                }

                function getArtistsApiPath() {
                    return app.config.ssApiPath + '/artists';
                }

                function getUsersApiPath() {
                    return app.config.ssApiPath + '/users';
                }
            })();

            this.$get = function () {
                return utilizr;
            };

            this.utilizr = utilizr;
        }]);
})();