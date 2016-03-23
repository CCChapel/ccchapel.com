using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using CMS.Core;
using CMS.DocumentEngine;
using CMS.Globalization;
using CMS.Helpers;
using CMS.Helpers.Internal;
using CMS.Localization;
using CMS.Membership;
using CMS.Modules;
using CMS.SiteProvider;
using CMS.UIControls;
using CMS.WebAnalytics;
using CMS.PortalEngine;
using CMS.MacroEngine;
using CMS.ExtendedControls;
using CMS.DataEngine;


[EditedObject(CampaignInfo.OBJECT_TYPE, "objectid")]
[UIElement(ModuleName.WEBANALYTICS, "Campaign.General", false, true)]
public partial class CMSModules_WebAnalytics_Pages_Tools_Campaign_Tab_General : CMSCampaignPage
{
    private object DataFromServer
    {
        get
        {
            var campaign = new CampaignEditViewModel(GetCampaignInfo(), DateTime.Now);

            var assets = CampaignAssetInfoProvider.GetCampaignAssets()
                                                  .WhereEquals("CampaignAssetCampaignID", campaign.CampaignID)
                                                  .ToList()
                                                  .Select(x => Service.Entry<ICampaignAssetModelService>().GetStrategy(x.CampaignAssetType).GetAssetViewModel(x))
                                                  .ToDictionary(x => x.AssetID);

            return new
            {
                Resources = new Dictionary<string, string>
                {
                    {"campaign.displayname", GetString("campaign.displayname")},
                    {"campaign.description", GetString("campaign.description")},
                    {"campaign.totalcost", GetString("campaign.totalcost")},
                    {"campaign.displayname.description", GetString("campaign.displayname.description")},
                    {"campaign.description.description", GetString("campaign.description.description")},
                    {"campaign.totalcost.description", GetString("campaign.totalcost.description")},
                    {"campaign.initialSave.validationAlert", GetString("campaign.initialSave.validationAlert")},
                    {"autosave.pending", GetString("campaign.autosave.pending")},
                    {"autosave.finished", GetString("campaign.autosave.finished")},
                    {"autosave.failed", GetString("campaign.autosave.failed")},
                    {"autosave.validationFailed", GetString("campaign.autosave.validationFailed")},
                    {"general.requiresvalue", GetString("general.requiresvalue")},
                    {"general.select", GetString("general.select")},
                    {"general.tryagain", GetString("general.tryagain")},
                    {"campaign.assets", GetString("campaign.assets")},
                    {"campaign.assets.content", GetString("general.content")},
                    {"campaign.assets.content.add", GetString("campaign.assets.content.add")},
                    {"campaign.assets.content.description", GetString("campaign.assets.landingpage.description")},
                    {"campaign.assets.content.create.description", GetString("campaign.assets.content.create.description")},
                    {"campaign.assets.content.create", GetString("campaign.assets.content.create")},
                    {"campaign.assets.cms.form", GetString("general.bizform")},
                    {"campaign.assets.newsletter.issue", GetString("general.email")},
                    {"campaign.assets.email.add", GetString("campaign.assets.email.add")},
                    {"campaign.assets.email.description", GetString("campaign.assets.email.description")},
                    {"campaign.assets.failed", GetString("campaign.assets.failed")},
                    {"campaign.asset.newsletter.issue.alreadyexists", GetString("campaign.asset.newsletter.issue.alreadyexists")},
                    {"campaign.asset.cms.document.alreadyexists", GetString("campaign.asset.cms.document.alreadyexists")},
                    {"url.selectcontentpath", GetDocumentDialogUrl("campaignAssetLandingPage") },
                    {"url.selectformpath", GetModalDialogUrl("cms.form", "campaignAssetForm") },
                    {"url.selectemailpath", GetModalDialogUrl("newsletter.issue", "campaignAssetEmail") },
                    {"url.createpagepath", GetCreateNewPageDialogUrl()},
                    {"campaign.launch", GetString("campaign.launch")},
                    {"campaign.launch.confirm", GetString("campaign.launch.confirm")},
                    {"campaign.launch.confirm.delimiter", GetString("campaign.launch.confirm.delimiter")},
                    {"campaign.launch.confirm.summary", GetString("campaign.launch.confirm.summary")},
                    {"campaign.launched", GetString("campaign.launched")},
                    {"campaign.finish", GetString("campaign.finish")},
                    {"campaign.finish.confirm", GetString("campaign.finish.confirm")},
                    {"campaign.finished", GetString("campaign.finished")},
                    {"general.failed", GetString("general.failed")},
                    {"campaign.finished.failed", GetString("campaign.finished.failed")},
                    {"campaign.launched.failed", GetString("campaign.launched.failed")},
                    {"campaign.utmcampaign", GetString("campaign.utmcampaign")},
                    {"campaign.utmcampaign.description", GetString("campaign.utmcampaign.description")},
                    {"campaign.promotion", GetString("campaign.promotion")},
                    {"campaign.promotion.description", GetString("campaign.promotion.description")},
                    {"campaign.conversion", GetString("campaign.conversion")},
                    {"campaign.conversion.description", GetString("campaign.conversion.description")},
                    {"campaign.assetlist.email", GetString("campaign.assetlist.email")},
                    {"campaign.utmsource", GetString("campaign.utmsource")},
                    {"conversion.name", GetString("conversion.name")},
                    {"conversion.value", GetString("conversion.value")},
                    {"campaign.assets.cms.document", GetString("campaign.assets.pagevisit")},
                    {"campaign.assetlist.selectemail", GetString("campaign.assetlist.selectemail")},
                    {"campaign.assetlist.removeemail", GetString("campaign.assetlist.removeemail")},
                    {"campaign.assetlist.selectasset", GetString("campaign.assetlist.selectasset")},
                    {"campaign.assetlist.removeasset", GetString("campaign.assetlist.removeasset")},
                    {"campaign.launchchecklist", GetString("campaign.launchchecklist")},
                    {"campaign.launchchecklist.subscribers", GetString("campaign.launchchecklist.subscribers")},
                    {"campaign.launchchecklist.messages", GetString("campaign.launchchecklist.messages")},
                    {"campaign.launchchecklist.syncteam", GetString("campaign.launchchecklist.syncteam")},
                    {"campaign.launchchecklist.marketingautomation", GetString("campaign.launchchecklist.marketingautomation")},
                    {"campaign.launchchecklist.external", GetString("campaign.launchchecklist.external")},                    
                    {"campaign.launchchecklist.launchedstatus", GetString("campaign.launchchecklist.launchedstatus")},
                    {"campaign.launchchecklist.finishedstatus", GetString("campaign.launchchecklist.finishedstatus")},
                    {"campaign.create.email", GetString("campaign.create.email")},
                    {"campaign.create.email.description", GetString("campaign.create.email.description")},
                    {"campaign.create.email.subject", GetString("campaign.create.email.subject")},
                    {"newsletter_newsletter.select.selectitem", GetString("newsletter_newsletter.select.selectitem")},
                    {"campaign.create.email.select.type.assign", GetString("campaign.create.email.select.type.assign")},
                    {"campaign.create.email.select.type.new", GetString("campaign.create.email.select.type.new")},
                    {"newsletter_edit.newsletterdisplaynamelabel", GetString("newsletter_edit.newsletterdisplaynamelabel")},
                    {"newsletter_edit.newslettersendernamelabel", GetString("newsletter_edit.newslettersendernamelabel")},
                    {"newsletter_edit.newslettersenderemaillabel", GetString("newsletter_edit.newslettersenderemaillabel")},
                    {"campaign.create.email.campaign.senderaddress.invalid.email", GetString("campaign.create.email.campaign.senderaddress.invalid.email")},
                    {"newsletter_edit.subscriptiontemplate", GetString("newsletter_edit.subscriptiontemplate")},
                    {"newsletter_edit.unsubscriptiontemplate", GetString("newsletter_edit.unsubscriptiontemplate")},
                    {"newsletter_edit.newslettertemplate", GetString("newsletter_edit.newslettertemplate")},
                    {"campaign.utmmedium", GetString("campaign.utmmedium")},
                    {"campaign.getcontentlink.dialog.title", GetString("campaign.getcontentlink.dialog.title")},
                    {"campaign.getcontentlink.dialog.utmsource.explanationtext", GetString("campaign.getcontentlink.dialog.utmsource.explanationtext")},
                    {"campaign.getcontentlink.dialog.utmmedium.explanationtext", GetString("campaign.getcontentlink.dialog.utmmedium.explanationtext")},
                    {"campaign.getcontentlink.dialog.description", GetString("campaign.getcontentlink.dialog.description")},
                    {"campaign.getcontentlink.dialog.linktextarea.label", GetString("campaign.getcontentlink.dialog.linktextarea.label")},
                    {"campaign.getcontentlink.button.title", GetString("campaign.getcontentlink.button.title")},
                    {"campaign.getcontentlink.emptyutmcampaign.button.title", GetString("campaign.getcontentlink.emptyutmcampaign.button.title")},
                    {"campaign.getcontentlink.dialog.emptyutmsource", GetString("campaign.getcontentlink.dialog.emptyutmsource")},
                    {"campaign.utmparameter.wrongformat", GetString("campaign.utmparameter.wrongformat")},
                    {"general.maximize", GetString("general.maximize")},
                    {"general.restore", GetString("general.restore")},
                    {"general.close", GetString("general.close")},
                    {"general.create", GetString("general.create")},
                    {"general.loading", GetString("general.loading")},
                },
                Breadcrumbs = GetBreadcrumbsData(),
                Campaign = campaign,
                Assets = assets,
                EmailRegexp = ValidationHelper.EmailRegExp.ToString(),
                IsNewsletterModuleLoaded = ModuleEntryManager.IsModuleLoaded(ModuleName.NEWSLETTER)
            };
        }
    }


