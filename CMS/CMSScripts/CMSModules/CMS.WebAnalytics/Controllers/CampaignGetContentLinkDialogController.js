cmsdefine(['Underscore', 'CMS/EventHub', 'CMS/UrlHelper', 'CMS.WebAnalytics/ModalDialog', "jQuery"], function (_, EventHub, UrlHelper, ModalDialog, $) {
    return function () {
        var Controller = function ($scope, resolveFilter, serverData) {
            
            this.resolveFilter = resolveFilter;
            this.$scope = $scope;
            this.serverData = serverData;

            $scope.$watchGroup(['utmSource', 'utmMedium'], this.utmParametersChanges.bind(this));
            $scope.textAreaClick = this.textAreaClick;
            
            EventHub.subscribe('CampaignAssetGetContentLink', this.openDialog.bind(this));
        };

        
        Controller.prototype.openDialog = function (asset) {
            this.asset = asset;

            if (!this.dialog) {
                this.dialog = new ModalDialog("#get-content-link-dialog-container", {
                    title: "Get link",
                    width: 800,
                    height: 420,
                    buttons: [{
                        title: this.resolveFilter('general.close'),
                        action: this.onDialogButtonAction.bind(this)
                    }],
                    resourceStrings: {
                        maximize: this.resolveFilter('general.maximize'),
                        restore: this.resolveFilter('general.restore'),
                        close: this.resolveFilter('general.close'),
                    }
                });
            }
            
            this.$scope.link = this.resolveFilter("campaign.getcontentlink.dialog.emptyutmsource");
            this.clearModel();
            this.dialog.open();
        };

        /**
         * Raises whenever utmSource or utmMedium properties on scope have changed.
         * Live site link is then updated accordingly.
         *
         * @param       newValues       array       contains both new UTM source value (at index 0) and new UTM medium value (at index 1)
         */
        Controller.prototype.utmParametersChanges = function (newValues) {
            if (this.asset && !this.$scope.getContentLinkForm.$error.pattern) {
                this.$scope.link = this.buildLink(this.asset.additionalProperties.liveSiteLink, newValues[0], newValues[1]);
            }
        };


        /**
         * Builds live site link with both the UTM medium and UTM source query parameters.
         * 
         * @param       originalLink    string      original live site link
         * @param       utmSource       string      UTM source to be added as query string. If empty, query string will not be added
         * @param       utmMedium       string      UTM medium to be added as query string. If empty, query string will not be added
         * @return                      string      URL containing original link and the UTM parameters
         */
        Controller.prototype.buildLink = function (originalLink, utmSource, utmMedium) {
            var originalLinkWithoutQueryString = UrlHelper.removeQueryString(originalLink),
                queryParams = UrlHelper.getParameters(originalLink),
                utmCampaign = this.serverData.campaign.utmCode;

            if (utmCampaign) {
                queryParams.utm_campaign = utmCampaign;
            }

            if (!utmSource) {
                return this.resolveFilter("campaign.getcontentlink.dialog.emptyutmsource");
            }

            queryParams.utm_source = utmSource;
            
            if (utmMedium) {
                queryParams.utm_medium = utmMedium;
            }
            
            return originalLinkWithoutQueryString + UrlHelper.buildQueryString(queryParams);
        };


        /**
         * If clicked event is textarea, selects all text within the control.
         */
        Controller.prototype.textAreaClick = function(event) {
            if (event.target.tagName === "TEXTAREA") {
                event.target.select();
            }
        };


        /**
         * Clears up the model, so after reopening the dialog the scope values are empty.
         */
        Controller.prototype.clearModel = function () {
            this.$scope.getContentLinkForm.$setPristine();
            this.$scope.utmSource = null;
            this.$scope.utmMedium = null;
        };


        /**
         * Handles click on the only close within the dialog. Closes the dialog.
         */
        Controller.prototype.onDialogButtonAction = function () {
            this.dialog.close();
        };

        return ['$scope', 'resolveFilter', 'serverData', Controller];
    };
});