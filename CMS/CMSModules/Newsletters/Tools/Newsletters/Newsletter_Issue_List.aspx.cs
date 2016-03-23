using System;
using System.Linq;
using System.Data;
using System.Collections.Generic;
using System.Web.UI.WebControls;

using CMS.ExtendedControls.ActionsConfig;
using CMS.Helpers;
using CMS.Membership;
using CMS.Newsletters;
using CMS.Base;
using CMS.PortalEngine;
using CMS.SiteProvider;
using CMS.UIControls;
using CMS.MacroEngine;
using CMS.DataEngine;


/// <summary>
/// Displays a list of issues for a specified newsletter.
/// </summary>
[EditedObject(NewsletterInfo.OBJECT_TYPE, "parentobjectid")]
[UIElement("CMS.Newsletter", "Newsletter.Issues")]
public partial class CMSModules_Newsletters_Tools_Newsletters_Newsletter_Issue_List : CMSNewsletterPage
{
    #region "Constants"

    const string ZERO_PERCENT = "0%";

    #endregion


    #region "Variables"

    private bool mBounceMonitoringEnabled;
    private bool mTrackingEnabled;
    private NewsletterInfo mNewsletter;
    private DataSet mClickedLinksSummary;
    private DataSet mVariantIssueSummaries;
    private DataSet mVariantIssues;
    
    #endregion


    #region "Methods"

    protected void Page_Load(object sender, EventArgs e)
    {
        mNewsletter = EditedObject as NewsletterInfo;

        if (mNewsletter == null)
        {
            RedirectToAccessDenied(GetString("general.invalidparameters"));
        }

        if (!mNewsletter.CheckPermissions(PermissionsEnum.Read, CurrentSiteName, CurrentUser))
        {
            RedirectToAccessDenied(mNewsletter.TypeInfo.ModuleName, "Read");
        }

        mBounceMonitoringEnabled = NewsletterHelper.MonitorBouncedEmails(CurrentSiteName);
        mTrackingEnabled = NewsletterHelper.IsTrackingAvailable();

        if (mTrackingEnabled && !mBounceMonitoringEnabled)
        {
            ShowInformation(GetString("newsletter.viewadditionalstatsmessage"));
        }

        ScriptHelper.RegisterTooltip(this);

        // Initialize unigrid
        UniGrid.WhereCondition = String.Format("IssueNewsletterID={0} AND IssueVariantOfIssueID IS NULL", mNewsletter.NewsletterID);
        UniGrid.ZeroRowsText = GetString("Newsletter_Issue_List.NoIssuesFound");
        UniGrid.OnAction += uniGrid_OnAction;
        UniGrid.OnBeforeDataReload += UniGrid_OnBeforeDataReload;
        UniGrid.OnExternalDataBound += UniGrid_OnExternalDataBound;

        // Initialize header actions
        InitHeaderActions();

        // Prepare data for listing
        // Group clicked link records by IssueID with Columns LinkIssueID, UniqueUserClicks, VariantParent (to calculate clicked links through all variant)
        mClickedLinksSummary = ClickedLinkInfoProvider.GetClickedLinks()
            .Columns(new QueryColumn("LinkIssueID"),
                     new AggregatedColumn(AggregationType.Count, "DISTINCT(ClickedLinkEmail)").As("UniqueUserClicks"))
            .Source(s => s.Join<LinkInfo>("ClickedLinkNewsletterLinkID", "LinkID"))
            .GroupBy("LinkIssueID");

        // Prepare variant summaries
        mVariantIssueSummaries = IssueInfoProvider.GetIssues()
            .Columns(new QueryColumn("IssueVariantOfIssueID"),
                     new AggregatedColumn(AggregationType.Sum, "IssueOpenedEmails").As("OpenedEmailsSum"))
            .WhereEquals("IssueNewsletterID", mNewsletter.NewsletterID)
            .GroupBy("IssueVariantOfIssueID")
            .Having("IssueVariantOfIssueID IS NOT NULL");
        
        // AB Variant issues for current newsletter
        mVariantIssues = IssueInfoProvider.GetIssues()
            .Columns("IssueID", "IssueVariantOfIssueID")
            .WhereEquals("IssueNewsletterID", mNewsletter.NewsletterID)
            .WhereNotNull("IssueVariantOfIssueID");
    }


