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
        '$scope',
        'PackagesService',
        function (
            $scope,
            PackagesService) {

            PackagesService.get(function (response) {
                $scope.packages = response.packages;
            });
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