    /// <summary>
    /// Initializes selection modal dialog.
    /// </summary>
    /// <param name="objectType">Object type</param>
    /// <param name="cliendId">Client ID</param>
    private string GetModalDialogUrl(string objectType, string cliendId)
    {
        var guid = Guid.NewGuid().ToString();
        WindowHelper.Add(guid, new Hashtable
        {
            {"AllowAll", false},
            {"AllowEmpty", false},
            {"AllowDefault", true},
            {"LocalizeItems", true},
            {"ObjectType", objectType},
            {"SelectionMode", SelectionModeEnum.SingleButton},
            {"ResourcePrefix",objectType},
            {"FilterControl", "~/CMSFormControls/Filters/ObjectFilter.ascx"},
            {"CurrentSiteOnly", true}
        });

        var url = URLHelper.GetAbsoluteUrl("~/CMSAdminControls/UI/UniSelector/SelectionDialog.aspx");
        url = URLHelper.AddParameterToUrl(url, "clientId", cliendId);
        url = URLHelper.AddParameterToUrl(url, "params", guid);
        return URLHelper.AddParameterToUrl(url, "hash", QueryHelper.GetHash(url));
    }


    /// <summary>
    /// Prepares JSON object to be inserted to the breadcrumbs. This object will be used when updating breadcrumbs after changing display name of the campaign.
    /// </summary>
    /// <returns>List of objects containing breadcrumb for root element and single campaign.</returns>
    private object GetBreadcrumbsData()
    {
        var breadcrumbsList = new List<object>();
        var application = UIContext.UIElement.Application;

