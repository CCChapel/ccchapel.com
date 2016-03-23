cmsdefine(["CMS/EventHub", "Underscore"], function (EventHub, _) {

    return function (dataFromServer) {
        var Controller = function ($scope, $timeout, assetResource, resolveFilter) {
            var that = this;
            this.$scope = $scope;
            this.$scope.resolveFilter = resolveFilter;
            this.$scope.model = {
                assets: dataFromServer.assets,
                isNewsletterModuleLoaded: dataFromServer.isNewsletterModuleLoaded
            };

            this.$scope.campaign = dataFromServer.campaign;
            this.$scope.ensureExistingCampaign = this.ensureExistingCampaign.bind(this);
            this.$scope.autosave = this.autosave.bind(this);
            this.$scope.addPageAsset = this.addPageAsset.bind(this);
            this.$scope.addEmailAsset = this.addEmailAsset.bind(this);
            this.$scope.removeAsset = this.removeAsset.bind(this);

            this.$timeout = $timeout;
            this.assetResource = assetResource;

            for (var key in dataFromServer.assets) {
                EventHub.publish("registerAutosaveContext", this.getContext(key), this.autosave.bind(this));
            }

            // Filters given assets and returns only those who are a promotion channel assets
            $scope.getPromotionChannels = function (items) {
                var result = new Array();
                _.each(items, function (item) {
                    if (item.type === "newsletter.issue") {
                        result.push(item);
                    }
                });
                return result;
            };

            // Filters given assets and returns only those who are a conversion tracking assets
            $scope.getTrackingAssets = function (items) {
                var result = new Array();
                _.each(items, function (item) {
                    if (item.type != "newsletter.issue") {
                        result.push(item);
                    }
                });
                return result;
            };

            $scope.showCreateEmailDialog = function() {
                EventHub.publish('CampaignAssetCreateNewEmail.CreateEmail', that.$scope.model.assets);
            };

            EventHub.subscribe('CampaignAssetCreateNewEmail.EmailCreated', function (emailid) {
                that.$scope.model.newEmailAsset = emailid;
                that.addEmailAsset();
            });

            EventHub.subscribe('CampaignAssetCreateNewEmail.EmailCreatedError', function(response) {
                that.addCampaignAssetFailed(0, response);
            });
            
            $scope.showGetContentLinkDialog = function (asset) {
                if ($scope.campaign.utmCode) {
                    EventHub.publish('CampaignAssetGetContentLink', asset);
                }
            };

            $scope.$on("autosaveForm", function (event, formController) {
                $scope.formController = formController;
            });
        };


        /**
         * Broadcasts that the campaign needs to be saved if creating new campaign.
         */
        Controller.prototype.ensureExistingCampaign = function () {
            if (!this.$scope.campaign.campaignID) {
                if (this.$scope.formController.$valid) {
                    this.$scope.$root.$broadcast("saveCampaign");
                } else {
                    alert(this.$scope.resolveFilter("campaign.initialSave.validationAlert"));
                    return false;
                }
            }
            return true;
        };
        

        /**
         * Performs update over given asset.
         */
        Controller.prototype.autosave = function (asset) {
            EventHub.publish("savingStarted", this.getContext(asset.assetID));
            this.assetResource.update(asset, this.autosaveFinished.bind(this), this.saveFailed.bind(this, asset.assetID));
        };


        /**
         * Sends a request to add a page asset.
         */
        Controller.prototype.addPageAsset = function () {
            var model = {
                campaignId: this.$scope.campaign.campaignID,
                id: this.$scope.model.newPageAsset,
                type: "cms.document"
            };

            this.assetResource.create(model, this.addCampaignAssetFinished.bind(this), this.addCampaignAssetFailed.bind(this, model.type));
        };


        /**
         * Sends a request to add a email asset.
         */
        Controller.prototype.addEmailAsset = function () {
            var model = {
                campaignId: this.$scope.campaign.campaignID,
                id: this.$scope.model.newEmailAsset,
                type: "newsletter.issue"
            };

            this.assetResource.create(model, this.addCampaignAssetFinished.bind(this), this.addCampaignAssetFailed.bind(this, model.type));
        };
        

        /**
         * Sends a request to remove given asset.
         */
        Controller.prototype.removeAsset = function (asset) {
            var model = {
                campaignAssetID: asset.assetID
            };

            EventHub.publish("removeAutosaveContext", this.getContext(asset.assetID));

            this.assetResource.delete(model, this.deleteAssetFinished.bind(this, asset.assetID), this.saveFailed.bind(this, asset.assetID));
        };


        /**
         * Raises after successfully saving the campaign on the server. 
         * Changes state of the info box to let user know the saving was successful.
         * 
         * @param   response    response object returned from the server. Should contain saved asset object.
         */
        Controller.prototype.autosaveFinished = function (response) {
            var that = this;

            // Timeout is set to avoid too quickly changing of the info box state.
            // In the worst cases user would not spot any auto-save was performed.
            this.$timeout(function () {
                EventHub.publish("savingFinished", that.getContext(response.AssetID));
            }, 1000);
        };


        /**
         * Raises whenever the auto-saving failed. This is mostly due to avoiding client validation, in this cases it is not necessary to display error messages.
         * Changes state of the info box to let user know something is not right.
         * 
         * @param   response    response object returned from the server. Should contain saved asset object.
         */
        Controller.prototype.saveFailed = function (assetID) {
            var that = this;

            // Timeout is set to avoid too quickly changing of the info box state.
            // In the worst cases user would not spot any auto-save was performed.
            this.$timeout(function () {
                EventHub.publish("savingFailed", that.getContext(assetID));
            }, 1000);
        };


        /**
         * Raises after successfully adding new asset. 
         * Changes state of the info box to let user know the saving was successful.
         * 
         * @param   response    response object returned from the server. Should contain saved asset object.
         */
        Controller.prototype.addCampaignAssetFinished = function (response) {
            var that = this;

            var newAsset = {
                assetID: response.AssetID,
                conversionDisplayName: response.ConversionDisplayName,
                conversionValue: response.ConversionValue,
                id: response.ID,
                link: response.Link,
                name: response.Name,
                type: response.Type,
                campaignID: response.CampaignID,
                additionalProperties: response.AdditionalProperties
            };

            that.$scope.model.assets[response.AssetID] = newAsset;
            EventHub.publish("registerAutosaveContext", that.getContext(newAsset.assetID), that.autosave.bind(that));

            // Timeout is set to avoid too quickly changing of the info box state.
            // In the worst cases user would not spot any auto-save was performed.
            this.$timeout(function () {
                EventHub.publish("savingFinished", that.getContext(newAsset.id + newAsset.type));
            }, 1000);
        };


        /**
         * Raises when some error occurs while adding new asset.
         * 
         * @param   response    response object returned from the server. Should contain saved asset object.
         */
        Controller.prototype.addCampaignAssetFailed = function (assetID, response) {
            if (assetID) {
                EventHub.publish("hideInfo", assetID);
            }
            
            var message = this.$scope.resolveFilter("campaign.assets.failed");
            if (response.data && _.isString(response.data.Message)) {
                message = response.data.Message;
            }
            alert(message);
        };


        /**
         * Raises after successfully deleting asset. 
         * Changes state of the info box to let user know the saving was successful.
         * 
         * @param   assetID    ID of deleted asset.
         */
        Controller.prototype.deleteAssetFinished = function (assetID) {
            var that = this;

            delete that.$scope.model.assets[assetID];

            // Timeout is set to avoid too quickly changing of the info box state.
            // In the worst cases user would not spot any auto-save was performed.
            this.$timeout(function () {
                EventHub.publish("savingFinished", that.getContext(assetID));
            }, 1000);
        };


        /**
         * Returns context name for given asset id.
         *
         * @param   assetID    ID of asset.
         */
        Controller.prototype.getContext = function (assetID) {
            return "model.assets[" + assetID + "]";
        };

        return ['$scope', "$timeout", "cmsAssetResource", "resolveFilter", Controller];
    };
});