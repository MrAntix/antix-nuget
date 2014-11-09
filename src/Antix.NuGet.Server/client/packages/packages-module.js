'use strict';

angular.module('packages', [
    'ngResource', 'ngSanitize'
])

    .directive('packages',
    [
        function () {

            return {
                restrict: 'E',
                replace: true,
                templateUrl: 'client/packages/packages.cshtml',
                link: function () {

                },
                controller: 'PackagesController'
            };
        }
    ])

    .filter('packageDependencies',
    [
        function () {

            return function (dependenciesString) {

                return "<li>" +
                    dependenciesString
                    .replace(/:/g, ' ')
                    .split('|').join('</li><li>')
                    + "</li>";
            }
        }
    ])

    .controller('PackagesController',
    [
        '$scope', '$interval',
        'PackagesService',
        function (
            $scope, $interval,
            PackagesService) {

            var load = function() {

                PackagesService.get(function(response) {
                    $scope.packages = response.packages;
                });
            }

            $interval(load, 1000);
        }
    ])

    .factory('PackagesService',
    [
        '$resource',
        function ($resource) {

            return $resource('', {}, {
                get: {
                    url: '/feed/Packages()',
                    method: 'GET'
                }
            });
        }
    ]);