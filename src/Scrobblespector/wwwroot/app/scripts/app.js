(function () {
    angular
        .module('scrobblespectorApp', [
            'scrobblespectorApp.global',
            'ngResource',
            'ngRoute'
        ])
        .config(function ($routeProvider) {
            $routeProvider
                .when('/', {
                    templateUrl: '/app/views/index.html',
                    controller: 'TestController',
                    controllerAs: 'vm'
                })
                .when('/users', {
                    templateUrl: '/app/views/users.html',
                    controller: 'TestController',
                    controllerAs: 'vm'
                })
                .when('/artists', {
                    templateUrl: '/app/views/artists.html',
                    controller: 'ArtistsController',
                    controllerAs: 'vm'
                })
                .otherwise({
                    redirectTo: '/'
                });
        })
        .run(function () {
            console.log('App run() started.');
        });
})();