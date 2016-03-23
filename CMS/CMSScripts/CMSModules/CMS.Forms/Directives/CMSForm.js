cmsdefine([], function () {
    return [function () {
        return {
            restrict: "E",
            template: 
'<form>' +
    '<div data-ng-transclude="" class="form-horizontal analytics_campaign"></div>' +
'</form>',
            transclude: true,
            replace: true
        };
            
    }];
});