        // Root application
        string rootRedirectUrl = URLHelper.ResolveUrl(UIContextHelper.GetApplicationUrl(application));
        breadcrumbsList.Add(new
        {
            text = MacroResolver.Resolve(application.ElementDisplayName),
            redirectUrl = rootRedirectUrl,
            isRoot = true
        });

        // (Campaign)
        breadcrumbsList.Add(new
        {
            suffix = ResHelper.GetString("analytics.campaign")
        });


        return new
        {
            data = breadcrumbsList,
            pin = new
            {
                elementGuid = UIElementInfoProvider.GetUIElementInfo(UIContext.UIElement.ElementParentID).ElementGUID,
                applicationGuid = application.ElementGUID,
                objectType = CampaignInfo.OBJECT_TYPE
            }
        };
    }


    protected void Page_Init(object sender, EventArgs e)
    {
        ScriptHelper.RegisterTooltip(this);
        ScriptHelper.RegisterAngularModule("CMS.WebAnalytics/Module", DataFromServer, AngularVersionEnum.Version_1_4_5);
        ScriptHelper.RegisterDialogScript(this);
    }


    /// <summary>
    /// Returns correct URL of the select path dialog.
    /// </summary>
    /// <param name="textboxId">Client ID of the textbox where result should be returned.</param>
    private string GetDocumentDialogUrl(string textboxId)
    {
        var config = new DialogConfiguration
        {
            HideLibraries = true,
            HideAnchor = true,
            HideAttachments = true,
            HideContent = false,
            HideEmail = true,
            HideWeb = true,
            EditorClientID = textboxId,
            ContentSelectedSite = SiteContext.CurrentSiteName,
            OutputFormat = OutputFormatEnum.Custom,
            CustomFormatCode = "selectpath",
            SelectableContent = SelectableContentEnum.AllContent,
            SelectablePageTypes = SelectablePageTypeEnum.Standard,
            ContentSites = AvailableSitesEnum.OnlyCurrentSite
        };

        string url = CMSDialogHelper.GetDialogUrl(config, false, false, null, false);

        url = URLHelper.RemoveParameterFromUrl(url, "hash");
        url = URLHelper.AddParameterToUrl(url, "selectionmode", "single");
        url = URLHelper.AddParameterToUrl(url, "hash", QueryHelper.GetHash(url));

        return url;
    }