    protected void InitHeaderActions()
    {
        if (!mNewsletter.NewsletterType.EqualsCSafe(NewsletterType.Dynamic))
        {
            CurrentMaster.HeaderActions.AddAction(new HeaderAction
                {
                    RedirectUrl = "Newsletter_Issue_New.aspx?parentobjectid=" + mNewsletter.NewsletterID,
                    Text = GetString("Newsletter_Issue_List.NewItemCaption"),
                    Tooltip = GetString("Newsletter_Issue_List.NewItemCaption"),
                    Permission = "AuthorIssues",
                    ResourceName = mNewsletter.TypeInfo.ModuleName,
                });
        }
    }


    protected void UniGrid_OnBeforeDataReload()
    {
        // Hide opened/clicked emails if tracking is not available
        UniGrid.NamedColumns["openedemails"].Visible = mTrackingEnabled;
        UniGrid.NamedColumns["issueclickedlinks"].Visible = mTrackingEnabled;

        // Hide bounced emails info if monitoring disabled or tracking is not available
        UniGrid.NamedColumns["deliveryrate"].Visible = mBounceMonitoringEnabled;
    }


    /// <summary>
    /// Handles the UniGrid's OnExternalDataBound event.
    /// </summary>
    /// <param name="sender">The sender</param>
    /// <param name="sourceName">Name of the source</param>
    /// <param name="parameter">The data row</param>
    /// <returns>Formatted value to be used in the UniGrid</returns>
    protected object UniGrid_OnExternalDataBound(object sender, string sourceName, object parameter)
    {
        string tooltipText = null;
        var webControl = sender as WebControl;
        
        // Prepare a tooltip for the column
        switch (sourceName.ToLowerCSafe())
        {
            case "issueopenedemails":
                tooltipText = GetString(mBounceMonitoringEnabled ? "newsletter.openratetooltip.delivered" : "newsletter.openratetooltip.sent");
                break;

            case "issueclickedlinks":
                tooltipText = GetString(mBounceMonitoringEnabled ? "newsletter.clickratetooltip.delivered" : "newsletter.clickratetooltip.sent");
                break;

            case "deliveryrate":
                tooltipText = GetString("newsletter.deliveryratetooltip");
                break;

            case "unsubscriberate":
                tooltipText = GetString(mBounceMonitoringEnabled ? "newsletter.unsubscriptionratetooltip.delivered" : "newsletter.unsubscriptionratetooltip.sent");
                break;

            default:
                break;
        }

        // If the sender is from a column with a tooltip, append tooltip to the control
        if ((webControl != null) && !String.IsNullOrEmpty(tooltipText))
        {
            ScriptHelper.AppendTooltip(webControl, tooltipText, null);
        }
        
        switch (sourceName.ToLowerCSafe())
        {
            case "issuesubject":
                return GetIssueSubject(parameter as DataRowView);

            case "issuestatus":
                IssueStatusEnum status = EnumHelper.GetDefaultValue<IssueStatusEnum>();
                var statusID = ValidationHelper.GetInteger(parameter, -1);

                if (Enum.IsDefined(typeof(IssueStatusEnum), statusID))
                {
                    status = (IssueStatusEnum)statusID;
                }

                return IssueHelper.GetStatusFriendlyName(status, null);

            case "issuesentemails":
                var num = ValidationHelper.GetInteger(parameter, 0);
                return (num > 0) ? num.ToString() : String.Empty;

            case "issueopenedemails":                
                return GetOpenedEmails(parameter as DataRowView);

            case "issueclickedlinks":               
                return GetClickRate(parameter as DataRowView);

            case "deliveryrate":
                return GetDeliveryRate(parameter as DataRowView);

            case "unsubscriberate":
                return GetUnsubscriptionRate(parameter as DataRowView);

            default:
                return parameter;
        }
    }


