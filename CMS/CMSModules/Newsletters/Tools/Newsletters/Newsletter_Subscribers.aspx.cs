using System;
using System.Data;
using System.Web.UI.WebControls;

using CMS.Core;
using CMS.FormControls;
using CMS.Helpers;
using CMS.Helpers.Markup;
using CMS.LicenseProvider;
using CMS.Newsletters;
using CMS.Base;
using CMS.PortalEngine;
using CMS.SiteProvider;
using CMS.Membership;
using CMS.UIControls;
using CMS.ExtendedControls;
using CMS.DataEngine;

[UIElement(ModuleName.NEWSLETTER, "Newsletter.Subscribers")]
[EditedObject(NewsletterInfo.OBJECT_TYPE, "objectid")]
public partial class CMSModules_Newsletters_Tools_Newsletters_Newsletter_Subscribers : CMSNewsletterPage
{
    #region "Variables"

    private const string SELECT = "SELECT";
    private const string APPROVE = "APPROVE";
    private const string REMOVE = "REMOVE";
    private const string BLOCK = "BLOCK";
    private const string UNBLOCK = "UNBLOCK";

    private int mBounceLimit;
    private bool mBounceInfoAvailable;
    private NewsletterInfo mNewsletter;
    private readonly ISubscriptionService mSubscriptionService = Service<ISubscriptionService>.Entry();


    /// <summary>
    /// Contact group selector.
    /// </summary>
    private FormEngineUserControl cgSelector;


    /// <summary>
    /// Contact selector.
    /// </summary>
    private FormEngineUserControl cSelector;


    /// <summary>
    /// Persona selector.
    /// </summary>
    private UniSelector personaSelector;

    #endregion


    #region "Methods"

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        CurrentMaster.ActionsViewstateEnabled = true;
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        mNewsletter = EditedObject as NewsletterInfo;
        if (mNewsletter == null)
        {
            RedirectToAccessDenied(GetString("general.invalidparameters"));
        }

        if (!mNewsletter.CheckPermissions(PermissionsEnum.Read, CurrentSiteName, CurrentUser))
        {
            RedirectToAccessDenied(mNewsletter.TypeInfo.ModuleName, "ManageSubscribers");
        }

        ScriptHelper.RegisterDialogScript(this);

        CurrentMaster.DisplayActionsPanel = true;
        chkRequireOptIn.CheckedChanged += chkRequireOptIn_CheckedChanged;

        string currentSiteName = SiteContext.CurrentSiteName;
        mBounceLimit = NewsletterHelper.BouncedEmailsLimit(currentSiteName);
        mBounceInfoAvailable = NewsletterHelper.MonitorBouncedEmails(currentSiteName);

        // Check if newsletter enables double opt-in
        if (!mNewsletter.NewsletterEnableOptIn)
        {
            chkRequireOptIn.Visible = false;
        }

        if (!RequestHelper.IsPostBack())
        {
            chkSendConfirmation.Checked = false;
        }

        // Initialize unigrid
        UniGridSubscribers.WhereCondition = "NewsletterID = " + mNewsletter.NewsletterID;
        UniGridSubscribers.OnAction += UniGridSubscribers_OnAction;
        UniGridSubscribers.OnExternalDataBound += UniGridSubscribers_OnExternalDataBound;
        UniGridSubscribers.ZeroRowsText = GetString("newsletter.subscribers.nodata");
        UniGridSubscribers.FilteredZeroRowsText = GetString("newsletter.subscribers.noitemsfound");

