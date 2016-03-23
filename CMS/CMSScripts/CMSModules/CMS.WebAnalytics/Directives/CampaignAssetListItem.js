cmsdefine([], function () {
    return [function () {
            return {
                restrict: "E",
                template: '<div class="campaign-assetlist-item row" data-ng-transclude=""></div>',
                transclude: true,
                replace: true
            };
        }];
});