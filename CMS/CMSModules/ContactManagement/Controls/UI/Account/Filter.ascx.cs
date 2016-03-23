using System;
using System.Web.UI.WebControls;

using CMS.DataEngine;
using CMS.Globalization;
using CMS.Helpers;
using CMS.Membership;
using CMS.OnlineMarketing;
using CMS.SiteProvider;
using CMS.UIControls;
using CMS.Controls;

using TextSimpleFilter = CMSAdminControls_UI_UniGrid_Filters_TextSimpleFilter;

public partial class CMSModules_ContactManagement_Controls_UI_Account_Filter : CMSAbstractBaseFilterControl
{
    #region "Variables"

    private int mSiteId = -1;
    private int mSelectedSiteID = -1;

    #endregion


    #region "Public properties"

    /// <summary>
    /// Gets or sets the site ID for which the events should be filtered.
    /// </summary>
    public override int SiteID
    {
        get
        {
            return mSiteId;
        }
        set
        {
            mSiteId = value;
            fltAccountStatus.SiteID = value;

            if (value == UniSelector.US_ALL_RECORDS)
            {
                fltAccountStatus.DisplayAll = true;
            }
            else if (value == UniSelector.US_GLOBAL_AND_SITE_RECORD)
            {
                fltAccountStatus.DisplaySiteOrGlobal = true;
                fltAccountStatus.SiteID = SiteContext.CurrentSiteID;
            }
        }
    }


    /// <summary>
    /// Indicates whether to show global statuses or not.
    /// </summary>
    public bool ShowGlobalStatuses
    {
        get
        {
            return fltAccountStatus.DisplaySiteOrGlobal;
        }
        set
        {
            fltAccountStatus.DisplaySiteOrGlobal = value;
        }
    }


    /// <summary>
    /// Selected site ID.
    /// </summary>
    public int SelectedSiteID
    {
        get
        {
            mSelectedSiteID = UniSelector.US_ALL_RECORDS;

            if (siteSelector.Visible)
            {
                mSelectedSiteID = siteSelector.SiteID;
            }
            else if (siteOrGlobalSelector.Visible)
            {
                mSelectedSiteID = siteOrGlobalSelector.SiteID;
            }

            if (mSelectedSiteID == 0)
            {
                mSelectedSiteID = UniSelector.US_ALL_RECORDS;
            }

            return mSelectedSiteID;
        }
    }


    /// <summary>
    /// Gets the where condition created using filtered parameters.
    /// </summary>
    public override string WhereCondition
    {
        get
        {
            return GenerateWhereCondition();
        }
    }


    /// <summary>
    /// Indicates if AccountStatus filter should be displayed even if site is not specified.
    /// </summary>
    public bool DisplayAccountStatus
    {
        get;
        set;
    }


    /// <summary>
    /// Indicates if filter is in advanced mode.
    /// </summary>
    protected bool IsAdvancedMode
    {
        get
        {
            return ValidationHelper.GetBoolean(ViewState["IsAdvancedMode"], false);
        }
        set
        {
            ViewState["IsAdvancedMode"] = value;
        }
    }


    /// <summary>
    /// Gets button used to toggle filter's advanced mode.
    /// </summary>
    public override IButtonControl ToggleAdvancedModeButton
    {
        get
        {
            return IsAdvancedMode ? lnkShowSimpleFilter : lnkShowAdvancedFilter;
        }
    }


    /// <summary>
    /// Indicates if  filter is used on live site or in UI.
    /// </summary>
    public override bool IsLiveSite
    {
        get
        {
            return base.IsLiveSite;
        }
        set
        {
            base.IsLiveSite = value;
            fltAccountStatus.IsLiveSite = value;
        }
    }


    /// <summary>
    /// Returns TRUE if displaying both merged and not merged accounts.
    /// </summary>
    public bool DisplayingAll
    {
        get
        {
            return chkMerged.Checked;
        }
    }


    /// <summary>
    /// Indicates if merging filter should be hidden.
    /// </summary>
    public bool HideMergedFilter
    {
        get
        {
            return !plcMerged.Visible;
        }
        set
        {
            plcMerged.Visible = !value;
        }
    }


