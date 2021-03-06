﻿'use strict';

angular.module('packages', [
    'ngResource', 'ngSanitize',
    'antix.cellLayout'
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

                if (!dependenciesString || dependenciesString.length === 0)
                    return '<li>(none)</li>';

                return '<li>' +
                    dependenciesString
                    .replace(/:/g, ' ')
                    .split('|').join('</li><li>')
                    + '</li>';
            }
        }
    ])

    .filter('url',
    [
        function () {

            return function (url) {

                var index = url.indexOf('//') + 1;

                return url.substring(index).replace(/^\/+|\/+$/gm, '');
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

            load();

            $scope.$on('packageStoreEvent:Added', load);
            $scope.$on('packageStoreEvent:Removed', load);
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