cmsdefine([
    'angular',
    'CMS.OnlineMarketing/ContactImport/Services/ContactResource',
    'CMS.OnlineMarketing/ContactImport/Services/ContactImportService',
    'CMS.OnlineMarketing/ContactImport/Services/ContactGroupResource'],
    function (
        angular,
        contactResource,
        contactImportService,
        contactGroupResource) {

        var moduleName = 'cms.onlinemarketing.contactimport.services';

        angular.module(moduleName, [])
            .factory('cmsContactResource', contactResource)
            .factory('cmsContactGroupResource', contactGroupResource)
            .factory('csmContactImportService', contactImportService);

        return moduleName;
    });
