using System;
using System.Linq;

using CMS.DocumentEngine;
using CMS.Helpers;
using CMS.Membership;
using CMS.PortalEngine;
using CMS.UIControls;
using CMS.ExtendedControls;
using CMS.DataEngine;
using CMS.WebAnalytics;
using CMS.Core;
using CMS.PortalEngine.Internal;

[SaveAction(0)]
public partial class CMSModules_Content_CMSDesk_OnlineMarketing_Settings_Default : CMSAnalyticsContentPage
{
    private const string CAMPAIGN_ELEMENT_CODENAME = "CampaignProperties";

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        DocumentManager.OnSaveData += DocumentManager_OnSaveData;
        DocumentManager.LocalDocumentPanel = pnlDoc;

        // Non-versioned data are modified
        DocumentManager.UseDocumentHelper = false;
        DocumentManager.HandleWorkflow = false;
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        // Check UI Analytics.Settings
        var ui = MembershipContext.AuthenticatedUser;
        if (!ui.IsAuthorizedPerUIElement("CMS.Content", "Analytics.Settings"))
        {
            RedirectToUIElementAccessDenied("CMS.Content", "Analytics.Settings");
        }
        EditedObject = Node;

        // Set disabled module info
        ucDisabledModule.SettingsKeys = "CMSAnalyticsEnabled;";
        ucDisabledModule.ParentPanel = pnlDisabled;

        ucConversionSelector.SelectionMode = SelectionModeEnum.SingleTextBox;
        ucConversionSelector.IsLiveSite = false;

        // Check modify permissions
        if (!DocumentUIHelper.CheckDocumentPermissions(Node, PermissionsEnum.Modify))
        {
            DocumentManager.DocumentInfo = String.Format(GetString("cmsdesk.notauthorizedtoeditdocument"), Node.NodeAliasPath);

            // Disable save button
            CurrentMaster.HeaderActions.Enabled = false;
        }

        if ((Node != null) && !URLHelper.IsPostback())
        {
            ReloadData();
        }
    }


    /// <summary>
    /// Reload data from node to controls.
    /// </summary>
    private void ReloadData()
    {
        ucConversionSelector.Value = Node.DocumentTrackConversionName;
        txtConversionValue.Value = Node.DocumentConversionValue;

        var campaignsListWithNameAndLink = CampaignInfoProvider.GetCampaigns()
                                                               .Source(s => s.Join<CampaignAssetInfo>("CampaignID", "CampaignAssetCampaignID"))
                                                               .WhereEquals("CampaignAssetAssetGuid", Node.NodeGUID)
                                                               .WhereEquals("CampaignAssetType", TreeNode.OBJECT_TYPE)
                                                               .Columns("CampaignDisplayName", "CampaignID")
                                                               .Select(campaign => new
                                                               {
                                                                   EncodedCampaignDisplayName = HTMLHelper.HTMLEncode(campaign.CampaignDisplayName),
                                                                   CampaignEditLink = URLHelper.GetAbsoluteUrl(Service.Entry<IUILinkProvider>().GetSingleObjectLink(CampaignInfo.TYPEINFO.ModuleName, CAMPAIGN_ELEMENT_CODENAME,
                                                                       new ObjectDetailLinkParameters
                                                                       {
                                                                           ObjectIdentifier = campaign.CampaignID,
                                                                           AllowNavigationToListing = true
                                                                       }))
                                                               });

        if (campaignsListWithNameAndLink.Any())
        {
            listViewCampaings.DataSource = campaignsListWithNameAndLink;
            listViewCampaings.DataBind();
            ucConversionSelector.Enabled = false;
            txtConversionValue.Enabled = false;
            smrtpEditInCampaign.Visible = true;
            smrtpEditInCampaign.Content = GetString("om.analyticSettings.editInCampain");
        }
        else
        {
            pnlTrackedCampaigns.Visible = false;
        }
    }


    protected void DocumentManager_OnSaveData(object sender, DocumentManagerEventArgs e)
    {
        if (Node != null)
        {
            string conversionName = ValidationHelper.GetString(ucConversionSelector.Value, String.Empty).Trim();

            if (!ucConversionSelector.IsValid())
            {
                e.ErrorMessage = ucConversionSelector.ValidationError;
                e.IsValid = false;
                return;
            }

            if (!txtConversionValue.IsValid())
            {
                e.ErrorMessage = GetString("conversionvalue.error");
                e.IsValid = false;
                return;
            }

            Node.DocumentConversionValue = txtConversionValue.Value.ToString();
            Node.DocumentTrackConversionName = conversionName;
        }
    }
}