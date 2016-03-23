using System;
using System.Data;
using System.Linq;

using CMS.Core;
using CMS.Helpers;
using CMS.Newsletters;
using CMS.Base;
using CMS.SiteProvider;
using CMS.Membership;
using CMS.UIControls;
using CMS.WebAnalytics;
using CMS.ExtendedControls;
using CMS.DataEngine;

public partial class CMSModules_Newsletters_Controls_MySubscriptions : CMSAdminControl
{
    #region "Variables"

    private SubscriberInfo subscriber;
    private string subscriberEmail = string.Empty;
    private bool userIsIdentified;
    private string selectorValue = string.Empty;
    private string currentValues = string.Empty;
    private bool mSendConfirmationEmail = true;
    private UserInfo userInfo;
    private readonly ISubscriptionService mSubscriptionService = Service<ISubscriptionService>.Entry();
    private readonly IUnsubscriptionProvider mUnsubscriptionProvider = Service<IUnsubscriptionProvider>.Entry();

    #endregion


    #region "Properties"


    /// <summary>
    /// Messages placeholder
    /// </summary>
    public override MessagesPlaceHolder MessagesPlaceHolder
    {
        get
        {
            return plcMess;
        }
    }


    /// <summary>
    /// Gets or sets the value that indicates whether send confirmation emails.
    /// </summary>
    public bool SendConfirmationEmail
    {
        get
        {
            return mSendConfirmationEmail;
        }
        set
        {
            mSendConfirmationEmail = value;
        }
    }


    /// <summary>
    /// Gets or sets the value that indicates whether this control is visible.
    /// </summary>
    public bool ForcedVisible
    {
        get
        {
            return plcMain.Visible;
        }
        set
        {
            plcMain.Visible = value;
        }
    }


    /// <summary>
    /// Gets or sets the value that indicates whether this control is used in other control.
    /// </summary>
    public bool ExternalUse
    {
        get;
        set;
    }


    /// <summary>
    /// Gets or sets the WebPart cache minutes.
    /// </summary>
    public int CacheMinutes
    {
        get;
        set;
    }


    /// <summary>
    /// Gets or sets current site ID.
    /// </summary>
    public int SiteID
    {
        get;
        set;
    }


    /// <summary>
    /// Gets or sets current user ID.
    /// </summary>
    public int UserID
    {
        get;
        set;
    }