    /// <summary>
    /// Returns URL of modal dialog for creating new page.
    /// </summary>
    private string GetCreateNewPageDialogUrl()
    {
        var path = SettingsKeyInfoProvider.GetValue("CMSCampaignNewPageLocation", SiteContext.CurrentSiteName);
        var tree = new TreeProvider(MembershipContext.AuthenticatedUser);
        var node = tree.SelectSingleNode(SiteContext.CurrentSiteName, path, SiteContext.CurrentSite.DefaultVisitorCulture, true);

        if (node == null)
        {
            node = tree.SelectSingleNode(SiteContext.CurrentSiteName, "/", SiteContext.CurrentSite.DefaultVisitorCulture, true);
        }

        // General url settings
        var settings = new UIPageURLSettings
        {
            AllowSplitview = false,
            NodeID = node.NodeID,
            Culture = node.DocumentCulture,
            Action = "new",
            AdditionalQuery = "dialog=1&action=new&hidecontentonly=true"
        };

        return DocumentUIHelper.GetDocumentPageUrl(settings);
    }


    /// <summary>
    /// Gets the campaign info we are working with. Either edited or being newly created.
    /// </summary>
    private CampaignInfo GetCampaignInfo()
    {
        var campaignInfo = (EditedObject as CampaignInfo) ?? new CampaignInfo { CampaignSiteID = SiteContext.CurrentSiteID };

        // Add default display name for new campaign
        if (String.IsNullOrEmpty(campaignInfo.CampaignDisplayName))
        {
            var currentCulture = CultureHelper.GetCultureInfo(CultureCode);
            var currentSite = SiteContext.CurrentSite;
            var currentDateTimeFormatted = TimeZoneHelper.GetSiteDateTime(currentSite).ToString(currentCulture.DateTimeFormat.ShortDatePattern.Replace("'", ""));

            campaignInfo.CampaignDisplayName = String.Format("{0} – {1}", GetString("campaign.defaultname"), currentDateTimeFormatted);
        }

        // Add default UTM code for new campaign
        if (String.IsNullOrEmpty(campaignInfo.CampaignUTMCode))
        {
            campaignInfo.CampaignUTMCode = campaignInfo.Generalized.GetUniqueName(GetString("campaign.defaultutmcode"), campaignInfo.CampaignID, "CampaignUTMCode", "_{0}", "[_](\\d+)$", false);
        }

        return campaignInfo;
    }
}