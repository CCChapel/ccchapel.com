using System;
using System.Linq;

using CMS.Helpers;
using CMS.MacroEngine;
using CMS.Newsletters;
using CMS.OnlineMarketing;
using CMS.WebAnalytics;


public partial class CMSModules_ContactManagement_Controls_UI_ActivityDetails_NewsletterUnsubscriptionFromAll : ActivityDetail
{
    #region "Methods"

    public override bool LoadData(ActivityInfo ai)
    {
        if ((ai == null) || (ai.ActivityType != PredefinedActivityType.NEWSLETTER_UNSUBSCRIBING_FROM_ALL))
        {
            return false;
        }

        // Get issue subject
        int issueId = ai.ActivityItemID;
        var issueInfo = IssueInfoProvider.GetIssueInfo(issueId);
        if (issueInfo != null)
        {
            // Get newsletter name
            var newsletterInfo = NewsletterInfoProvider.GetNewsletterInfo(issueInfo.IssueNewsletterID);
            if (newsletterInfo != null)
            {
                ucDetails.AddRow("om.activitydetails.newsletter", newsletterInfo.NewsletterDisplayName);
            }

            var issueSubject = MacroSecurityProcessor.RemoveSecurityParameters(issueInfo.IssueSubject, true, null);
            ucDetails.AddRow("om.activitydetails.newsletterissue", issueSubject);
        }

        return ucDetails.IsDataLoaded;
    }

    #endregion
}