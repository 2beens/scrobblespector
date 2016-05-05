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
                    getApiFullUrl:     getApiFullUrl,
                    getArtistsApiPath: getArtistsApiPath,
                    getUsersApiPath:   getUsersApiPath,
                    isNullOrUndef:     isNullOrUndef,
                    isNotNullAndUndef: isNotNullAndUndef
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

                function isNullOrUndef(object) {
                    return (object === null || object === undefined);
                }

                function isNotNullAndUndef(object) {
                    return (object !== null && object !== undefined);
                }
            })();

            this.$get = function () {
                return utilizr;
            };

            this.utilizr = utilizr;
        }]);
})();