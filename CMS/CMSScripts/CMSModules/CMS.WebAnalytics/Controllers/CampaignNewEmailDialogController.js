cmsdefine(['Underscore', 'CMS/EventHub', 'CMS.WebAnalytics/ModalDialog'], function (_, EventHub, ModalDialog) {
    return function () {
        var Controller = function($scope, $timeout, $q, campaignResource, newsletterResource, serverData, resolveFilter) {
            this.$scope = $scope;
            this.$scope.EMAIL_REGEXP = serverData.emailRegexp;
            this.$timeout = $timeout;
            this.$q = $q;
            this.resolveFilter = resolveFilter;
            this.campaignResource = campaignResource;
            this.newsletterResource = newsletterResource;

            $scope.model = {
                campaign: serverData.campaign,
            };
            $scope.data = serverData;
            $scope.data.emailCampaigns = [];
            $scope.data.emailTemplates = {};

            EventHub.subscribe('CampaignAssetCreateNewEmail.CreateEmail', this.onCreateEmail.bind(this));
        };


        Controller.prototype.getPromotionChannels = function (items) {
            return _.where(items, { type: "newsletter.issue" });
        };

        Controller.prototype.hasPromotionChannel = function (items) {
            return _.where(items, { type: "newsletter.issue" }).length;
        };

        Controller.prototype.onCreateEmail = function(assets) {
            var that = this;

            this.$scope.data.assets = assets;
            this.$scope.emailCampaignType = this.hasPromotionChannel(assets) ? 'assign' : 'new';

            if (!this.dialog) {
                this.dialog = new ModalDialog("#create-email-dialog-container", {
                    title: this.resolveFilter('campaign.create.email'),
                    width: 800,
                    height: 530,
                    buttons: [
                        {
                            title: this.resolveFilter('general.create'),
                            action: this.onDialogButtonAction.bind(this)
                        }
                    ],
                    resourceStrings: {
                        maximize: this.resolveFilter('general.maximize'),
                        restore: this.resolveFilter('general.restore'),
                        close: this.resolveFilter('general.close'),
                    }
                });
            }

            var campaignsPromise = this.newsletterResource.all().$promise.then(function (data) {
                // Convert to select option forman
                return _.map(data, function(newsletter) {
                    return {
                        id: newsletter.id,
                        name: newsletter.displayName,
                    };
                });
            }).then(function (options) {
                // Create hints for user so he can see used email campaigns for given campaign
                return that.newsletterResource.issues({ issueIds: _.pluck(that.getPromotionChannels(that.$scope.data.assets), 'id') })
                    .$promise
                    .then(function(issueModels) {
                        if (!issueModels || !issueModels.length) {
                            return options;
                        }

                        var newsletters = _.pluck(issueModels, 'newsletterId');

                        var hints = options.filter(function (option) {
                            return newsletters.indexOf(option.id) >= 0;
                        });

                        if (!hints || !hints.length) {
                            return options;
                        }

                        hints.push({
                            id: 0,
                            name: '------',
                            disabled: true,
                        });

                        return hints.concat(options);
                    });
            }).then(function (options) {
                // Assing hints to html select directive
                that.$scope.data.emailCampaigns = options;
            });

            var templatePromise = this.newsletterResource.templates(function (data) {
                // Convert templates into select formant by type
                var templates = data.map(function (template) {
                    return {
                        id: template.id,
                        name: template.displayName,
                        type: template.type.toLowerCase(),
                    };
                });
                that.$scope.data.emailTemplates = _.groupBy(templates, 'type');
            }).$promise;

            // Show loader screen and hide it immediately alfter all data are retrieved from server.
            window.top.Loader.show();
            this.$q.all([campaignsPromise, templatePromise]).catch(function (error) {
                EventHub.publish('CampaignAssetCreateNewEmail.EmailCreatedError', error);
                that.dialog.close();
            }).finally(function() {
                window.top.Loader.hide();
            });

            this.dialog.open();
        };


        Controller.prototype.onDialogButtonAction = function () {
            var that = this;

            if (this.$scope.newEmailForm.$invalid) {
                // Makes newEmailForm act as submitted. Shows all error messasges and so on.
                this.$timeout(function() {
                    that.$scope.newEmailForm.$setSubmitted();
                });

                return;
            }

            var creationType = this.$scope.emailCampaignType;
            var selectedNewsletter = this.$scope.emailCampaignSelect;

            var emailToCreate = {
                subject: this.$scope.emailSubject,
                displayName: this.$scope.emailDisplayName,
                senderName: this.$scope.emailSenderName,
                senderEmail: this.$scope.emailSenderAddress,
                templates: {
                    issue: this.$scope.templateIssue,
                    subscription: this.$scope.templateSubscription,
                    unsubscription : this.$scope.templateUnsubscription
                },
            };

            var issueModelPromise;
            if (creationType === 'new') {
                // Create static email campaign and then email
                issueModelPromise = this.newsletterResource
                                        .createStaticNewsletter(emailToCreate)
                                        .$promise
                                        .then(function(newsletterModel) {
                                            return that.createNewIssuePromise(newsletterModel.id, emailToCreate.subject);
                                        });
            } else {
                // Create email only
                issueModelPromise = this.createNewIssuePromise(selectedNewsletter, emailToCreate.subject);
            }

            issueModelPromise.then(function (issueModel) {
                EventHub.publish('CampaignAssetCreateNewEmail.EmailCreated', issueModel.id);
            }).catch(function(error) {
                EventHub.publish('CampaignAssetCreateNewEmail.EmailCreatedError', error);
            }).finally(function() {
                that.dialog.close();
            });
        };


        Controller.prototype.createNewIssuePromise = function (id, subject) {
            return this.newsletterResource.createNewIssue( { newsletterId: id },'"' + subject + '"').$promise;
        }

        return ['$scope', '$timeout', '$q', 'cmsCampaignResource', 'cmsNewsletterResource', 'serverData', 'resolveFilter', Controller];
    };
});