using System;
using System.Data;
using System.Globalization;
using System.Web.UI.WebControls;

using CMS.Base;
using CMS.Core;
using CMS.DataEngine;
using CMS.ExtendedControls;
using CMS.Helpers;
using CMS.Helpers.Markup;
using CMS.Membership;
using CMS.Newsletters;
using CMS.UIControls;

[EditedObject(SubscriberInfo.OBJECT_TYPE, "objectid")]
[UIElement(ModuleName.NEWSLETTER, "SubscriberProperties.Subscriptions")]
public partial class CMSModules_Newsletters_Tools_Subscribers_Subscriber_Subscriptions : CMSNewsletterPage
{
    #region "Constants and variables"

    private const string SELECT = "SELECT";
    private const string APPROVE = "APPROVE";
    private const string REMOVE = "REMOVE";
    private int mSubscriberID;
    private bool mIsMultipleSubscriber;
    private readonly ISubscriptionService mSubscriptionService = Service<ISubscriptionService>.Entry();

    #endregion


    protected void Page_Load(object sender, EventArgs e)
    {
        // Get subscriber by its ID and check its existence
        var subscriber = EditedObject as SubscriberInfo;

        if (subscriber == null)
        {
            RedirectToAccessDenied(GetString("general.invalidparameters"));
        }

        if (!subscriber.CheckPermissions(PermissionsEnum.Read, CurrentSiteName, CurrentUser))
        {
            RedirectToAccessDenied(subscriber.TypeInfo.ModuleName, "ManageSubscribers");
        }

        mSubscriberID = subscriber.SubscriberID;

        // Check if it is role or contact group subscriber
        mIsMultipleSubscriber = (subscriber.SubscriberRelatedID > 0) && (subscriber.SubscriberType != null) &&
                               (subscriber.SubscriberType.EqualsCSafe(RoleInfo.OBJECT_TYPE, true) ||
                                subscriber.SubscriberType.EqualsCSafe(PredefinedObjectType.CONTACTGROUP, true) ||
                                subscriber.SubscriberType.EqualsCSafe(PredefinedObjectType.PERSONA, true));

        // Initialize uniselector for newsletters
        selectNewsletter.UniSelector.SelectionMode = SelectionModeEnum.MultipleButton;
        selectNewsletter.UniSelector.OnItemsSelected += UniSelector_OnItemsSelected;
        selectNewsletter.UniSelector.ReturnColumnName = "NewsletterID";
        selectNewsletter.ShowSiteFilter = false;
        selectNewsletter.ResourcePrefix = "newsletterselect";
        selectNewsletter.IsLiveSite = false;

        // Initialize unigrid
        unigridNewsletters.WhereCondition = "SubscriberID = " + mSubscriberID.ToString(CultureInfo.InvariantCulture);
        unigridNewsletters.OnAction += unigridNewsletters_OnAction;
        unigridNewsletters.OnExternalDataBound += unigridNewsletters_OnExternalDataBound;

        // Initialize mass actions
        if (drpActions.Items.Count == 0)
        {
            drpActions.Items.Add(new ListItem(GetString("general.selectaction"), SELECT));
            drpActions.Items.Add(new ListItem(GetString("newsletter.approvesubscription"), APPROVE));
            drpActions.Items.Add(new ListItem(GetString("newsletter.deletesubscriptions"), REMOVE));
        }
    }


    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);

        // Display/hide mass action dialog under the unigrid
        pnlActions.Visible = !DataHelper.DataSourceIsEmpty(unigridNewsletters.GridView.DataSource);
        // Display/hide double opt-in option
        plcRequireOptIn.Visible = !mIsMultipleSubscriber;
    }


    /// <summary>
    /// Uniselector item selected event handler.
    /// </summary>
    protected void UniSelector_OnItemsSelected(object sender, EventArgs e)
    {
        // Check permissions
        if (!MembershipContext.AuthenticatedUser.IsAuthorizedPerResource("cms.newsletter", "ManageSubscribers"))
        {
            RedirectToAccessDenied("cms.newsletter", "ManageSubscribers");
        }

        // Get new items from selector
        string newValues = ValidationHelper.GetString(selectNewsletter.Value, null);
        string[] newItems = newValues.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

        // Get all selected newsletters
        foreach (string item in newItems)
        {
            int newsletterId = ValidationHelper.GetInteger(item, 0);

            if (!mSubscriptionService.IsSubscribed(mSubscriberID, newsletterId))
            {
                var subscribeSettings = new SubscribeSettings()
                {
                    SendConfirmationEmail = chkSendConfirmation.Checked,
                    RequireOptIn = !mIsMultipleSubscriber && chkRequireOptIn.Checked,
                    RemoveAlsoUnsubscriptionFromAllNewsletters = false,
                };

                mSubscriptionService.Subscribe(mSubscriberID, newsletterId, subscribeSettings);
            }
        }

        selectNewsletter.Value = null;
        unigridNewsletters.ReloadData();
        pnlUpdate.Update();
    }


    /// <summary>
    /// Unigrid databound handler.
    /// </summary>
    protected object unigridNewsletters_OnExternalDataBound(object sender, string sourceName, object parameter)
    {
        switch (sourceName.ToUpperInvariant())
        {
            case APPROVE:
                bool approved = ValidationHelper.GetBoolean(((DataRowView)((GridViewRow)parameter).DataItem).Row["SubscriptionApproved"], true);
                var button = ((CMSGridActionButton)sender);
                button.Visible = !approved;
                break;

            case "STATUS":
                return GetSubscriptionStatus(parameter as DataRowView);
        }

        return null;
    }


    /// <summary>
    /// Unigrid newsletters action handler.
    /// </summary>
    protected void unigridNewsletters_OnAction(string actionName, object actionArgument)
    {
        // Check 'configure' permission
        if (!MembershipContext.AuthenticatedUser.IsAuthorizedPerResource("cms.newsletter", "ManageSubscribers"))
        {
            RedirectToAccessDenied("cms.newsletter", "ManageSubscribers");
        }

        var newsletter = NewsletterInfoProvider.GetNewsletterInfo(ValidationHelper.GetInteger(actionArgument, 0));
        if (newsletter == null)
        {
            ShowError(GetString("newsletter.erroronactiononsubscriber"));
            return;
        }

        DoSubscriberAction(newsletter, actionName);
    }


    /// <summary>
    /// Returns colored status of the subscription
    /// </summary>
    private FormattedText GetSubscriptionStatus(DataRowView rowView)
    {
        var newsletterID = DataHelper.GetIntValue(rowView.Row, "NewsletterID");

        var subscribed = mSubscriptionService.IsSubscribed(mSubscriberID, newsletterID);

        if (!subscribed)
        {
            return new FormattedText(GetString("newsletterview.headerunsubscribed")).ColorRed();
        }

        bool approved = DataHelper.GetBoolValue(rowView.Row, "SubscriptionApproved", true);
        if (approved)
        {
            return new FormattedText(GetString("general.approved")).ColorGreen();
        }

        return new FormattedText(GetString("administration.users_header.myapproval")).ColorOrange();
    }


    /// <summary>
    /// Handles multiple selector actions.
    /// </summary>
    protected void btnOk_Clicked(object sender, EventArgs e)
    {
        // Check permissions
        if (!MembershipContext.AuthenticatedUser.IsAuthorizedPerResource("cms.newsletter", "ManageSubscribers"))
        {
            RedirectToAccessDenied("cms.newsletter", "ManageSubscribers");
        }
        // Return if no action was selected
        if (drpActions.SelectedValue.EqualsCSafe(SELECT, true))
        {
            return;
        }

        // Get selected items
        if (unigridNewsletters.SelectedItems.Count > 0)
        {
            foreach (string id in unigridNewsletters.SelectedItems)
            {
                var newsletter = NewsletterInfoProvider.GetNewsletterInfo(ValidationHelper.GetInteger(id, 0));
                if (newsletter == null)
                {
                    continue;
                }

                DoSubscriberAction(newsletter, drpActions.SelectedValue);
            }
        }

        unigridNewsletters.ResetSelection();
        unigridNewsletters.ReloadData();
    }


    /// <summary>
    /// Performs action on given subscriber.
    /// </summary>
    /// <param name="newsletter">Newsletter info</param>
    /// <param name="actionName">Name of action</param>
    private void DoSubscriberAction(NewsletterInfo newsletter, string actionName)
    {
        try
        {
            var subscriber = SubscriberInfoProvider.GetSubscriberInfo(mSubscriberID);

            switch (actionName.ToUpperInvariant())
            {
                case REMOVE:
                    mSubscriptionService.RemoveSubscription(subscriber.SubscriberID, newsletter.NewsletterID, chkSendConfirmation.Checked);
                    break;

                case APPROVE:
                    SubscriberNewsletterInfoProvider.ApproveSubscription(mSubscriberID, newsletter.NewsletterID);
                    break;
            }
        }
        // Exception might be thrown when one of the object does not exist, especially when newsletter gets deleted before refreshing this page.
        catch (Exception exception)
        {
            LogAndShowError("Subscriber subscriptions", "NEWSLETTERS", exception);
        }
    }
}