    /// <summary>
    /// Indicates if selector control is on live site.
    /// </summary>
    public override bool IsLiveSite
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("IsLiveSite"), false);
        }
        set
        {
            SetValue("IsLiveSite", value);
            plcMess.IsLiveSite = value;
        }
    }


    /// <summary>
    /// Last selector value.
    /// </summary>
    private string SelectorValue
    {
        get
        {
            if (string.IsNullOrEmpty(selectorValue))
            {
                // Try to get value from hidden field
                selectorValue = ValidationHelper.GetString(hdnValue.Value, string.Empty);
            }

            return selectorValue;
        }
        set
        {
            selectorValue = value;
        }
    }

    #endregion


    #region "Methods"

    /// <summary>
    /// PageLoad.
    /// </summary>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (ExternalUse)
        {
            LoadData();
        }
    }


    /// <summary>
    /// Load data.
    /// </summary>
    public void LoadData()
    {
        if (StopProcessing)
        {
            // Hide control
            Visible = false;
        }
        else
        {
            SetContext();

            // Get specified user if used instead of current user
            if (UserID > 0)
            {
                userInfo = UserInfoProvider.GetUserInfo(UserID);
            }
            else
            {
                userInfo = MembershipContext.AuthenticatedUser;
            }

            // Get specified site ID instead of current site ID
            int siteId = 0;
            if (SiteID > 0)
            {
                siteId = SiteID;
            }
            else
            {
                siteId = SiteContext.CurrentSiteID;
            }

            usNewsletters.WhereCondition = "NewsletterSiteID = " + siteId;
            usNewsletters.OnSelectionChanged += new EventHandler(usNewsletters_OnSelectionChanged);
            usNewsletters.IsLiveSite = IsLiveSite;

            userIsIdentified = (userInfo != null) && (!userInfo.IsPublic()) && (ValidationHelper.IsEmail(userInfo.Email) || ValidationHelper.IsEmail(userInfo.UserName));
            if (userIsIdentified)
            {
                usNewsletters.Visible = true;

                // Try to get subscriber info with specified e-mail
                subscriber = SubscriberInfoProvider.GetSubscriberInfo(userInfo.Email, siteId);
                if (subscriber == null)
                {
                    // Try to get subscriber info according to user info
                    subscriber = SubscriberInfoProvider.GetSubscriberInfo(UserInfo.OBJECT_TYPE, userInfo.UserID, siteId);
                }

                // Get user e-mail address
                if (subscriber != null)
                {
                    subscriberEmail = subscriber.SubscriberEmail;

                    // Get selected newsletters
                    DataSet ds = SubscriberNewsletterInfoProvider.GetSubscriberNewsletters().WhereEquals("SubscriberID", subscriber.SubscriberID).Column("NewsletterID");
                    if (!DataHelper.DataSourceIsEmpty(ds))
                    {
                        currentValues = TextHelper.Join(";", DataHelper.GetStringValues(ds.Tables[0], "NewsletterID"));
                    }

                    // Load selected newsletters
                    if (!RequestHelper.IsPostBack() || !string.IsNullOrEmpty(DataHelper.GetNewItemsInList(SelectorValue, currentValues)))
                    {
                        usNewsletters.Value = currentValues;
                    }
                }

                // Try to get email address from user data
                if (string.IsNullOrEmpty(subscriberEmail))
                {
                    if (ValidationHelper.IsEmail(userInfo.Email))
                    {
                        subscriberEmail = userInfo.Email;
                    }
                    else if (ValidationHelper.IsEmail(userInfo.UserName))
                    {
                        subscriberEmail = userInfo.UserName;
                    }
                }
            }
            else
            {
                usNewsletters.Visible = false;

                if ((UserID > 0) && (MembershipContext.AuthenticatedUser.UserID == UserID))
                {
                    ShowInformation(GetString("MySubscriptions.CannotIdentify"));
                }
                else
                {
                    if (!IsLiveSite)
                    {
                        // It's located in Admin/Users/Subscriptions
                        lblText.ResourceString = "MySubscriptions.EmailCommunicationDisabled";
                    }
                    else
                    {
                        ShowInformation(GetString("MySubscriptions.CannotIdentifyUser"));
                    }
                }
            }

            ReleaseContext();
        }
    }


    /// <summary>
    /// Logs activity for subscribing/unsubscribing
    /// </summary>
    /// <param name="ui">User</param>
    /// <param name="newsletterId">Newsletter ID</param>
    /// <param name="subscribe">Subscribing/unsubscribing flag</param>
    private void LogActivity(UserInfo ui, int newsletterId, bool subscribe)
    {
        if ((subscriber == null) || (ui == null))
        {
            return;
        }

        // Log activity only if subscriber is User
        if ((subscriber.SubscriberType != null) && subscriber.SubscriberType.Equals(UserInfo.OBJECT_TYPE, StringComparison.InvariantCultureIgnoreCase))
        {
            NewsletterInfo news = NewsletterInfoProvider.GetNewsletterInfo(newsletterId);
            Activity activity;
            if (subscribe)
            {
                activity = new ActivityNewsletterSubscribing(subscriber, news, AnalyticsContext.ActivityEnvironmentVariables);
            }
            else
            {
                activity = new ActivityNewsletterUnsubscribing(news, AnalyticsContext.ActivityEnvironmentVariables);
            }
            activity.Log();
        }
    }


    private void usNewsletters_OnSelectionChanged(object sender, EventArgs e)
    {
        if (RaiseOnCheckPermissions("ManageSubscribers", this))
        {
            if (StopProcessing)
            {
                return;
            }
        }

        // Get specified site ID instead of current site ID
        int siteId = 0;
        if (SiteID > 0)
        {
            siteId = SiteID;
        }
        else
        {
            siteId = SiteContext.CurrentSiteID;
        }

        if ((subscriber == null) && (userInfo != null))
        {
            // Create new subsciber (bind to existing user account)
            if ((!userInfo.IsPublic()) && (ValidationHelper.IsEmail(userInfo.Email) || ValidationHelper.IsEmail(userInfo.UserName)))
            {
                subscriber = new SubscriberInfo();
                if (userInfo != null)
                {
                    if (!string.IsNullOrEmpty(userInfo.FirstName) && !string.IsNullOrEmpty(userInfo.LastName))
                    {
                        subscriber.SubscriberFirstName = userInfo.FirstName;
                        subscriber.SubscriberLastName = userInfo.LastName;
                    }
                    else
                    {
                        subscriber.SubscriberFirstName = userInfo.FullName;
                    }
                    // Full name consists of "user " and user full name
                    subscriber.SubscriberFullName = new SubscriberFullNameFormater().GetUserSubscriberName(userInfo.FullName);
                }

                subscriber.SubscriberSiteID = siteId;
                subscriber.SubscriberType = UserInfo.OBJECT_TYPE;
                subscriber.SubscriberRelatedID = userInfo.UserID;
                // Save subscriber to DB
                SubscriberInfoProvider.SetSubscriberInfo(subscriber);
            }
        }

        if (subscriber == null)
        {
            return;
        }

        // Create membership between current contact and subscriber
        ModuleCommands.OnlineMarketingCreateRelation(subscriber.SubscriberID, MembershipType.NEWSLETTER_SUBSCRIBER, ModuleCommands.OnlineMarketingGetCurrentContactID());

        // Remove old items
        int newsletterId = 0;
        string newValues = ValidationHelper.GetString(usNewsletters.Value, null);
        string items = DataHelper.GetNewItemsInList(newValues, currentValues);
        if (!String.IsNullOrEmpty(items))
        {
            string[] newItems = items.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            var ids = newItems.Select(item => ValidationHelper.GetInteger(item, 0)).ToArray();
            var subscriptions = SubscriberNewsletterInfoProvider
                .GetSubscriberNewsletters()
                .WhereEquals("SubscriberID", subscriber.SubscriberID)
                .WhereIn("NewsletterID", ids);
                    
            foreach (var subscription in subscriptions)
            {
                subscription.Delete();
                LogActivity(userInfo, newsletterId, false);
            }
        }

        // Add new items
        items = DataHelper.GetNewItemsInList(currentValues, newValues);
        if (!String.IsNullOrEmpty(items))
        {
            string[] newItems = items.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            if (newItems != null)
            {
                foreach (string item in newItems)
                {
                    newsletterId = ValidationHelper.GetInteger(item, 0);

                    try
                    {
                        // If subscriber is not subscribed, subscribe him
                        if (!mSubscriptionService.IsSubscribed(subscriber.SubscriberID, newsletterId))
                        {
                            mSubscriptionService.Subscribe(subscriber.SubscriberID, newsletterId, new SubscribeSettings()
                            {
                                SendConfirmationEmail = SendConfirmationEmail,
                                RequireOptIn = true,
                                RemoveAlsoUnsubscriptionFromAllNewsletters = true,
                            });
                            // Log activity
                            LogActivity(userInfo, newsletterId, true);
                        }
                    }
                    catch
                    {
                        // Can occur e.g. when newsletter is deleted while the user is selecting it for subscription.
                        // This is rare scenario, the main purpose of this catch is to avoid YSOD on the live site.
                    }
                }
            }
        }

        // Display information about successful (un)subscription
        ShowChangesSaved();
    }


    protected void btnUnsubscribeFromAll_Click(object sender, EventArgs e)
    {
        if (userIsIdentified && IsLiveSite)
        {
            string email = userInfo.Email;
            var isUnsubscribed = mUnsubscriptionProvider.IsUnsubscribedFromAllNewsletters(email, SiteContext.CurrentSiteID);

            if (!isUnsubscribed)
            {
                mSubscriptionService.UnsubscribeFromAllNewsletters(email, SiteContext.CurrentSiteID);
            }
            else
            {
                mUnsubscriptionProvider.RemoveUnsubscriptionsFromAllNewsletters(email, SiteContext.CurrentSiteID);
            }
        }
    }


    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);

        if (!IsLiveSite)
        {
            // It's located in Admin/Users/Subscriptions
            headNewsletters.Visible = false;
            lblUnsubscribeFromAll.Visible = false;
            btnUsubscribeFromAll.Visible = false;
        }

        // Display appropriate message
        if (userIsIdentified)
        {
            if (!IsLiveSite)
            {
                lblText.ResourceString = "mysubscriptions.selectorheading.thirdperson";
            }
            else
            {
                string email = userInfo.Email;
                bool isUnsubscribedFromAll = mUnsubscriptionProvider.IsUnsubscribedFromAllNewsletters(email, SiteContext.CurrentSiteID);

                if (isUnsubscribedFromAll)
                {
                    lblUnsubscribeFromAll.Text = GetString("mysubscriptions.unsubscribed.description");
                    btnUsubscribeFromAll.Text = GetString("mysubscriptions.unsubscribed.buttontext");
                }
                else
                {
                    lblUnsubscribeFromAll.Text = string.Format(GetString("mysubscriptions.notunsubscribed.description"), email);
                    btnUsubscribeFromAll.Text = GetString("mysubscriptions.notunsubscribed.buttontext");
                }
            }
        }

        // Preserve selected values
        hdnValue.Value = ValidationHelper.GetString(usNewsletters.Value, string.Empty);
    }


    /// <summary>
    /// Overridden SetValue - because of MyAccount webpart.
    /// </summary>
    /// <param name="propertyName">Name of the property to set</param>
    /// <param name="value">Value to set</param>
    public override bool SetValue(string propertyName, object value)
    {
        base.SetValue(propertyName, value);

        switch (propertyName.ToLowerCSafe())
        {
            case "forcedvisible":
                ForcedVisible = ValidationHelper.GetBoolean(value, false);
                break;

            case "externaluse":
                ExternalUse = ValidationHelper.GetBoolean(value, false);
                break;

            case "cacheminutes":
                CacheMinutes = ValidationHelper.GetInteger(value, 0);
                break;

            case "reloaddata":
                // Special property which enables to call LoadData from MyAccount webpart
                LoadData();
                break;

            case "userid":
                UserID = ValidationHelper.GetInteger(value, 0);
                break;

            case "siteid":
                SiteID = ValidationHelper.GetInteger(value, 0);
                break;

            case "sendconfirmationemail":
                mSendConfirmationEmail = ValidationHelper.GetBoolean(value, true);
                break;
        }

        return true;
    }

    #endregion
}