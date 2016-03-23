cmsdefine(['Underscore'], function (_) {
    return function () {
        return ['$scope', 'cmsCampaignResource', 'serverData', function ($scope, campaignResource, serverData) {
            $scope.model = {
                campaigns: serverData.campaigns.map(function(campaign) {
                    // Ensure Draft campaigns will not intervene sorting by visitors (since there cannot be any visitor)
                    if (campaign.status === "Draft") {
                        campaign.visitors = -1;
                    }
                    
                    return campaign;
                }),
                newCampaignClick: function () {
                    top.location = serverData.newCampaignLink;
                }
            };

            $scope.$watch("statusFilter", function(newValue) {
                if ((newValue === "draft") && ($scope.sortBy === "-visitors")) {
                    $scope.sortBy = "displayName";
                }
                
                if ((newValue !== "") && ($scope.sortBy === "")) {
                    $scope.sortBy = "displayName";
                }
            });

            $scope.$on('deleteCampaign', function (event, campaign) {
                campaignResource.delete({ id: campaign.campaignID });

                $scope.model.campaigns = _.reject($scope.model.campaigns, function (item) {
                    return item.campaignID === campaign.campaignID;
                });
            });
        }];
    };
});