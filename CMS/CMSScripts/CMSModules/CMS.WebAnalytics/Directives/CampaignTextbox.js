cmsdefine([], function () {
    var Controller = function ($scope) {
        this.$scope = $scope;
        this.$scope.inputFieldUpdate = this.inputFieldUpdate.bind(this);
    },
        directive = function () {
            return {
                restrict: "E",
                template: 
'<div class="{{classes}}" title="{{title}}">' +
    '<label for="{{id}}" class="control-label" data-ng-if="ngIf()">{{label}}:</br></label>' +
    '<input type="text" ' +
        'data-ng-if="ngIf()"' +
        'class="form-control" ' +
        'data-ng-change="inputFieldUpdate()" ' +
        'data-ng-model="value" ' +
        'data-ng-pattern="pattern" ' +
        'data-ng-required="required" ' +
        'data-ng-maxLength="maxlength" ' +
        'data-ng-disabled="disabled()"' +
        'name="{{id}}" +/>' +
    '<span class="form-control-error" data-ng-if="model.$error.pattern && ngIf()">{{patternError|resolve}}</span>' +
    '<span class="form-control-error" data-ng-if="model.$error.required && ngIf()">{{"general.requiresvalue"|resolve}}</span>' +
    '<span class="form-control-error" data-ng-if="model.$error.serverValidation && ngIf()">{{model.serverValidationMessage}}</span>' +
'</div>',
                replace: true,
                require: "^form",
                scope: {
                    value: "=",
                    id: "@",
                    title: "@",
                    label: "@",
                    required: "@",
                    maxlength: "@",
                    inputType: "@",
                    classes: "@",
                    pattern: "@",
                    patternError: "@",
                    disabled: "&",
                    if: "&"
                },
                controller: Controller,
                link: function ($scope, $element, $attrs, $ctrl) {
                    $scope.ngIf = function () { return $scope.if() !== false; };
                    $scope.form = $ctrl;
                }
            };
        };


    /**
     * Keeps the value of input in required format.
     */
    Controller.prototype.inputFieldUpdate = function () {
        this.$scope.model = this.$scope.model || this.$scope.form[this.$scope.id];
        this.$scope.model.$setValidity("serverValidation", true);
        var value = this.$scope.model.$viewValue;

        if (this.$scope.inputType == "positive-integer") {
            value = value.replace(/[^0-9]/g, '');
            value = value.replace(/0*([0-9]+)/g, '$1');
        }
        else if (this.$scope.inputType == "positive-float") {
            value = value.replace(/[^0-9.]/g, '');
            value = value.replace(/(\..*)\./g, '$1');
            value = value.replace(/0*([0-9][0-9.]*)/g, '$1');
        }

        this.$scope.model.$setViewValue(this.$scope.value = value);
        this.$scope.model.$render();
    };
    
    Controller.$inject = [
        '$scope'
    ];

    return [directive];
});