'use strict';

var app = angular.module('nugetServer', [
    'ngTouch',
    'ngAnimate',
    'ui.bootstrap',
    'ui.router',
    'home'
]);

app
    .controller(
        'AppController',
        [
            '$log', '$scope',
            function (
                $log, $scope
                ) {

            }
        ])

    .config([
        '$stateProvider', '$urlRouterProvider',
        function ($stateProvider, $urlRouterProvider) {
            $urlRouterProvider.otherwise("/");

            $stateProvider
                .state('home', {
                    url: '/',
                    templateUrl: '/client/home/home.cshtml',
                    controller: 'HomeController'
                });
        }
    ]);