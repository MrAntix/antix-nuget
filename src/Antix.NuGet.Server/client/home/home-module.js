'use strict';

angular.module('home', [
        'ngCookies',
        'packages'
])
    .controller(
        'HomeController',
        [
            '$scope',
            function (
                $scope) {


            }
        ])
    .directive('scrollTop',
    [
        '$window', '$document',
        function (
            $window, $document) {

            return {
                restrict: 'A',
                replace: false,
                link: function (scope, element, attrs) {

                    var scrollTop = function () {
                        return ($window.pageYOffset !== undefined)
                            ? $window.pageYOffset
                            : ($document.documentElement || $document.body.parentNode || $document.body).scrollTop;
                    }

                    var target = angular.element($window),
                        handler = function () {
                            var scrollTopValue = scrollTop();
                            element.attr({ 'scroll-top': scrollTopValue });
                            element.toggleClass("scroll-top-at-top", scrollTopValue < 20);
                        };

                    target.on('scroll', scope.$apply.bind(scope, handler));

                    handler();
                }
            };
        }
    ]);