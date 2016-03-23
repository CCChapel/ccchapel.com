cmsdefine(["CMS/EventHub", "Underscore", "CMS/Tabs", "CMS/UrlHelper", "jQuery"], function (EventHub, _, Tabs, UrlHelper, $) {

    return function (dataFromServer) {
        var Controller = function ($scope, $timeout, campaignResource, resolveFilter, messageHub) {
            this.$scope = $scope;
            this.$scope.model = {
                campaign: dataFromServer.campaign,
            };

            this.$scope.autosave = this.autosave.bind(this);
            this.$scope.launchCampaign = this.launchCampaign.bind(this);
            this.$scope.finishCampaign = this.finishCampaign.bind(this);

            this.breadcrumbs = dataFromServer.breadcrumbs;
            this.messageHub = messageHub;

            this.$timeout = $timeout;
            this.resolveFilter = resolveFilter;
            this.campaignResource = campaignResource;

            this.autosaveInProgress = false;
            this.autosaveQueued = false;

            this.isNewCampaign = !dataFromServer.campaign.campaignID;
            this.context = "model.campaign";

            // Autosave should be registered only once even though the control is registered twice on the page => we need to store a flag in shared object.
            if (!window.campaignEditControllerRegistered) {
                EventHub.publish("registerAutosaveContext", this.context, this.autosave.bind(this));
                window.campaignEditControllerRegistered = true;

                $scope.$on("saveCampaign", function () {
                    $scope.autosave();
                });
            }

            $scope.$on("autosaveForm", function (event, formController) {
                $scope.formController = formController;
            });

            // Collection of contexts being curretly in saving process
            this.itemsBeingSaved = {}
            // Is true if itemsBeingSaved contain some fields
            this.$scope.savingInProgress = false;

            var that = this;
            EventHub.subscribe("savingStarted", function (context) {
                that.itemsBeingSaved[context] = true;
                $scope.savingInProgress = true;
            });

            this.savingFinished = function (context) {
                delete that.itemsBeingSaved[context];
                $scope.savingInProgress = Object.getOwnPropertyNames(that.itemsBeingSaved).length > 0;
            };

            EventHub.subscribe("savingFinished", this.savingFinished);
            EventHub.subscribe("savingFailed", this.savingFinished);
        };


        /**
         * Performs auto-saving of the campaign object. If campaign has set campaign ID, update action is selected and the campaign model only updates
         * existing campaign on the server. If no ID is present, saving action is selected and new campaign is created on the server from the sent model.
         */
        Controller.prototype.autosave = function () {
            var method = this.getAutosaveMethod();
            EventHub.publish("savingStarted", this.context);
            method.call(this, this.$scope.model.campaign, this.autosaveFinished.bind(this), this.autosaveFailed.bind(this));
        };


        /**
         * Returns autosave method.
         */
        Controller.prototype.getAutosaveMethod = function () {
            if (this.$scope.model.campaign.campaignID === 0) {
                if (this.autosaveInProgress) {
                    this.autosaveQueued = true;
                    return function () { };
                }
                else {
                    this.autosaveInProgress = true;
                    return this.campaignResource.save;
                }
            }

            return this.campaignResource.update;
        };


        /**
         * Launches a campaign. Sends request to the server.
         */
        Controller.prototype.launchCampaign = function () {
            var that = this,
                errorFunction = function () {
                    that.messageHub.publishError(that.resolveFilter("campaign.launched.failed"));
                };


            this.campaignResource.launchLog({ id: this.$scope.model.campaign.campaignID },
                function (logResponse) {
                    var confirmMessage = that.resolveFilter('campaign.launch.confirm');
                        
                    if (logResponse.Messages.length > 0) {
                        confirmMessage += '\n' +
                            that.resolveFilter('campaign.launch.confirm.delimiter') + '\n' +
                            that.resolveFilter('campaign.launch.confirm.summary');
                            confirmMessage += '\n' +logResponse.Messages.join('\n');
                    }

                    if (!confirm(confirmMessage))
                        return;

                    that.messageHub.publishClear();
                    that.campaignResource.launch({ id: that.$scope.model.campaign.campaignID }, function (response) {
                        that.$scope.model.campaign.enabled = response.Enabled;
                        that.$scope.model.campaign.status = response.Status;

                        that.messageHub.publishSuccess(that.resolveFilter("campaign.launched"));
                    }, errorFunction);
                }, errorFunction);
        };


        /**
         * Finishes a campaign. Sends request to the server.
         */
        Controller.prototype.finishCampaign = function () {
            if (confirm(this.resolveFilter('campaign.finish.confirm'))) {
                var that = this;

                this.messageHub.publishClear();
                this.campaignResource.finish({ id: this.$scope.model.campaign.campaignID },
                    function (response) {
                        that.$scope.model.campaign.status = response.Status;

                        that.messageHub.publishSuccess(that.resolveFilter("campaign.finished"));
                    }, function () {
                        that.messageHub.publishError(that.resolveFilter("campaign.finished.failed"));
                    });
            }
        };


        /**
         * Handles autosave status and launches new autosave if queued.
         */
        Controller.prototype.handleAutosaveStatus = function () {
            this.autosaveInProgress = false;

            if (this.autosaveQueued) {
                this.autosaveQueued = false;
                this.autosave();
            }
        };


        /**
         * Raises after successfully saving the campaign on the server. 
         * Changes state of the info box to let user know the saving was successful.
         * Sets up campaign ID form the response, so any further changes triggers update of existing campaign instead of creating new one.
         * 
         * @param   response    response object returned from the server. Should contain saved campaign object.
         */
        Controller.prototype.autosaveFinished = function (response) {
            var that = this;

            // Timeout is set to avoid too quickly changing of the info box state.
            // In the worst cases user would not spot any auto-save was performed.
            this.$timeout(function () {
                // Modify tab links when saved for the first time
                if (that.$scope.model.campaign.campaignID === 0) {
                    that.updateTabLinks(response.CampaignID);
                }

                that.$scope.model.campaign.campaignID = response.CampaignID;
                EventHub.publish("savingFinished", that.context);
                that.updateBreadcrumbs(response);
                that.handleAutosaveStatus();

                that.isNewCampaign = false;
            }, 1000);
        };


        /**
         * Raises whenever the auto-saving failed. This is mostly due to avoiding client validation, in this cases it is not necessary to display error messages.
         * Changes state of the info box to let user know something is not right.
         */
        Controller.prototype.autosaveFailed = function (response) {
            var that = this;

            if (response.status === 400) {
                var model = that.$scope.formController["descriptionUTMCampaign"];
                model.$setValidity("serverValidation", false);
                model.serverValidationMessage = response.data.ModelState["model.UTMCode"][0];

                EventHub.publish("savingFailed", that.context, that.resolveFilter("autosave.validationFailed"));
                return;
            }

            // Timeout is set to avoid too quickly changing of the info box state.
            // In the worst cases user would not spot any auto-save was performed.
            this.$timeout(function () {
                EventHub.publish("savingFailed", that.context);
                that.handleAutosaveStatus();
            }, 1000);
        };


        /**
         * Publishes event for manually updating the breadcrumbs to match name of the campaign given in parameter.
         *
         * @param   campaign    updated campaign
         */
        Controller.prototype.updateBreadcrumbs = function (campaign) {
            var breadcrumbs = this.breadcrumbs;
            breadcrumbs.data[1].text = _.escape(campaign.DisplayName);

            if (this.isNewCampaign) {
                breadcrumbs.pin.objectName = campaign.CodeName;
                breadcrumbs.pin.objectSiteName = campaign.SiteName;
                breadcrumbs.pin.isPinned = false;
            } else {
                breadcrumbs.pin = null;
            }

            EventHub.publish("OverwriteBreadcrumbs", breadcrumbs);
        };


        /**
         * Manually updates the tab links to include the campaign ID.
         *
         * @param   campaignId    campaign ID
         */
        Controller.prototype.updateTabLinks = function (campaignId) {
            // Get ID of the tabs element
            var tabsId = UrlHelper.getParameter(UrlHelper.getQueryString(window.location.href), "tabselemid"),
            tabs = new Tabs(),
            element = $("#" + tabsId, window.parent.document);

            tabs.ensureQueryParamForTabs(element, "campaignid", campaignId);
            tabs.ensureQueryParamForTabs(element, "objectid", campaignId);
        };

        return ['$scope', "$timeout", "cmsCampaignResource", "resolveFilter", "messageHub", Controller];
    };
});