    /// <summary>
    /// Indicates if filter should return only not merged accounts. Otherwise filter returns only merged accounts.
    /// Applies only when filter is hidden.
    /// </summary>
    public bool NotMerged
    {
        get;
        set;
    }


    /// <summary>
    /// Indicates if siteselector should be displayed.
    /// </summary>
    public bool DisplaySiteSelector
    {
        get
        {
            return siteSelector.Visible;
        }
        set
        {
            if (value)
            {
                plcSite.Visible = true;
            }
            siteSelector.Visible = value;
        }
    }


    /// <summary>
    /// Indicates if 'Site or global selector' should be displayed.
    /// </summary>
    public bool DisplayGlobalOrSiteSelector
    {
        get
        {
            return siteOrGlobalSelector.Visible;
        }
        set
        {
            if (value)
            {
                plcSite.Visible = true;
            }
            siteOrGlobalSelector.Visible = value;
        }
    }


    /// <summary>
    /// Gets or sets if all accounts merged into global accounts should be hide.
    /// </summary>
    public bool HideMergedIntoGlobal
    {
        get;
        set;
    }


    /// <summary>
    /// Gets or sets value that indicates whether "show children" checkbox should be visible.
    /// </summary>
    public bool ShowChildren
    {
        get;
        set;
    }


    /// <summary>
    /// Returns true if all child contacts will be shown.
    /// </summary>
    public bool ChildrenSelected
    {
        get
        {
            return chkChildren.Checked;
        }
    }


    /// <summary>
    /// When true, site id is not added to where clause generated by filter.
    /// </summary>
    public bool DisableGeneratingSiteClause
    {
        get;
        set;
    }

    #endregion