    /// <summary>
    /// Handles the UniGrid's OnAction event.
    /// </summary>
    /// <param name="actionName">Name of item (button) that throws event</param>
    /// <param name="actionArgument">ID (value of Primary key) of corresponding data row</param>
    protected void uniGrid_OnAction(string actionName, object actionArgument)
    {
        switch (actionName)
        {
            case "edit":
                string url = UIContextHelper.GetElementUrl("cms.newsletter", "EditIssueProperties", false, actionArgument.ToInteger(0));
                url = URLHelper.AddParameterToUrl(url, "parentobjectid", Convert.ToString(mNewsletter.NewsletterID));
                URLHelper.Redirect(url);
                break;

            case "delete":
                DeleteIssue(ValidationHelper.GetInteger(actionArgument, 0));
                break;
        }
    }
    

    /// <summary>
    /// Returns a subject of an issue. A/B test icon is added to the subject if the issue is an A/B test.
    /// </summary>
    /// <param name="rowView">A <see cref="DataRowView" /> that represents one row from UniGrid's source</param>
    private string GetIssueSubject(DataRowView rowView)
    {
        var isABTest = DataHelper.GetBoolValue(rowView.Row, "IssueIsABTest");
        var subject = HTMLHelper.HTMLEncode(MacroSecurityProcessor.RemoveSecurityParameters(DataHelper.GetStringValue(rowView.Row, "IssueSubject"), true, null));

        // Add the icon for A/B tests
        if (isABTest)
        {
            subject += UIHelper.GetAccessibleIconTag("NodeLink icon-two-squares-line tn", GetString("unigrid.newsletter_issue.abtesticontooltip"));
        }

        return subject;
    }


    /// <summary>
    /// Gets a clickable open rate link based on the values from datasource.
    /// </summary>
    /// <param name="rowView">A <see cref="DataRowView" /> that represents one row from UniGrid's source</param>
    /// <returns>A link with detailed statistics about opened emails</returns>
    private string GetOpenedEmails(DataRowView rowView)
    {
        var issueRow = rowView.Row;

        var issueSentEmails = DataHelper.GetIntValue(issueRow, "IssueSentEmails");
        if (issueSentEmails == 0)
        {
            return String.Empty;
        }

        // Get issue ID
        int issueId = DataHelper.GetIntValue(issueRow, "IssueID");

        // Get opened emails count from issue record
        int openedEmails = DataHelper.GetIntValue(issueRow, "IssueOpenedEmails");
        
        // Add winner variant data if it is an A/B test and a winner has been selected
        if (DataHelper.GetBoolValue(issueRow, "IssueIsABTest"))
        {
            var row = mVariantIssueSummaries.Tables[0].Select(string.Format("IssueVariantOfIssueID={0}", issueId)).FirstOrDefault();
            if(row != null)
            { 
                int variantOpensSum = DataHelper.GetIntValue(row, "OpenedEmailsSum");
                openedEmails += variantOpensSum;
            }
        }

        var delivered = GetDeliveredCount(rowView);

        if ((openedEmails > 0) && (delivered > 0))
        {
            return string.Format("{0:F2}%", ((double)openedEmails / delivered) * 100);
        }

        return ZERO_PERCENT;
    }


