cmsdefine([], function () {
    var Controller = function ($scope) {
        this.$scope = $scope;
    },
        directive = function () {
            return {
                restrict: "E",
                template:
'<div data-ng-class="{\'form-group\': true, \'has-error\': (form.$submitted || model.$dirty) && model.$invalid}" ' +
      'title="{{title}}">' +
    '<div class="editing-form-label-cell">' +
        '<label for="{{id}}" class="control-label editing-form-label">' +
            '{{label}}:<span class="required-mark" data-ng-if="required">*</span>' +
        '</label>' +
    '</div>' +
    '<div class="editing-form-value-cell">' +
        '<div class="editing-form-control-nested-control">' +
            '<select class="form-control"' +
                    'id="{{id}}"  ' +
                    'data-ng-model="value" ' +
                    'data-ng-required="required" ' +
                    'name="{{id}}">' +
                '<option data-ng-repeat="option in options track by $index"   ' +
                        'value="{{option.id}}"  ' +
                        'data-ng-disabled="option.disabled" ' +
                        'data-ng-selected="option.selected">{{option.name}} ' +
                '</option>  ' +
            '</select>' +
            '<span class="form-control-loading" data-ng-if="showSpinner"><i class="icon-spinner spinning cms-icon-80" aria-hidden="true"></i>{{"general.loading"|resolve}}</span>' +
            '<span class="form-control-error" data-ng-if="(form.$submitted || model.$dirty) && model.$error.required">' +
                '{{"general.requiresvalue"|resolve}}' +
            '</span>' +
        '</div>' +
    '</div>' +
'</div>',
                replace: true,
                require: "^form",
                scope: {
                    value: "=",
                    required: "=",
                    options: '=',
                    id: "@",
                    title: "@",
                    label: "@",
                    showSpinner: "="
                },
                controller: Controller,
                link: function ($scope, $element, $attrs, form) {
                    $scope.form = form;
                    $scope.model = form[$scope.id];
                }
            };
        };

    Controller.$inject = [
      '$scope'
    ];

    return [directive];
});