        // Initialize selectors and mass actions
        SetupSelectors();
    }


    protected void Page_PreRender(object sender, EventArgs e)
    {
        // Hide columns with bounced emails if bounce info is not available
        UniGridSubscribers.NamedColumns["blocked"].Visible =
            UniGridSubscribers.NamedColumns["bounces"].Visible = mBounceInfoAvailable;

        pnlActions.Visible = !DataHelper.DataSourceIsEmpty(UniGridSubscribers.GridView.DataSource);
    }


    /// <summary>
    /// Configures selectors.
    /// </summary>
    private void SetupSelectors()
    {
        // Setup role selector
        selectRole.CurrentSelector.SelectionMode = SelectionModeEnum.MultipleButton;
        selectRole.CurrentSelector.OnItemsSelected += RolesSelector_OnItemsSelected;
        selectRole.CurrentSelector.ReturnColumnName = "RoleID";
        selectRole.CurrentSelector.ZeroRowsText = string.Format(GetString("newsletter.subscribers.addroles.nodata"), URLHelper.ResolveUrl(UIContextHelper.GetApplicationUrl("CMS.Roles", "Administration.Roles")));
        selectRole.ShowSiteFilter = false;
        selectRole.CurrentSelector.ResourcePrefix = "addroles";
        selectRole.IsLiveSite = false;
        selectRole.UseCodeNameForSelection = false;
        selectRole.GlobalRoles = false;

        // Setup user selector
        selectUser.SelectionMode = SelectionModeEnum.MultipleButton;
        selectUser.UniSelector.OnItemsSelected += UserSelector_OnItemsSelected;
        selectUser.UniSelector.ReturnColumnName = "UserID";
        selectUser.UniSelector.DisplayNameFormat = "{%FullName%} ({%Email%})";
        selectUser.UniSelector.AdditionalSearchColumns = "UserName, Email";
        selectUser.UniSelector.ZeroRowsText = string.Format(GetString("newsletter.subscribers.addusers.nodata"), URLHelper.ResolveUrl(UIContextHelper.GetApplicationUrl("CMS.Users", "Administration.Users")));
        selectUser.WhereCondition = new WhereCondition().WhereNotEmpty("Email").ToString(true);
        selectUser.ShowSiteFilter = false;
        selectUser.ResourcePrefix = "newsletteraddusers";
        selectUser.IsLiveSite = false;

        // Setup subscriber selector
        selectSubscriber.UniSelector.SelectionMode = SelectionModeEnum.MultipleButton;
        selectSubscriber.UniSelector.OnItemsSelected += SubscriberSelector_OnItemsSelected;
        selectSubscriber.UniSelector.ReturnColumnName = "SubscriberID";
        selectSubscriber.UniSelector.ZeroRowsText = string.Format(GetString("newsletter.subscribers.addsubscribers.nodata"), URLHelper.ResolveUrl(UIContextHelper.GetApplicationUrl(ModuleName.NEWSLETTER, "Newsletter", "?tabname=AllSubscribers")));
        selectSubscriber.ShowSiteFilter = false;
        selectSubscriber.IsLiveSite = false;
        selectSubscriber.UniSelector.RemoveMultipleCommas = true;

        // Setup contact group and contact selectors
        if (ModuleEntryManager.IsModuleLoaded(ModuleName.ONLINEMARKETING) && LicenseHelper.CheckFeature(RequestContext.CurrentDomain, FeatureEnum.ContactManagement))
        {
            plcSelectCG.Controls.Clear();

            // Check read permission for contact groups
            if (MembershipContext.AuthenticatedUser.IsAuthorizedPerResource(ModuleName.CONTACTMANAGEMENT, "ReadContactGroups"))
            {
                // Load selector control and initialize it
                cgSelector = (FormEngineUserControl)Page.LoadUserControl("~/CMSModules/ContactManagement/FormControls/ContactGroupSelector.ascx");
                if (cgSelector != null)
                {
                    cgSelector.ID = "selectCG";
                    cgSelector.ShortID = "scg";
                    // Get inner uniselector control
                    UniSelector selector = (UniSelector)cgSelector.GetValue("uniselector");
                    if (selector != null)
                    {
                        // Bind an event handler on 'items selected' event
                        selector.OnItemsSelected += CGSelector_OnItemsSelected;
                        selector.ResourcePrefix = "contactgroupsubscriber";
                        selector.ZeroRowsText = string.Format(GetString("newsletter.subscribers.addcontactgroups.nodata"), URLHelper.ResolveUrl(UIContextHelper.GetApplicationUrl(ModuleName.ONLINEMARKETING, "ContactGroups")));
                    }
                    // Insert selector to the header
                    plcSelectCG.Controls.Add(cgSelector);
                }
            }

            // Check read permission for contacts
            if (MembershipContext.AuthenticatedUser.IsAuthorizedPerResource(ModuleName.CONTACTMANAGEMENT, "ReadContacts"))
            {
                // Load selector control and initialize it
                cSelector = (FormEngineUserControl)Page.LoadUserControl("~/CMSModules/ContactManagement/FormControls/ContactSelector.ascx");
                if (cSelector != null)
                {
                    cSelector.ID = "slContact";
                    cSelector.ShortID = "sc";
                    // Set where condition to filter contacts with email addresses
                    cSelector.SetValue("wherecondition", "NOT (ContactEmail IS NULL OR ContactEmail LIKE '')");
                    // Set site ID
                    cSelector.SetValue("siteid", SiteContext.CurrentSiteID);
                    // Get inner uniselector control
                    UniSelector selector = (UniSelector)cSelector.GetValue("uniselector");
                    if (selector != null)
                    {
                        // Bind an event handler on 'items selected' event
                        selector.OnItemsSelected += ContactSelector_OnItemsSelected;
                        selector.SelectionMode = SelectionModeEnum.MultipleButton;
                        selector.ResourcePrefix = "contactsubscriber";
                        selector.DisplayNameFormat = "{%ContactFirstName%} {%ContactLastName%} ({%ContactEmail%})";
                        selector.AdditionalSearchColumns = "ContactFirstName,ContactMiddleName,ContactEmail";
                        selector.ZeroRowsText = string.Format(GetString("newsletter.subscribers.addcontacts.nodata"), URLHelper.ResolveUrl(UIContextHelper.GetApplicationUrl(ModuleName.ONLINEMARKETING, "ContactsFrameset", "?tabname=contacts")));
                    }
                    // Insert selector to the header
                    plcSelectCG.Controls.Add(cSelector);
                }
            }
        }

        // Setup persona selectors
        if (ModuleEntryManager.IsModuleLoaded(ModuleName.PERSONAS) && LicenseHelper.CheckFeature(RequestContext.CurrentDomain, FeatureEnum.Personas))
        {
            // Check read permission for contact groups
            if (MembershipContext.AuthenticatedUser.IsAuthorizedPerResource(ModuleName.PERSONAS, "Read"))
            {
                // Load selector control and initialize it
                personaSelector = (UniSelector)Page.LoadUserControl("~/CMSAdminControls/UI/Uniselector/Uniselector.ascx");
                if (personaSelector != null)
                {
                    personaSelector.ID = "personaSelector";
                    personaSelector.ShortID = "ps";
                    personaSelector.ObjectType = PredefinedObjectType.PERSONA;
                    personaSelector.ReturnColumnName = "PersonaID";
                    personaSelector.WhereCondition = "PersonaSiteID = " + SiteContext.CurrentSiteID;
                    personaSelector.SelectionMode = SelectionModeEnum.MultipleButton;
                    personaSelector.DisplayNameFormat = "{%PersonaDisplayName%}";
                    personaSelector.ResourcePrefix = "personasubscriber";
                    personaSelector.IsLiveSite = false;
                    personaSelector.ZeroRowsText = string.Format(GetString("newsletter.subscribers.addpersonas.nodata"), URLHelper.ResolveUrl(UIContextHelper.GetApplicationUrl(ModuleName.PERSONAS, "Personas")));

                    // Bind an event handler on 'items selected' event
                    personaSelector.OnItemsSelected += PersonaSelector_OnItemsSelected;

                    // Add selector to the header
                    plcSelectCG.Controls.Add(personaSelector);
                }
            }
        }

        // Initialize mass actions
        if (drpActions.Items.Count == 0)
        {
            drpActions.Items.Add(new ListItem(GetString("general.selectaction"), SELECT));
            drpActions.Items.Add(new ListItem(GetString("newsletter.approvesubscription"), APPROVE));
            drpActions.Items.Add(new ListItem(GetString("newsletter.deletesubscription"), REMOVE));
        }
    }


    /// <summary>
    /// Unigrid external databound event handler.
    /// </summary>
    protected object UniGridSubscribers_OnExternalDataBound(object sender, string sourceName, object parameter)
    {
        string sourceNameUpper = sourceName.ToUpperInvariant();

        switch (sourceNameUpper)
        {
            case BLOCK:
                var gridViewRow = parameter as GridViewRow;
                if (gridViewRow != null)
                {
                    return SetBlockAction(sender, (gridViewRow.DataItem) as DataRowView);
                }
                break;

            case UNBLOCK:
                var viewRow = parameter as GridViewRow;
                if (viewRow != null)
                {
                    return SetUnblockAction(sender, (viewRow.DataItem) as DataRowView);
                }
                break;

            case APPROVE:
                bool approved = ValidationHelper.GetBoolean(((DataRowView)((GridViewRow)parameter).DataItem).Row["SubscriptionApproved"], true);
                CMSGridActionButton button = ((CMSGridActionButton)sender);
                button.Visible = !approved;
                break;

            case "EMAIL":
                return GetEmail(parameter as DataRowView);

            case "STATUS":
                return GetSubscriptionStatus(parameter as DataRowView);

            case "BLOCKED":
                return GetBlocked(parameter as DataRowView);

            case "BOUNCES":
                return GetBounces(parameter as DataRowView);
        }

        return null;
    }


    /// <summary>
    /// Handles the UniGrid's OnAction event.
    /// </summary>
    /// <param name="actionName">Name of item (button) that threw event</param>
    /// <param name="actionArgument">ID (value of Primary key) of corresponding data row</param>
    protected void UniGridSubscribers_OnAction(string actionName, object actionArgument)
    {
        // Check 'manage subscribers' permission
        CheckAuthorization();

        int subscriberId = ValidationHelper.GetInteger(actionArgument, 0);

        DoSubscriberAction(subscriberId, actionName);
    }


    /// <summary>
    /// Displays/hides block action button in unigrid.
    /// </summary>
    private object SetBlockAction(object sender, DataRowView rowView)
    {
        int bounces = GetBouncesFromRow(rowView);

        var imageButton = sender as CMSGridActionButton;
        if (imageButton != null)
        {
            imageButton.Visible = mBounceInfoAvailable && !IsMultiSubscriber(rowView)
            && ((mBounceLimit > 0 && bounces < mBounceLimit) || (mBounceLimit == 0 && bounces < int.MaxValue));
        }

        return null;
    }


    /// <summary>
    /// Displays/hides un-block action button in unigrid.
    /// </summary>
    private object SetUnblockAction(object sender, DataRowView rowView)
    {
        int bounces = GetBouncesFromRow(rowView);

        var imageButton = sender as CMSGridActionButton;
        if (imageButton != null)
        {
            imageButton.Visible = mBounceInfoAvailable && !IsMultiSubscriber(rowView)
            && ((mBounceLimit > 0 && bounces >= mBounceLimit) || (mBounceLimit == 0 && bounces == int.MaxValue));
        }

        return null;
    }


    /// <summary>
    /// Returns subscriber's e-mail address.
    /// </summary>
    private object GetEmail(DataRowView rowView)
    {
        // Try to get subscriber email - classic or contact one
        string email = ValidationHelper.GetString(rowView.Row["SubscriberEmail"], string.Empty);
        if (string.IsNullOrEmpty(email))
        {
            // Get user email
            return ValidationHelper.GetString(rowView.Row["Email"], string.Empty);
        }
        return email;
    }


    /// <summary>
    /// Returns colored status of the subscription.
    /// </summary>
    private FormattedText GetSubscriptionStatus(DataRowView rowView)
    {
        var subscriberID = DataHelper.GetIntValue(rowView.Row, "SubscriberID");

        var user = GetSubscriberUser(rowView);
        if (user != null && !user.UserEnabled)
        {
            return new FormattedText(GetString("newsletterview.subscriberuserdisabled")).ColorRed();
        }

        var subscribed = mSubscriptionService.IsSubscribed(subscriberID, mNewsletter.NewsletterID);
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
    /// Checks whether the given subscriber is of type User. If so, returns proper User info; otherwise, null.
    /// </summary>
    /// <param name="subscriber">Subscriber info to be checked</param>
    /// <returns>UserInfo related to the given subscriber or null</returns>
    private UserInfo GetSubscriberUser(SubscriberInfo subscriber)
    {
        if (subscriber.SubscriberType == UserInfo.OBJECT_TYPE)
        {
            return UserInfoProvider.GetUserInfo(subscriber.SubscriberRelatedID);
        }

        return null;
    }


    /// <summary>
    /// Checks whether the given subscriber is of type User. If so, returns proper User info; otherwise, null.
    /// </summary>
    /// <param name="rowView">Subscriber to be checked</param>
    /// <returns>UserInfo related to the given subscriber or null</returns>
    private UserInfo GetSubscriberUser(DataRowView rowView)
    {
        string subscriptionType = ValidationHelper.GetString(rowView.Row["SubscriberType"], string.Empty);
        if (subscriptionType == UserInfo.OBJECT_TYPE)
        {
            int userId = ValidationHelper.GetInteger(rowView.Row["SubscriberRelatedID"], 0);
            return UserInfoProvider.GetUserInfo(userId);
        }

        return null;
    }


    /// <summary>
    /// Returns colored yes/no or nothing according to subscriber's blocked info.
    /// </summary>
    private string GetBlocked(DataRowView rowView)
    {
        // Do not handle if bounce email monitoring is not available
        if (!mBounceInfoAvailable)
        {
            return null;
        }

        // If bounce limit is not a natural number, then the feature is considered disabled
        if (mBounceLimit < 0)
        {
            return UniGridFunctions.ColoredSpanYesNoReversed(false);
        }

        if (IsMultiSubscriber(rowView))
        {
            return null;
        }

        int bounces = GetBouncesFromRow(rowView);

        return UniGridFunctions.ColoredSpanYesNoReversed((mBounceLimit > 0 && bounces >= mBounceLimit) || (mBounceLimit == 0 && bounces == int.MaxValue));
    }


    /// <summary>
    /// Returns number of bounces or nothing according to subscriber's bounce info.
    /// </summary>
    private string GetBounces(DataRowView rowView)
    {
        // Do not handle if bounce email monitoring is not available
        if (!mBounceInfoAvailable)
        {
            return null;
        }

        int bounces = GetBouncesFromRow(rowView);

        if (bounces == 0 || bounces == int.MaxValue || IsMultiSubscriber(rowView))
        {
            return null;
        }

        return bounces.ToString();
    }


    /// <summary>
    /// Checks if the user has permission to manage subscribers.
    /// </summary>
    private static void CheckAuthorization()
    {
        if (!MembershipContext.AuthenticatedUser.IsAuthorizedPerResource("cms.newsletter", "managesubscribers"))
        {
            RedirectToAccessDenied("cms.newsletter", "managesubscribers");
        }
    }


    /// <summary>
    /// Checkbox 'Require double opt-in' state changed.
    /// </summary>
    protected void chkRequireOptIn_CheckedChanged(object sender, EventArgs e)
    {
        if (chkRequireOptIn.Checked)
        {
            chkSendConfirmation.Enabled = false;
            chkSendConfirmation.Checked = false;
        }
        else
        {
            chkSendConfirmation.Enabled = true;
        }
    }


    /// <summary>
    /// Roles control items changed event.
    /// </summary>
    protected void RolesSelector_OnItemsSelected(object sender, EventArgs e)
    {
        // Check permissions
        CheckAuthorization();

        int siteId = SiteContext.CurrentSiteID;

        // Get new items from selector
        string newValues = ValidationHelper.GetString(selectRole.Value, null);

        // Get added items
        string[] newItems = newValues.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
        foreach (string item in newItems)
        {
            // Check limited number of subscribers
            if (!SubscriberInfoProvider.LicenseVersionCheck(RequestContext.CurrentDomain, FeatureEnum.Subscribers, ObjectActionEnum.Insert))
            {
                ShowError(GetString("licenselimitations.subscribers.errormultiple"));
                break;
            }

            int roleID = ValidationHelper.GetInteger(item, 0);

            // Get subscriber
            SubscriberInfo subscriber = SubscriberInfoProvider.GetSubscriberInfo(RoleInfo.OBJECT_TYPE, roleID, siteId);
            if (subscriber == null)
            {
                // Get role info and copy display name to new subscriber
                RoleInfo ri = RoleInfoProvider.GetRoleInfo(roleID);
                if ((ri == null) || (ri.SiteID != siteId))
                {
                    continue;
                }

                // Create new subscriber of role type
                subscriber = new SubscriberInfo();
                subscriber.SubscriberFirstName = ri.DisplayName;
                // Full name consists of "role " and role display name
                subscriber.SubscriberFullName = new SubscriberFullNameFormater().GetRoleSubscriberName(ri.DisplayName);
                subscriber.SubscriberSiteID = siteId;
                subscriber.SubscriberType = RoleInfo.OBJECT_TYPE;
                subscriber.SubscriberRelatedID = roleID;

                CheckPermissionsForSubscriber(subscriber);

                SubscriberInfoProvider.SetSubscriberInfo(subscriber);
            }

            if (!mSubscriptionService.IsSubscribed(subscriber.SubscriberID, mNewsletter.NewsletterID))
            {
                mSubscriptionService.Subscribe(subscriber.SubscriberID, mNewsletter.NewsletterID, new SubscribeSettings()
                {
                    SendConfirmationEmail = chkSendConfirmation.Checked,
                    RequireOptIn = false,
                    RemoveAlsoUnsubscriptionFromAllNewsletters = false,
                });
            }
        }

        selectRole.Value = null;
        UniGridSubscribers.ReloadData();
        pnlUpdate.Update();
    }


    /// <summary>
    /// User control items changed event.
    /// </summary>
    protected void UserSelector_OnItemsSelected(object sender, EventArgs e)
    {
        // Check permissions
        CheckAuthorization();

        int siteId = SiteContext.CurrentSiteID;

        // Get new items from selector
        string newValues = ValidationHelper.GetString(selectUser.Value, null);

        string[] newItems = newValues.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
        foreach (string item in newItems)
        {
            // Check limited number of subscribers
            if (!SubscriberInfoProvider.LicenseVersionCheck(RequestContext.CurrentDomain, FeatureEnum.Subscribers, ObjectActionEnum.Insert))
            {
                ShowError(GetString("licenselimitations.subscribers.errormultiple"));
                break;
            }

            int userID = ValidationHelper.GetInteger(item, 0);

            // Get subscriber
            SubscriberInfo subscriber = SubscriberInfoProvider.GetSubscriberInfo(UserInfo.OBJECT_TYPE, userID, siteId);
            if (subscriber == null)
            {
                // Get user info
                UserInfo ui = UserInfoProvider.GetUserInfo(userID);
                if (ui == null)
                {
                    continue;
                }

                // Create new subscriber of user type
                subscriber = new SubscriberInfo();
                subscriber.SubscriberFirstName = ui.FullName;
                subscriber.SubscriberFullName = new SubscriberFullNameFormater().GetUserSubscriberName(ui.FullName);
                subscriber.SubscriberSiteID = siteId;
                subscriber.SubscriberType = UserInfo.OBJECT_TYPE;
                subscriber.SubscriberRelatedID = userID;

                CheckPermissionsForSubscriber(subscriber);

                SubscriberInfoProvider.SetSubscriberInfo(subscriber);
            }

            if (!mSubscriptionService.IsSubscribed(subscriber.SubscriberID, mNewsletter.NewsletterID))
            {
                mSubscriptionService.Subscribe(subscriber.SubscriberID, mNewsletter.NewsletterID, new SubscribeSettings()
                {
                    SendConfirmationEmail = chkSendConfirmation.Checked,
                    RequireOptIn = chkRequireOptIn.Checked,
                    RemoveAlsoUnsubscriptionFromAllNewsletters = false,
                });
            }
        }

        selectUser.Value = null;
        UniGridSubscribers.ReloadData();
        pnlUpdate.Update();
    }


    /// <summary>
    /// Checks whether the current user has permissions to add the subscriber. If not, redirect to access denied page.
    /// </summary>
    /// <param name="subscriber">Subscriber to be checked</param>
    private void CheckPermissionsForSubscriber(SubscriberInfo subscriber)
    {
        if (!subscriber.CheckPermissions(PermissionsEnum.Modify, CurrentSiteName, CurrentUser))
        {
            RedirectToAccessDenied(ModuleName.NEWSLETTER, "ManageSubscribers");
        }
    }


    /// <summary>
    /// Subscriber control items changed event.
    /// </summary>
    protected void SubscriberSelector_OnItemsSelected(object sender, EventArgs e)
    {
        // Check permissions
        CheckAuthorization();

        // Get new items from selector
        string newValues = ValidationHelper.GetString(selectSubscriber.Value, null);

        string[] newItems = newValues.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

        // Add all new items to site
        foreach (string item in newItems)
        {
            int subscriberID = ValidationHelper.GetInteger(item, 0);

            // Get subscriber
            SubscriberInfo subscriber = SubscriberInfoProvider.GetSubscriberInfo(subscriberID);

            // If subscriber exists and is not subscribed, subscribe him
            if (subscriber != null)
            {
                CheckPermissionsForSubscriber(subscriber);

                if (!mSubscriptionService.IsSubscribed(subscriber.SubscriberID, mNewsletter.NewsletterID))
                {
                    mSubscriptionService.Subscribe(subscriber.SubscriberID, mNewsletter.NewsletterID, new SubscribeSettings()
                    {
                        SendConfirmationEmail = chkSendConfirmation.Checked,
                        RequireOptIn = chkRequireOptIn.Checked,
                        RemoveAlsoUnsubscriptionFromAllNewsletters = false,
                    });
                }
            }
        }

        selectSubscriber.Value = null;
        UniGridSubscribers.ReloadData();
        pnlUpdate.Update();
    }


    /// <summary>
    /// Contact group items selected event handler.
    /// </summary>
    protected void CGSelector_OnItemsSelected(object sender, EventArgs e)
    {
        // Check permissions
        CheckAuthorization();

        if (cgSelector == null)
        {
            return;
        }
        int siteId = SiteContext.CurrentSiteID;

        // Get new items from selector
        string newValues = ValidationHelper.GetString(cgSelector.Value, null);

        // Get added items
        string[] newItems = newValues.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
        foreach (string item in newItems)
        {
            // Check limited number of subscribers
            if (!SubscriberInfoProvider.LicenseVersionCheck(RequestContext.CurrentDomain, FeatureEnum.Subscribers, ObjectActionEnum.Insert))
            {
                ShowError(GetString("licenselimitations.subscribers.errormultiple"));
                break;
            }

            // Get group ID
            int groupID = ValidationHelper.GetInteger(item, 0);

            // Try to get subscriber
            SubscriberInfo subscriber = SubscriberInfoProvider.GetSubscriberInfo(PredefinedObjectType.CONTACTGROUP, groupID, siteId);
            if (subscriber == null)
            {
                // Get contact group display name
                string cgName = ModuleCommands.OnlineMarketingGetContactGroupName(groupID);
                if (string.IsNullOrEmpty(cgName))
                {
                    continue;
                }

                // Create new subscriber of contact group type
                subscriber = new SubscriberInfo();
                subscriber.SubscriberFirstName = cgName;
                // Full name consists of "contact group " and display name
                subscriber.SubscriberFullName = new SubscriberFullNameFormater().GetContactGroupSubscriberName(cgName);
                subscriber.SubscriberSiteID = siteId;
                subscriber.SubscriberType = PredefinedObjectType.CONTACTGROUP;
                subscriber.SubscriberRelatedID = groupID;

                CheckPermissionsForSubscriber(subscriber);

                SubscriberInfoProvider.SetSubscriberInfo(subscriber);
            }

            if (!mSubscriptionService.IsSubscribed(subscriber.SubscriberID, mNewsletter.NewsletterID))
            {
                mSubscriptionService.Subscribe(subscriber.SubscriberID, mNewsletter.NewsletterID, new SubscribeSettings()
                {
                    SendConfirmationEmail = chkSendConfirmation.Checked,
                    RequireOptIn = false,
                    RemoveAlsoUnsubscriptionFromAllNewsletters = false,
                });
            }
        }

        cgSelector.Value = null;
        UniGridSubscribers.ReloadData();
        pnlUpdate.Update();
    }


    /// <summary>
    /// Contact items selected event handler.
    /// </summary>
    protected void ContactSelector_OnItemsSelected(object sender, EventArgs e)
    {
        // Check permissions
        CheckAuthorization();

        if (cSelector == null)
        {
            return;
        }
        int siteId = SiteContext.CurrentSiteID;

        // Get new items from selector
        string newValues = ValidationHelper.GetString(cSelector.Value, null);

        // Get added items
        string[] newItems = newValues.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
        foreach (string item in newItems)
        {
            // Check limited number of subscribers
            if (!SubscriberInfoProvider.LicenseVersionCheck(RequestContext.CurrentDomain, FeatureEnum.Subscribers, ObjectActionEnum.Insert))
            {
                ShowError(GetString("licenselimitations.subscribers.errormultiple"));
                break;
            }

            // Get contact ID
            int contactID = ValidationHelper.GetInteger(item, 0);

            // Try to get subscriber
            SubscriberInfo subscriber = SubscriberInfoProvider.GetSubscriberInfo(PredefinedObjectType.CONTACT, contactID, siteId);
            if (subscriber == null)
            {
                // Get contact's info
                DataSet contactData = ModuleCommands.OnlineMarketingGetContactForNewsletters(contactID, "ContactFirstName,ContactMiddleName,ContactLastName,ContactEmail");
                if (DataHelper.DataSourceIsEmpty(contactData))
                {
                    continue;
                }

                string firstName = ValidationHelper.GetString(contactData.Tables[0].Rows[0]["ContactFirstName"], string.Empty);
                string lastName = ValidationHelper.GetString(contactData.Tables[0].Rows[0]["ContactLastName"], string.Empty);
                string middleName = ValidationHelper.GetString(contactData.Tables[0].Rows[0]["ContactMiddleName"], string.Empty);
                string email = ValidationHelper.GetString(contactData.Tables[0].Rows[0]["ContactEmail"], string.Empty);

                // Create new subscriber of contact type
                subscriber = new SubscriberInfo();
                subscriber.SubscriberFirstName = firstName;
                subscriber.SubscriberLastName = lastName;
                subscriber.SubscriberEmail = email;
                subscriber.SubscriberFullName = new SubscriberFullNameFormater().GetContactSubscriberName(firstName, middleName, lastName);
                subscriber.SubscriberSiteID = siteId;
                subscriber.SubscriberType = PredefinedObjectType.CONTACT;
                subscriber.SubscriberRelatedID = contactID;

                CheckPermissionsForSubscriber(subscriber);

                SubscriberInfoProvider.SetSubscriberInfo(subscriber);
            }

            if (!mSubscriptionService.IsSubscribed(subscriber.SubscriberID, mNewsletter.NewsletterID))
            {
                mSubscriptionService.Subscribe(subscriber.SubscriberID, mNewsletter.NewsletterID, new SubscribeSettings()
                {
                    SendConfirmationEmail = chkSendConfirmation.Checked,
                    RequireOptIn = chkRequireOptIn.Checked,
                    RemoveAlsoUnsubscriptionFromAllNewsletters = false,
                });
            }
        }

        cSelector.Value = null;
        UniGridSubscribers.ReloadData();
        pnlUpdate.Update();
    }


    /// <summary>
    /// Persona items selected event handler.
    /// </summary>
    protected void PersonaSelector_OnItemsSelected(object sender, EventArgs e)
    {
        // Check permissions
        CheckAuthorization();

        if (personaSelector == null)
        {
            return;
        }

        int siteId = SiteContext.CurrentSiteID;

        // Get new items from selector
        string newValues = ValidationHelper.GetString(personaSelector.Value, null);

        // Get added items
        string[] newItems = newValues.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
        foreach (string item in newItems)
        {
            // Check limited number of subscribers
            if (!SubscriberInfoProvider.LicenseVersionCheck(RequestContext.CurrentDomain, FeatureEnum.Personas, ObjectActionEnum.Insert))
            {
                ShowError(GetString("licenselimitations.subscribers.errormultiple"));
                break;
            }

            // Get persona ID
            int personaID = ValidationHelper.GetInteger(item, 0);

            // Try to get subscriber
            SubscriberInfo subscriber = SubscriberInfoProvider.GetSubscriberInfo(PredefinedObjectType.PERSONA, personaID, siteId);
            if (subscriber == null)
            {
                // Get persona display name
                var persona = BaseAbstractInfoProvider.GetInfoById(PredefinedObjectType.PERSONA, personaID);
                string personaName = ValidationHelper.GetString(persona.GetValue("PersonaDisplayName"), string.Empty);
                if (string.IsNullOrEmpty(personaName))
                {
                    continue;
                }

                // Create new subscriber of persona type
                subscriber = new SubscriberInfo();

                subscriber.SubscriberFirstName = personaName;

                // Full name consists of "persona" and display name
                subscriber.SubscriberFullName = new SubscriberFullNameFormater().GetPersonaSubscriberName(personaName);

                subscriber.SubscriberSiteID = siteId;
                subscriber.SubscriberType = PredefinedObjectType.PERSONA;
                subscriber.SubscriberRelatedID = personaID;

                CheckPermissionsForSubscriber(subscriber);

                SubscriberInfoProvider.SetSubscriberInfo(subscriber);
            }

            if (!mSubscriptionService.IsSubscribed(subscriber.SubscriberID, mNewsletter.NewsletterID))
            {
                mSubscriptionService.Subscribe(subscriber.SubscriberID, mNewsletter.NewsletterID, new SubscribeSettings()
                {
                    SendConfirmationEmail = chkSendConfirmation.Checked,
                    RequireOptIn = false,
                    RemoveAlsoUnsubscriptionFromAllNewsletters = false,
                });
            }
        }

        personaSelector.Value = null;
        UniGridSubscribers.ReloadData();
        pnlUpdate.Update();
    }


    /// <summary>
    /// Returns if type of the subscriber is role, persona or contact group.
    /// </summary>
    private static bool IsMultiSubscriber(DataRowView rowView)
    {
        string type = DataHelper.GetStringValue(rowView.Row, "SubscriberType");
        return (type.EqualsCSafe(RoleInfo.OBJECT_TYPE, true) || type.EqualsCSafe(PredefinedObjectType.CONTACTGROUP, true) || type.EqualsCSafe(PredefinedObjectType.PERSONA));
    }


    /// <summary>
    /// Returns number of bounces of the subscriber.
    /// </summary>
    private static int GetBouncesFromRow(DataRowView rowView)
    {
        return DataHelper.GetIntValue(rowView.Row, "SubscriberBounces");
    }


    /// <summary>
    /// Handles multiple selector actions.
    /// </summary>
    protected void btnOk_Clicked(object sender, EventArgs e)
    {
        // Check permissions
        CheckAuthorization();

        if (drpActions.SelectedValue != SELECT)
        {
            // Go through all selected items
            if (UniGridSubscribers.SelectedItems.Count != 0)
            {
                foreach (string subscriberId in UniGridSubscribers.SelectedItems)
                {
                    int subscriberIdInt = ValidationHelper.GetInteger(subscriberId, 0);

                    DoSubscriberAction(subscriberIdInt, drpActions.SelectedValue);
                }
            }
        }
        UniGridSubscribers.ResetSelection();
        UniGridSubscribers.ReloadData();
    }


    /// <summary>
    /// Performs action on given subscriber.
    /// </summary>
    /// <param name="subscriberId">Id of subscriber</param>
    /// <param name="actionName">Name of action</param>
    private void DoSubscriberAction(int subscriberId, string actionName)
    {
        try
        {
            // Check manage subscribers permission
            var subscriber = SubscriberInfoProvider.GetSubscriberInfo(subscriberId);
            if (!subscriber.CheckPermissions(PermissionsEnum.Modify, SiteContext.CurrentSiteName, MembershipContext.AuthenticatedUser))
            {
                RedirectToAccessDenied(subscriber.TypeInfo.ModuleName, "ManageSubscribers");
            }

            Func<bool> subscriberIsUserAndIsDisabled = () =>
            {
                var user = GetSubscriberUser(subscriber);
                return ((user != null) && !user.UserEnabled);
            };

            switch (actionName.ToUpperInvariant())
            {
                // Remove subscription
                case REMOVE:
                    mSubscriptionService.RemoveSubscription(subscriberId, mNewsletter.NewsletterID, chkSendConfirmation.Checked);
                    break;

                // Approve subscription
                case APPROVE:
                    if (subscriberIsUserAndIsDisabled())
                    {
                        return;
                    }
                    SubscriberNewsletterInfoProvider.ApproveSubscription(subscriberId, mNewsletter.NewsletterID);
                    break;

                // Block selected subscriber
                case BLOCK:
                    SubscriberInfoProvider.BlockSubscriber(subscriberId);
                    break;

                // Un-block selected subscriber
                case UNBLOCK:
                    SubscriberInfoProvider.UnblockSubscriber(subscriberId);
                    break;
            }
        }
        catch (Exception exception)
        {
            LogAndShowError("Newsletter subscriber", "NEWSLETTERS", exception);
        }
    }

    #endregion
}