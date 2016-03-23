using System;

using CMS.Core;
using CMS.Helpers;
using CMS.Newsletters;
using CMS.SiteProvider;
using CMS.UIControls;
using CMS.WebAnalytics;

public partial class CMSModules_Newsletters_Controls_SubscriptionApproval : CMSUserControl
{
    #region "Public properties"

    /// <summary>
    /// Gets or sets successful approval text.
    /// </summary>
    public string SuccessfulApprovalText
    {
        get;
        set;
    }


    /// <summary>
    /// Gets or sets unsuccessful approval text.
    /// </summary>
    public string UnsuccessfulApprovalText
    {
        get;
        set;
    }

    #endregion


    #region "Methods"

    protected void Page_Load(object sender, EventArgs e)
    {
        // If StopProcessing flag is set, do nothing
        if (StopProcessing)
        {
            Visible = false;
            return;
        }

        string subscriptionHash = QueryHelper.GetString("subscriptionhash", string.Empty);
        string requestTime = QueryHelper.GetString("datetime", string.Empty);
        DateTime datetime = DateTimeHelper.ZERO_TIME;

        // Get date and time
        if (!string.IsNullOrEmpty(requestTime))
        {
            try
            {
                datetime = DateTime.ParseExact(requestTime, SecurityHelper.EMAIL_CONFIRMATION_DATETIME_FORMAT, null);
            }
            catch
            {
                lblInfo.Text = ResHelper.GetString("newsletter.approval_failed");
                return;
            }
        }

        if (string.IsNullOrEmpty(subscriptionHash))
        {
            Visible = false;
            return;
        }

        var approvalService = Service<ISubscriptionApprovalService>.Entry();
        var result = approvalService.ApproveSubscription(subscriptionHash, false, SiteContext.CurrentSiteName, datetime);

        switch (result)
        {
            case ApprovalResult.Success:
                lblInfo.Text = !String.IsNullOrEmpty(SuccessfulApprovalText) ? SuccessfulApprovalText : ResHelper.GetString("newsletter.successful_approval");

                var subscription = SubscriberNewsletterInfoProvider.GetSubscriberNewsletterInfo(subscriptionHash);
                if (subscription == null)
                {
                    return;
                }

                var newsletter = NewsletterInfoProvider.GetNewsletterInfo(subscription.NewsletterID);
                var subscriber = SubscriberInfoProvider.GetSubscriberInfo(subscription.SubscriberID);

                if ((newsletter == null) || (subscriber == null))
                {
                    return;
                }

                LogNewsletterSubscriptionActivity(subscriber, newsletter);
                break;

            case ApprovalResult.Failed:
                lblInfo.Text = !String.IsNullOrEmpty(UnsuccessfulApprovalText) ? UnsuccessfulApprovalText : ResHelper.GetString("newsletter.approval_failed");
                break;

            case ApprovalResult.TimeExceeded:
                lblInfo.Text = !String.IsNullOrEmpty(UnsuccessfulApprovalText) ? UnsuccessfulApprovalText : ResHelper.GetString("newsletter.approval_timeexceeded");
                break;

            case ApprovalResult.AlreadyApproved:
                lblInfo.Text = !String.IsNullOrEmpty(SuccessfulApprovalText) ? SuccessfulApprovalText : ResHelper.GetString("newsletter.successful_approval");
                break;

            // Subscription not found
            default:
                lblInfo.Text = !String.IsNullOrEmpty(UnsuccessfulApprovalText) ? UnsuccessfulApprovalText : ResHelper.GetString("newsletter.approval_invalid");
                break;
        }
    }


    private static void LogNewsletterSubscriptionActivity(SubscriberInfo subscriber, NewsletterInfo newsletter)
    {
        // Under what contacts this subscriber belogs to?
        int contactId = ActivityTrackingHelper.GetContactID(subscriber);
        if (contactId <= 0)
        {
            return;
        }

        // Activity object is created for contact taken from ActivityContext.
        // Set ID of contact opening this email into this context otherwise new contact could be created.
        var activityContext = AnalyticsContext.ActivityEnvironmentVariables;
        activityContext.ContactID = contactId;

        var activity = new ActivityNewsletterSubscribing(subscriber, newsletter, activityContext);
        activity.Log();
    }

    #endregion
}