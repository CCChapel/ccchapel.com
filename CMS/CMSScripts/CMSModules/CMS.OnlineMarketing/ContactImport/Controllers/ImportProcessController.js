cmsdefine(['CMS/EventHub', 'CMS/NavigationBlocker'], function (EventHub, NavigationBlocker) {
    return ['$scope', '$location', 'resolveFilter', 'csmContactImportService', '$timeout', 'serverData', function ($scope, $location, resolveFilter, contactImportService, $timeout, serverData) {
        var contactFieldsMapping = contactImportService.getContactFieldsMapping(),
            contactGroup = contactImportService.getContactGroup(),
            fileStream = contactImportService.getFileStream(),
            navigationBlocker = new NavigationBlocker();

        EventHub.publish('cms.angularViewLoaded');

        if (!contactFieldsMapping || !contactGroup || !fileStream) {
            $location.path('/');
            return;
        }

        $scope.contactFieldsMapping = contactFieldsMapping;
        $scope.contactGroup = contactGroup;
        $scope.fileStream = fileStream;
        $scope.siteGuid = serverData.siteGuid;

        // Event fired by import directive
        $scope.$on('importStarted', function () {
            navigationBlocker.block(resolveFilter("om.contact.importcsv.confirmleave"));
        });

        $scope.$on('importFinished', function () {
            // Clear data so that import will not start again if going back in history
            contactImportService.clear();

            navigationBlocker.unblock();
        });
    }];
});