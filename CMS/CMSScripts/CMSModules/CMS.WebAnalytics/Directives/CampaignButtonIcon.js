cmsdefine([], function () {
    return [function () {
        return {
            restrict: "E",
            replace: true,
            template:
'<div title="{{title}}">' +
    '<button type="button" value="" data-ng-disabled="disabled()" data-ng-class=\'{ "icon-only": true, "btn-icon": true, "btn": true, "btn-disabled": disabled() }\' title="{{title}}" data-ng-click="click()">' +
        '<i aria-hidden="true" class="{{iconClass}}"></i>' +
        '<span class="sr-only">Delete</span>' +
    '</button>' +
'</div>',
            scope: {
                title: "@",
                iconClass: "@",
                click: "&",
                disabled: "&"
            }
        };
    }];
});