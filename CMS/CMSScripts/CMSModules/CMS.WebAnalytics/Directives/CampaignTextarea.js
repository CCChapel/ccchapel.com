cmsdefine([], function () {
    return [function () {
            return {
                restrict: "E",
                template: 
'<div class="{{classes}}"  title="{{title}}">' +
    '<label for="{{id}}" class="control-label">{{label}}:</br></label>' +
    '<textarea class="form-control" data-ng-model="value" cms-input-attributes=""></textarea>' +
'</div>',
                replace: true,
                scope: {
                    cols: "@",
                    rows: "@",
                    title: "@",
                    id: "@",
                    label: "@",
                    value: "=ngModel",
                    required: "@",
                    maxlength: "@",
                    classes: "@"
                }
            };
        }];
});