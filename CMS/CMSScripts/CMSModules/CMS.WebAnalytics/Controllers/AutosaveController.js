cmsdefine([], function () {

    return function (dataFromServer) {
        var Controller = function ($scope) {
            $scope.model = dataFromServer;
        };

        return ['$scope', Controller];
    };
});