'use strict';

angular.module('antix.status', [
    ])
    .directive('aStatus',
    [
        function() {

            return {
                restrict: 'E',
                replace: true,
                templateUrl: 'Scripts/antix/status/antix-status.cshtml',
                controller: 'AntixStatusController'
            };
        }
    ])
    .constant('AntixStatusEvents', {
        Spin: 'antix.status:Spin',
        Unspin: 'antix.status:Unspin',
        Status: 'antix.status:Status',
        StatusScope: 'antix.status:StatusScope'
    })
    .controller('AntixStatusController', [
        '$log', '$scope', '$timeout',
        'AntixStatusEvents',
        function(
            $log, $scope, $timeout,
            AntixStatusEvents) {
            $log.debug('AntixStatusController.init');

            var spins = 0,
                showTimeout,
                statusCurrent = null,
                statusScope;

            $scope.$on(AntixStatusEvents.Spin, function(e, message) {

                spins++;

                if (!showTimeout) {

                    showTimeout = $timeout(function() {
                        $log.debug('AntixStatusController.spin timeout');
                        $scope.showStatus({
                            message: message || 'please wait ...',
                            type: 'info'
                        });

                        showTimeout = null;
                    }, 400);
                }

                $log.debug('AntixStatusController.spin ' + spins);
            });

            $scope.$on(AntixStatusEvents.Unspin, function(e) {

                spins--;

                if (spins <= 0) {
                    if (showTimeout) {
                        $timeout.cancel(showTimeout);
                        showTimeout = null;
                    }

                    spins = 0;

                    $scope.showStatus(statusCurrent);
                }

                $log.debug('AntixStatusController.unspin ' + spins);
            });

            $scope.$on(AntixStatusEvents.Status, function(e, status) {
                statusCurrent = status;
                $scope.showStatus(status);
            });

            $scope.$on(AntixStatusEvents.StatusScope, function(e, newStatusScope, status) {
                statusScope = newStatusScope;

                statusCurrent = status;
                $scope.showStatus(status);
            });

            $scope.showStatus = function(status) {
                $log.debug('AntixStatusController.showStatus ' + JSON.stringify(status));

                var scope = statusScope || $scope;

                scope.status = status;
                scope.show = status && !statusScope;

                if (status && status.duration) {
                    $timeout(function() {
                        $log.debug('AntixStatusController.showStatus.hide');

                        scope.show = false;
                        statusCurrent = null;

                    }, status.duration);
                }
            }

            $scope.hide = function() {
                $log.debug('AntixStatusController.hide ');

                var scope = statusScope || $scope;

                scope.status = null;
                scope.show = false;
                statusCurrent = null;
            }
        }
    ]);