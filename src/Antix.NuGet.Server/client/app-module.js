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

                var eventsHub = $.connection.eventsHub;
                $.connection.hub.start();
                $log.debug('AppController.hub.start');

                eventsHub.client.raise = function (e) {
                    $log.debug(
                        'Server Event: '
                        + JSON.stringify(e)
                    );
                    $scope.$broadcast(e.name, e.args);
                }

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