    /// <summary>
    /// Gets a clickable click rate link based on the values from datasource.
    /// </summary>
    /// <param name="rowView">A <see cref="DataRowView" /> that represents one row from UniGrid's source</param>
    /// <returns>A link with detailed statistics clicks</returns>
    private string GetClickRate(DataRowView rowView)
    {
        var issueSentEmails = DataHelper.GetIntValue(rowView.Row, "IssueSentEmails");
        if (issueSentEmails == 0)
        {
            return String.Empty;
        }

        if (DataHelper.DataSourceIsEmpty(mClickedLinksSummary))
        {
            return ZERO_PERCENT;
        }

        var issueId = DataHelper.GetIntValue(rowView.Row, "IssueID");

        // All issue ids (main issue and AB variants) to sum up click count
        var allIssueIds = new List<int> { issueId };
        // Get variants for current issue and add them to issue id list
        var variantIds = mVariantIssues.Tables[0].Select(string.Format("IssueVariantOfIssueID={0}", issueId));
        allIssueIds.AddRange(variantIds.Select(variantRow => DataHelper.GetIntValue(variantRow, "IssueID")));

        // Get clicked links summary rows for main issue and AB variant issues
        var clickedLinks = mClickedLinksSummary.Tables[0].Select(string.Format("LinkIssueID IN ({0})", TextHelper.Join(",", allIssueIds)));

        var delivered = GetDeliveredCount(rowView);

        if ((clickedLinks.Length > 0) && (delivered > 0))
        {
            // Sum up unique clicks for the base issue and also the AB variants
            var uniqueClicks = clickedLinks.Sum(dataRow => ValidationHelper.GetInteger(DataHelper.GetDataRowValue(dataRow, "UniqueUserClicks"), 0));
            return string.Format("{0:F2}%", ((double)uniqueClicks / delivered) * 100);
        }

        return ZERO_PERCENT;
    }


    /// <summary>
    /// Returns delivery rate based on the values from datasource.
    /// </summary>
    /// <param name="rowView">A <see cref="DataRowView" /> that represents one row from UniGrid's source</param>
    private string GetDeliveryRate(DataRowView rowView)
    {
        var sent = DataHelper.GetIntValue(rowView.Row, "IssueSentEmails");
        var delivered = (double)GetDeliveredCount(rowView);

        if (sent == 0)
        {
            return String.Empty;
        }

        return (delivered > 0) ? string.Format("{0:F2}%", (delivered / sent) * 100) : ZERO_PERCENT;
    }


    /// <summary>
    /// Returns unsubscription rate based on the values from datasource.
    /// </summary>
    /// <param name="rowView">A <see cref="DataRowView" /> that represents one row from UniGrid's source</param>
    private string GetUnsubscriptionRate(DataRowView rowView)
    {
        var issueSentEmails = DataHelper.GetIntValue(rowView.Row, "IssueSentEmails");
        if (issueSentEmails == 0)
        {
            return String.Empty;
        }

        var unsubscribed = DataHelper.GetIntValue(rowView.Row, "IssueUnsubscribed");
        if (unsubscribed > 0)
        {
            return string.Format("{0:F2}%", ((double)unsubscribed / GetDeliveredCount(rowView)) * 100);
        }

        return ZERO_PERCENT;
    }

    /// <summary>
    /// Returns delivered emails count based on the values from datasource. Sent emails count is returned if the bounce monitoring is disabled.
    /// </summary>
    /// <param name="rowView">A <see cref="DataRowView" /> that represents one row from UniGrid's source</param>
    private int GetDeliveredCount(DataRowView rowView)
    {
        var sent = DataHelper.GetIntValue(rowView.Row, "IssueSentEmails");

        if (!mBounceMonitoringEnabled)
        {
            return sent;
        }

        var bounces = DataHelper.GetIntValue(rowView.Row, "IssueBounces");
        return sent - bounces;
    }


    /// <summary>
    /// Deletes an issue specified by its ID (if authorized).
    /// </summary>
    /// <param name="issueId">Issue's ID</param>
    private static void DeleteIssue(int issueId)
    {
        var issue = IssueInfoProvider.GetIssueInfo(issueId);

        if (issue == null)
        {
            RedirectToAccessDenied(GetString("general.invalidparameters"));
        }

        // User has to have both destroy and issue privileges to be able to delete the issue.
        if (!issue.CheckPermissions(PermissionsEnum.Delete, SiteContext.CurrentSiteName, MembershipContext.AuthenticatedUser))
        {
            RedirectToAccessDenied(issue.TypeInfo.ModuleName, "AuthorIssues");
        }

        IssueInfoProvider.DeleteIssueInfo(issue);
    }

    #endregion
}