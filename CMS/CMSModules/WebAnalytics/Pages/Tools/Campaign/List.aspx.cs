using System;
using System.Collections.Generic;
using System.Linq;

using CMS.Core;
using CMS.Helpers;
using CMS.Helpers.Internal;
using CMS.PortalEngine.Internal;
using CMS.SiteProvider;
using CMS.UIControls;
using CMS.WebAnalytics;
using CMS.PortalEngine;
using CMS.WebAnalytics.Internal;

[UIElement(ModuleName.WEBANALYTICS, "Campaigns")]
[Security(Resource = ModuleName.WEBANALYTICS, ResourceSite = true, Permission = "ManageCampaigns")]
public partial class CMSModules_WebAnalytics_Pages_Tools_Campaign_List : CMSCampaignPage
{
    private const string CAMPAIGN_ELEMENT_CODENAME = "CampaignProperties";
    private const string ANALYTICS_CAMPAIGNS_TABNAME = "analytics.campaigns.campaigns";

    private object DataFromServer
    {
        get
        {
            return new
            {
                Resources = new Dictionary<string, string>
                {
                    {"general.deleteconfirmation", GetString("general.deleteconfirmation")},
                    {"campaigns.list.campaign.new", GetString("campaigns.list.campaign.new")},
                    {"campaigns.list.campaign.edit", GetString("campaigns.list.campaign.edit")},
                    {"campaigns.list.campaign.delete", GetString("campaigns.list.campaign.delete")},
                    {"campaigns.campaign.status.launched", GetString("campaigns.campaign.status.launched")},
                    {"campaigns.campaign.status.scheduled", GetString("campaigns.campaign.status.scheduled")},
                    {"campaigns.campaign.status.draft", GetString("campaigns.campaign.status.draft")},
                    {"campaigns.campaign.status.finished", GetString("campaigns.campaign.status.finished")},
                    {"campaign.visitors", GetString("campaign.visitors")},
                    {"campaign.status", GetString("general.status")},
                    {"campaign.name", GetString("general.name")},
                    {"campaign.filterby", GetString("campaign.filterby")},
                    {"campaign.sortby", GetString("campaign.sortby")},
                    {"campaign.filterby.placeholder", GetString("campaign.filterby.placeholder")},
                    {"campaign.list.conversions", GetString("campaign.list.conversions")},
                    {"campaign.conversionrate", GetString("campaign.conversionrate")},
                    {"campaigns.campaign.scheduled.info", GetString("campaigns.campaign.scheduled.info")},
                    {"campaigns.campaign.draft.info", GetString("campaigns.campaign.draft.info")},
                    {"campaign.codename", GetString("campaign.codename")},
                    {"general.all", GetString("general.all")},
                },
                Campaigns = GetCampaigns(),
                NewCampaignLink = GetCreateCampaignLink()
            };
        }
    }


    protected override void OnPreRender(EventArgs e)
    {
        ScriptHelper.RegisterAngularModule("CMS.WebAnalytics/Module", DataFromServer, AngularVersionEnum.Version_1_4_5);

        base.OnPreRender(e);

        CurrentMaster.PanelContent.CssClass = "";
    }


    private static IList<CampaignListItemViewModel> GetCampaigns()
    {
        var now = DateTime.Now;
        return CampaignInfoProvider.GetCampaigns()
                                   .OnSite(SiteContext.CurrentSiteID)
                                   .ToList()
                                   .OrderBy(campaign => (int)campaign.GetCampaignStatus(now))
                                   .ThenBy(campaign => campaign.CampaignDisplayName)
                                   .Select(campaign => CreateCampaignViewModel(campaign, now))
                                   .ToList();
    }


    private static CampaignListItemViewModel CreateCampaignViewModel(CampaignInfo campaign, DateTime now)
    {
        return Service<ICampaignListItemViewModelService>.Entry().GetModel(campaign, now);
    }


    private static string GetCreateCampaignLink()
    {
        return URLHelper.GetAbsoluteUrl(Service.Entry<IUILinkProvider>().GetSingleObjectLink(CampaignInfo.TYPEINFO.ModuleName, CAMPAIGN_ELEMENT_CODENAME, new ObjectDetailLinkParameters
        {
            ParentTabName = ANALYTICS_CAMPAIGNS_TABNAME,
            AllowNavigationToListing = true,
        }));
    }
}