    #region "Page methods"

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        fltPhone.Columns = new[] { 
            "AccountPhone", 
            "AccountFax" 
        };
        fltContactName.Columns = new[] { 
            "PrimaryContactFullName",
            "SecondaryContactFullName"
        };
        btnReset.Text = GetString("general.reset");
        btnReset.Click += btnReset_Click;
        btnSearch.Click += btnSearch_Click;
    }


    /// <summary>
    /// Resets the associated UniGrid control.
    /// </summary>
    protected void btnReset_Click(object sender, EventArgs e)
    {
        UniGrid grid = FilteredControl as UniGrid;
        if (grid != null)
        {
            grid.FilterIsSet = true;
            grid.Reset();
        }
    }


    /// <summary>
    /// Applies filter on associated UniGrid control.
    /// </summary>
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        UniGrid grid = FilteredControl as UniGrid;
        if (grid != null)
        {
            grid.ApplyFilter(sender, e);
        }
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        InitializeForm();
        plcAdvancedSearch.Visible = IsAdvancedMode;
        plcChildren.Visible = ShowChildren;
    }

    #endregion


    #region "UI methods"

    /// <summary>
    /// Shows/hides all elements for advanced or simple mode.
    /// </summary>
    private void ShowFilterElements(bool showAdvanced)
    {
        plcAdvancedSearch.Visible = showAdvanced;
        pnlAdvanced.Visible = showAdvanced;
        pnlSimple.Visible = !showAdvanced;
    }


    /// <summary>
    /// Initializes the layout of the form.
    /// </summary>
    private void InitializeForm()
    {
        // General UI
        siteSelector.AdditionalDropDownCSSClass = "DropDownFieldFilter";
        fltAccountStatus.AdditionalDropDownCSSClass = "DropDownFieldFilter";
        lnkShowAdvancedFilter.Text = GetString("general.displayadvancedfilter");
        lnkShowSimpleFilter.Text = GetString("general.displaysimplefilter");
        plcAdvancedSearch.Visible = IsAdvancedMode;
        pnlAdvanced.Visible = IsAdvancedMode;
        pnlSimple.Visible = !IsAdvancedMode;
    }


    /// <summary>
    /// Sets the advanced mode.
    /// </summary>
    protected void lnkShowAdvancedFilter_Click(object sender, EventArgs e)
    {
        IsAdvancedMode = true;
        ShowFilterElements(true);
    }


    /// <summary>
    /// Sets the simple mode.
    /// </summary>
    protected void lnkShowSimpleFilter_Click(object sender, EventArgs e)
    {
        IsAdvancedMode = false;
        ShowFilterElements(false);
    }

    #endregion


    #region "Search methods - where condition"

    /// <summary>
    /// Generates complete filter where condition.
    /// </summary>    
    private string GenerateWhereCondition()
    {
        var whereCondition = new WhereCondition();

        // Create WHERE condition for basic filter
        int contactStatus = ValidationHelper.GetInteger(fltAccountStatus.Value, -1);
        if (fltAccountStatus.Value == null)
        {
            whereCondition = whereCondition.WhereNull("AccountStatusID");
        }
        else if (contactStatus > 0)
        {
            whereCondition = whereCondition.WhereEquals("AccountStatusID", contactStatus);
        }

        whereCondition = whereCondition
            .Where(fltName.GetCondition())
            .Where(fltEmail.GetCondition())
            .Where(fltContactName.GetCondition());

        if (IsAdvancedMode)
        {
            whereCondition = whereCondition
                .Where(fltCity.GetCondition())
                .Where(fltPhone.GetCondition())
                .Where(fltCreated.GetCondition())
                .Where(GetOwnerCondition(fltOwner))
                .Where(GetCountryCondition(fltCountry))
                .Where(GetStateCondition(fltState));
        }

        // When "merged/not merged" filter is hidden
        if ((HideMergedFilter && NotMerged) ||
            (IsAdvancedMode && !HideMergedFilter && !chkMerged.Checked) ||
            (!IsAdvancedMode && !HideMergedFilter && !NotMerged))
        {
            whereCondition = whereCondition
               .Where(
                   new WhereCondition(
                       new WhereCondition()
                           .WhereNull("AccountMergedWithAccountID")
                           .WhereGreaterOrEquals("AccountSiteID", 0)
                       )
                       .Or(
                           new WhereCondition()
                               .WhereNull("AccountGlobalAccountID")
                               .WhereNull("AccountSiteID")
                       ));
        }

        // Hide accounts merged into global account when displaying list of available accounts for global account
        if (HideMergedIntoGlobal)
        {
            whereCondition = whereCondition.WhereNull("AccountGlobalAccountID");
        }

        if (!DisableGeneratingSiteClause)
        {
            // Filter current account's site
            if (!plcSite.Visible)
            {
                // Filter site objects
                if (SiteID > 0)
                {
                    whereCondition = whereCondition.WhereEquals("AccountSiteID", SiteID);
                }
                // Filter only global objects
                else if (SiteID == UniSelector.US_GLOBAL_RECORD)
                {
                    whereCondition = whereCondition.WhereNull("AccountSiteID");
                }
            }
            // Filter by site filter
            else
            {
                // Only global objects
                if (SelectedSiteID == UniSelector.US_GLOBAL_RECORD)
                {
                    whereCondition = whereCondition.WhereNull("AccountSiteID");
                }
                // Global and site objects
                else if (SelectedSiteID == UniSelector.US_GLOBAL_AND_SITE_RECORD)
                {
                    whereCondition = whereCondition.WhereEqualsOrNull("AccountSiteID", SiteContext.CurrentSiteID);
                }
                // Site objects
                else if (SelectedSiteID != UniSelector.US_ALL_RECORDS)
                {
                    whereCondition = whereCondition.WhereEquals("AccountSiteID", mSelectedSiteID);
                }
            }
        }

        return whereCondition.ToString(true);
    }

    #endregion


    #region "Additional filter conditions"

    /// <summary>
    /// Gets Where condition for filtering by the country. When using separated database, materializes the nested query on the main DB.
    /// </summary>
    private string GetCountryCondition(TextSimpleFilter filter)
    {
        string originalQuery = filter.WhereCondition;
        if (String.IsNullOrEmpty(originalQuery))
        {
            return string.Empty;
        }

        // Query with AccountInfo context has to be used in order to be able to determine DB context of the query (otherwise the materialization would not perform).
        var query = AccountInfoProvider.GetAccounts()
            .WhereIn("AccountCountryID", CountryInfoProvider
                .GetCountries()
                .Where(originalQuery)
                .Column(CountryInfo.TYPEINFO.IDColumn)
            );

        if (filter.FilterOperator == WhereBuilder.NOT_LIKE || filter.FilterOperator == WhereBuilder.NOT_EQUAL)
        {
            query = query.Or(new WhereCondition().WhereNull("AccountCountryID"));
        }

        query.EnsureParameters();
        return query.Parameters.Expand(query.WhereCondition);
    }


    /// <summary>
    /// Gets Where condition for filtering by the user. When using separated database, materializes the nested query on the other DB.
    /// </summary>
    private string GetOwnerCondition(TextSimpleFilter filter)
    {
        string originalQuery = filter.WhereCondition;
        if (String.IsNullOrEmpty(originalQuery))
        {
            return string.Empty;
        }

        // Query with AccountInfo context has to be used in order to be able to determine DB context of the query (otherwise the materialization would not perform).
        var query = AccountInfoProvider.GetAccounts()
            .WhereIn("AccountOwnerUserID", UserInfoProvider
                .GetUsers()
                .Where(originalQuery)
                .Column(UserInfo.TYPEINFO.IDColumn)
            );

        if (filter.FilterOperator == WhereBuilder.NOT_LIKE || filter.FilterOperator == WhereBuilder.NOT_EQUAL)
        {
            query = query.Or(new WhereCondition().WhereNull("AccountOwnerUserID"));
        }

        query.EnsureParameters();
        return query.Parameters.Expand(query.WhereCondition);
    }


    /// <summary>
    /// Gets Where condition for filtering by the state. When using separated database, materializes the nested query on the other DB.
    /// </summary>
    private string GetStateCondition(TextSimpleFilter filter)
    {
        string originalQuery = filter.WhereCondition;
        if (String.IsNullOrEmpty(originalQuery))
        {
            return string.Empty;
        }

        // Query with AccountInfo context has to be used in order to be able to determine DB context of the query (otherwise the materialization would not perform).
        var query = AccountInfoProvider.GetAccounts()
            .WhereIn("AccountStateID", StateInfoProvider
                .GetStates()
                .Where(originalQuery)
                .Column(StateInfo.TYPEINFO.IDColumn)
            );

        if (filter.FilterOperator == WhereBuilder.NOT_LIKE || filter.FilterOperator == WhereBuilder.NOT_EQUAL)
        {
            query = query.Or(new WhereCondition().WhereNull("AccountStateID"));
        }

        query.EnsureParameters();
        return query.Parameters.Expand(query.WhereCondition);
    }

    #endregion


    #region "State management"

    /// <summary>
    /// Resets filter to the default state.
    /// </summary>
    public override void ResetFilter()
    {
        fltAccountStatus.Value = UniSelector.US_ALL_RECORDS;
        fltCity.ResetFilter();
        fltContactName.ResetFilter();
        fltCountry.ResetFilter();
        fltCreated.Clear();
        fltEmail.ResetFilter();
        fltName.ResetFilter();
        fltOwner.ResetFilter();
        fltPhone.ResetFilter();
        fltState.ResetFilter();
        chkChildren.Checked = false;
        chkMerged.Checked = false;
    }


    /// <summary>
    /// Stores filter state to the specified object.
    /// </summary>
    /// <param name="state">The object that holds the filter state.</param>
    public override void StoreFilterState(FilterState state)
    {
        state.AddValue("ShowAll", chkMerged.Checked);
        state.AddValue("AdvancedMode", IsAdvancedMode);
        state.AddValue("Status", fltAccountStatus.Value);
        state.AddValue("ToTime", fltCreated.ValueToTime);
        state.AddValue("FromTime", fltCreated.ValueFromTime);
        base.StoreFilterState(state);
    }


    /// <summary>
    /// Restores filter state from the specified object.
    /// </summary>
    /// <param name="state">The object that holds the filter state.</param>
    public override void RestoreFilterState(FilterState state)
    {
        base.RestoreFilterState(state);
        chkMerged.Checked = state.GetBoolean("ShowAll");
        IsAdvancedMode = state.GetBoolean("AdvancedMode");
        fltAccountStatus.Value = state.GetString("Status");
        fltCreated.ValueFromTime = state.GetDateTime("FromTime");
        fltCreated.ValueToTime = state.GetDateTime("ToTime");
    }

    #endregion
}