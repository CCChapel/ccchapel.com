cmsdefine([
    'angular',
    'CMS.OnlineMarketing/ContactImport/Directives/FileUploadDirective',
    'CMS.OnlineMarketing/ContactImport/Directives/AttributeMappingDirective',
    'CMS.OnlineMarketing/ContactImport/Directives/AttributeMappingControlDirective',
    'CMS.OnlineMarketing/ContactImport/Directives/ImportProcessDirective',
    'CMS.OnlineMarketing/ContactImport/Directives/DownloadDataDirective',
    'CMS/Messages/Module'],
    function (
        angular,
        fileUploadDirective,
        attributeMappingDirective,
        attributeMappingControlDirective,
        importProcessDirective,
        downloadDataDirective,
        messageModule) {

        var moduleName = 'cms.onlinemarketing.contactimport.directives';

        angular.module(moduleName, [messageModule(angular)])
            .directive('cmsFileUploadDirective', fileUploadDirective)
            .directive('cmsAttributeMappingDirective', attributeMappingDirective)
            .directive('cmsAttributeMappingControlDirective', attributeMappingControlDirective)
            .directive('cmsImportProcessDirective', importProcessDirective)
            .directive('cmsDownloadDataDirective', downloadDataDirective);
    
        return moduleName;
    });