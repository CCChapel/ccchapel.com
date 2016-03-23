cmsdefine([], function () {

    /**
     * Represents one campaign detail in campaign list.
     */
    var Controller = function ($scope, resolveFilter) {
        var STATUS_TO_CLASS = {
            'Launched': 'tag-active',
            'Scheduled': 'tag-scheduled',
            'Draft': 'tag-default',
            'Finished': 'tag-incomplete',
        },
            STATUS_TO_RESOURCE_STRING = {
                'Launched': 'campaigns.campaign.status.launched',
                'Scheduled': 'campaigns.campaign.status.scheduled',
                'Draft': 'campaigns.campaign.status.draft',
                'Finished': 'campaigns.campaign.status.finished',
            },
            DELETE_CONFIRMATION_RESOURCE_STRING = 'general.deleteconfirmation',
            campaign = $scope.campaign;

        if (campaign.openFrom && !(campaign.openFrom instanceof Date)) {
            // Since IE9 returns invalid date due presence of milliseconds in datetime, milliseconds must be omitted from string.
            campaign.openFrom = new Date(campaign.openFrom.split(".")[0]);
        }

        if (campaign.openTo && !(campaign.openTo instanceof Date)) {
            // Since IE9 returns invalid date due presence of milliseconds in datetime, milliseconds must be omitted from string.
            campaign.openTo = new Date(campaign.openTo.split(".")[0]);
        }

        this.$scope = $scope;
        this.$scope.model = {
            /**
             * Returns css class for campaigns status.
             */
            getStatusClass: function () {
                return STATUS_TO_CLASS[$scope.campaign.status];
            },

            /**
             * Returns resource string for campaigns status.
             */
            getStatusResourceString: function () {
                return STATUS_TO_RESOURCE_STRING[campaign.status];
            },

            /**
             * Calculates number of days for remaining to campaign start.
             */
            daysToRun: function days_between(date) {
                // The number of milliseconds in one day
                var ONE_DAY = 1000 * 60 * 60 * 24;
                return Math.round(Math.abs((date.getTime() - new Date()) / ONE_DAY));
            },

            /**
             * Emits event that campaign wants to be deleted.
             */
            deleteCampaignClick: function () {
                if (confirm(resolveFilter(DELETE_CONFIRMATION_RESOURCE_STRING))) {
                    $scope.$emit('deleteCampaign', campaign);
                }
            },
        };
    },
        directive = function () {
            return {
                restrict: 'A',
                templateUrl: 'CampaignDetailTemplate.html',
                controller: Controller,
                scope: {
                    campaign: '=',
                }
            };
        };

    Controller.$inject = [
      '$scope',
      'resolveFilter',
    ];

    return [directive];
});