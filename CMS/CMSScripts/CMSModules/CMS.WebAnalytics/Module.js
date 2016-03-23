cmsdefine([
        "angular-1.4.5",
        "angular-resource-1.4.5",
        "angular-sanitize-1.4.5",
        "CMS.WebAnalytics/Services/CampaignResource",
        "CMS.WebAnalytics/Services/AssetResource",
        "CMS.WebAnalytics/Services/NewsletterResource",
        "CMS.WebAnalytics/Controllers/AutosaveController",
        "CMS.WebAnalytics/Controllers/AssetController",
        "CMS.WebAnalytics/Controllers/CampaignEditController",
        "CMS.WebAnalytics/Controllers/CampaignListController",
        "CMS.WebAnalytics/Controllers/CampaignNewEmailDialogController",
        "CMS.WebAnalytics/Controllers/CampaignGetContentLinkDialogController",
        "CMS.Forms/Directives/CMSForm",
        "CMS.Forms/Directives/CMSAutosaveFormDirective",
        "CMS.Forms/Directives/CMSSelectPath",
        "CMS.Forms/Directives/CMSSelectItem",
        "CMS.Forms/Directives/CMSCreatePage",
        "CMS.Forms/Directives/CMSModalDialog",
        "CMS.Forms/Directives/CMSTextbox",
        "CMS.Forms/Directives/CMSSelect",
        "CMS.Forms/Directives/CMSTextarea",
        "CMS.Forms/Directives/CMSRadioButton",
        "CMS.WebAnalytics/Directives/CampaignDetailDirective",
        "CMS.Forms/Directives/CMSInputAttributesDirective",
        "CMS/Filters/Resolve",
        "CMS/Filters/StringFormat",
        "CMS/Filters/NumberShortener",
        "CMS/Messages/Module",
        "CMS.WebAnalytics/Directives/CampaignTextarea",
        "CMS.WebAnalytics/Directives/CampaignTextbox",
        "CMS.WebAnalytics/Directives/CampaignAssetListItem",
        "CMS.WebAnalytics/Directives/CampaignButtonIcon"
    ],
    function (
        angular,
        ngResource,
        ngSanitize,
        campaignResource,
        assetResource,
        newsletterResource,
        autosaveController,
        assetController,
        campaignEditController,
        campaignListController,
        campaignNewEmailDialogController,
        campaignGetContentLinkDialog,
        cmsForm,
        cmsAutosaveForm,
        cmsSelectPath,
        cmsSelectItem,
        cmsCreatePage,
        cmsModalDialog,
        cmsTextbox,
        cmsSelect,
        cmsTextarea,
        cmsRadioButton,
        campaignDetailDirective,
        cmsInputAttributes,
        resolveFilter,
        stringFormatFilter,
        numberShortener,
        messagesModule,
        campaignTextarea,
        campaignTextbox,
        campaignAssetListItem,
        campaignButtonIcon
    ) {

        return function (dataFromServer) {
            var moduleName = "Campaigns",
                module = angular.module(moduleName, [
                    "ngResource",
                    "ngSanitize",
                    resolveFilter(angular, dataFromServer.resources),
                    stringFormatFilter(angular),
                    numberShortener(angular),
                    messagesModule(angular)
                ]);

            module.controller("Autosave", autosaveController(dataFromServer));
            module.controller("Asset", assetController(dataFromServer));
            module.controller("CampaignEdit", campaignEditController(dataFromServer));
            module.controller("CampaignList", campaignListController());
            module.controller("CampaignNewEmailDialog", campaignNewEmailDialogController());
            module.controller("CampaignGetContentLinkDialog", campaignGetContentLinkDialog());

            module.directive("cmsForm", cmsForm);
            module.directive("cmsAutosave", cmsAutosaveForm);
            module.directive("campaignDetail", campaignDetailDirective);
            module.directive("cmsSelectPath", cmsSelectPath);
            module.directive("cmsSelectItem", cmsSelectItem);
            module.directive("cmsCreatePage", cmsCreatePage);
            module.directive("cmsModalDialog", cmsModalDialog);
            module.directive("cmsTextbox", cmsTextbox);
            module.directive("cmsSelect", cmsSelect);
            module.directive("cmsTextarea", cmsTextarea);
            module.directive("cmsRadioButton", cmsRadioButton);
            module.directive("cmsInputAttributes", cmsInputAttributes);
            module.directive("campaignTextbox", campaignTextbox);
            module.directive("campaignTextarea", campaignTextarea);
            module.directive("campaignAssetListItem", campaignAssetListItem);
            module.directive("campaignButtonIcon", campaignButtonIcon);

            module.factory("cmsCampaignResource", campaignResource);
            module.factory("cmsAssetResource", assetResource);
            module.factory("cmsNewsletterResource", newsletterResource);

            module.factory("authorizeInterceptor", ["$q", function($q) {
                return {
                    'responseError': function (rejection) {
                        // User was signed off, need to redirect to the login page
                        if (rejection.status === 403) {
                            var logonPageUrl = rejection.headers("logonpageurl");

                            if (logonPageUrl) {
                                window.top.location.href = logonPageUrl;
                            }
                        }
                        
                        return $q.reject(rejection);
                    }
                };
            }]);

            // CMSApi does not support PUT and DELETE requests due to security reasons
            // Transform those request to the GET and POST one with appropriate URLs
            module.factory("httpMethodInterceptor", function () {

                return {
                    request: function (config) {
                        if (config.url.indexOf('cmsapi/newsletters/') >= 0) {
                            return config;
                        }

                        if (config.method === "POST") {
                            config.url += "/Post";
                        }

                        if (config.method === "PUT") {
                            config.method = "POST";
                            config.url += "/Put";
                        }
                        
                        if (config.method === "DELETE") {
                            config.method = "GET";
                            config.url += "/Delete";
                        }
                        
                        return config;
                    }
                };
            });
            
            module.config(["$httpProvider", function ($httpProvider) {
                $httpProvider.interceptors.push("authorizeInterceptor");
                $httpProvider.interceptors.push("httpMethodInterceptor");
            }]);

            // Create constant for server data
            module.constant("serverData", dataFromServer);

            return moduleName;
        };
});