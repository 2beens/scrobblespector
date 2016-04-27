'use strict';

/**
 * @ngdoc overview
 * @name scrobblespectorApp
 * @description
 * # scrobblespectorApp
 *
 * Main module of the application.
 */
angular
  .module('scrobblespectorApp', [
    'ngResource',
    'ngRoute'
  ])
  .config(function ($routeProvider) {
      $routeProvider
        .when('/', {
            templateUrl: 'app/views/index.html',
            controller: 'TestController',
            controllerAs: 'vm'
        })        
        .otherwise({
            redirectTo: '/'
        });
  });


//var scrobblespectorApp = angular.module('scrobblespectorApp', [])
//    .controller('testController', ['$scope', function ($scope) {
//        $scope.name = 'ST';
//    }]);