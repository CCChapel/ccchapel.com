using System;

using CMS.ExtendedControls.ActionsConfig;
using CMS.Helpers;
using CMS.Membership;
using CMS.PortalEngine;
using CMS.UIControls;
using CMS.OnlineMarketing;

[Security(Resource = "CMS.ABTest", UIElements = "ABTestListing")]
[UIElement("CMS.ABTest", "ABTestListing")]
public partial class CMSModules_OnlineMarketing_Pages_Content_ABTesting_ABTest_List : CMSABTestPage
{
    /// <summary>
    /// If true, the items are edited in dialog
    /// </summary>
    private bool EditInDialog
    {
        get
        {
            return listElem.Grid.EditInDialog;
        }
        set
        {
            listElem.Grid.EditInDialog = value;
        }
    }


    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        EditInDialog = QueryHelper.GetBoolean("editindialog", false);
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        // Set disabled module info
        ucDisabledModule.SettingsKeys = "CMSAnalyticsEnabled;CMSABTestingEnabled";
        ucDisabledModule.ParentPanel = pnlDisabled;

        InitHeaderActions();
        InitTitle();
        InitSmartTip();
    }


    /// <summary>
    /// Initializes header actions.
    /// </summary>
    private void InitHeaderActions()
    {
        if (MembershipContext.AuthenticatedUser.IsAuthorizedPerUIElement("CMS.ABTest", "New"))
        {
            string url = UIContextHelper.GetElementUrl("CMS.ABTest", "New", EditInDialog);

            // Get the alias path of the current node, if in content
            if (Node != null)
            {
                listElem.NodeID = Node.NodeID;
                listElem.ShowOriginalPageColumn = false;
                string aliasPath = Node.NodeAliasPath;
                listElem.AliasPath = aliasPath;

                url = URLHelper.AddParameterToUrl(url, "NodeID", Node.NodeID.ToString());
                url = URLHelper.AddParameterToUrl(url, "AliasPath", aliasPath);
            }

            url = ResolveUrl(url);

            // Set header action
            var action = new HeaderAction
            {
                ResourceName = "CMS.ABTest",
                Permission = "Manage",
                Text = GetString("abtesting.abtest.new"),
                RedirectUrl = url,
                OpenInDialog = EditInDialog
            };

            CurrentMaster.HeaderActions.AddAction(action);
        }
    }


    /// <summary>
    /// Sets title if not in content.
    /// </summary>
    private void InitTitle()
    {
        if (NodeID <= 0)
        {
            SetTitle(GetString("analytics_codename.abtests"));
        }
    }


    /// <summary>
    /// Init the smart tip with the how to video.
    /// Shows how to video.
    /// </summary>
    private void InitSmartTip()
    {      
        var linkBuilder = new MagnificPopupYouTubeLinkBuilder();
        var linkID = Guid.NewGuid().ToString();
        var link = linkBuilder.GetLink("2wU7rNzC95w", linkID, GetString("abtesting.howto.howtosetupabtest.link"));
             
        new MagnificPopupYouTubeJavaScriptRegistrator().RegisterMagnificPopupElement(this, linkID);

        smrtpHowToListing.Content = string.Format("<h4>{0}</h4>{1}<br />{2}", GetString("abtesting.howto.howtosetupabtest.title"), GetString("abtesting.howto.howtosetupabtest.text"), link);
        smrtpHowToListing.DismissedStateIdentifier = "howtovideo|abtest|listing";
    }
}