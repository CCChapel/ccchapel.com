using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

using CMS.Base;
using CMS.UIControls;
using CMS.SiteProvider;
using CMS.Membership;
using CMS.Helpers;
using CMS.DataEngine;
using CMS.Modules;

public partial class CMSAdminControls_Basic_DisabledModuleInfo : CMSUserControl
{
    #region "Variables"

    private String mSettingsKeys = String.Empty;
    private String mConfigKeys = String.Empty;

    private Panel mParentPanel;
    private String mInfoText = String.Empty;

    private bool settingsChecked = false;
    private bool mShowButtons = true;

    private DisabledModuleScope mKeyScope = DisabledModuleScope.Both;
    private bool mReloadUIWhenModuleEnabled = true;

    private string mInfoTextResourceString = "ui.moduledisabled.general";

    #endregion


    #region "Properties"

    /// <summary>
    /// Info text to show. This text has precedence over <see cref="InfoTextResourceString"/>.
    /// </summary>
    /// <remarks>
    /// Unlike the <see cref="InfoTextResourceString"/>, this text is used as it is (i.e. no format items substitution is performed).
    /// </remarks>
    public String InfoText
    {
        get
        {
            return mInfoText;
        }
        set
        {
            mInfoText = value;
        }
    }



    /// <summary>
    /// Info message resource string. Resource string is used only when <see cref="InfoText"/> is not set (i.e. null or empty).
    /// If this property and <see cref="InfoText"/> is not set, default message The "XY" setting is currently disabled, is used.
    /// Format string is expected in resource string value and it includes setting's display name.
    /// </summary>
    public string InfoTextResourceString
    {
        get
        {
            return mInfoTextResourceString;
        }
        set
        {
            mInfoTextResourceString = value;
        }
    }


    /// <summary>
    /// Scope of key check. It's used for 'SettingsKeys' property.
    /// </summary>
    public DisabledModuleScope KeyScope
    {
        get
        {
            return mKeyScope;
        }
        set
        {
            // Settings are not checked with new key scope
            settingsChecked = false;
            mKeyScope = value;
        }
    }


    /// <summary>
    /// Get/set sitename. Store sitename to viewstate - in case of save, before sitename is known.
    /// </summary>
    public string SiteName
    {
        get
        {
            var siteName = ValidationHelper.GetString(ViewState["SiteName"], null);
            return String.IsNullOrEmpty(siteName) ? SiteContext.CurrentSiteName : siteName;
        }
        set
        {
            ViewState["SiteName"] = value;
        }
    }


    /// <summary>
    /// Settings keys to check delimited by ';'
    /// </summary>
    public String SettingsKeys
    {
        get
        {
            return mSettingsKeys;
        }
        set
        {
            // Settings are not checked with new key scope
            settingsChecked = false;
            mSettingsKeys = value;
        }
    }


    /// <summary>
    /// Web.config keys to check delimited by ';'. If enabled through web.config keys, does not check the settings and considered enabled.
    /// </summary>
    public String ConfigKeys
    {
        get
        {
            return mConfigKeys;
        }
        set
        {
            mConfigKeys = value;
        }
    }


    /// <summary>
    /// Parent panel, used for hiding info row if no module is disabled. Is automatically hidden when no module is disabled.
    /// </summary>
    public Panel ParentPanel
    {
        get
        {
            return mParentPanel;
        }
        set
        {
            mParentPanel = value;
        }
    }


    /// <summary>
    /// This value contains result of settings checking
    /// </summary>
    public bool SettingsEnabled
    {
        get;
        protected set;
    }


    /// <summary>
    /// Indicates whether show "site" and "global" buttons
    /// </summary>
    public bool ShowButtons
    {
        get
        {
            return mShowButtons;
        }
        set
        {
            mShowButtons = value;
        }
    }


    /// <summary>
    /// Indicates if UI should be refreshed by reload after enabling module
    /// </summary>
    public bool ReloadUIWhenModuleEnabled
    {
        get
        {
            return mReloadUIWhenModuleEnabled;
        }
        set
        {
            mReloadUIWhenModuleEnabled = value;
        }
    }


    /// <summary>
    /// Sets the text for global button.
    /// </summary>
    public string GlobalButtonText
    {
        get;
        set;
    }


    /// <summary>
    /// Sets the text for site button.
    /// </summary>
    public string SiteButtonText
    {
        get;
        set;
    }


    /// <summary>
    /// Returns settings keys in list.
    /// </summary>
    private List<string> SettingsList
    {
        get 
        { 
            return String.IsNullOrEmpty(SettingsKeys) ?  new List<string>() : SettingsKeys.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList(); 
        }
    }


    /// <summary>
    /// Returns config keys in list.
    /// </summary>
    private List<string> ConfigKeysList
    {
        get 
        { 
            return String.IsNullOrEmpty(ConfigKeys) ? new List<string>() : ConfigKeys.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList(); 
        }
    }


    /// <summary>
    /// If true, settings are considered as checked if at least one setting is checked. This is used only for displaying general information.
    /// </summary>
    public bool AtLeastOne
    {
        get;
        set;
    }

    #endregion


    #region "Methods"

    protected void Page_Load(object sender, EventArgs e)
    {
        Visible = true;

        // Set text for link
        btnGlobal.Text = String.IsNullOrEmpty(GlobalButtonText) ? GetString("module.allowglobal") : GlobalButtonText;
        btnSite.Text = String.IsNullOrEmpty(SiteButtonText) ? GetString("module.allowsite") : SiteButtonText;
    }


    protected void btnGlobal_clicked(object sender, EventArgs ea)
    {
        Save(false);
    }


    protected void btnSiteOnly_clicked(object sender, EventArgs ea)
    {
        Save(true);
    }


    /// <summary>
    /// Check settings and return result.
    /// </summary>    
    public bool Check()
    {
        settingsChecked = true;

        SettingsEnabled = IsModuleEnabledByAppConfigKeys() || IsModuleEnabledBySettingKeys();
        return SettingsEnabled;
    }


    protected override void OnPreRender(EventArgs e)
    {
        if (!settingsChecked)
        {
            Check();
        }

        // Shows or hides disabled module info, depends on SettingEnabled property 
        DisplayErrorText();

        base.OnPreRender(e);
    }

    #endregion


    #region "Private helper methods"
    
    /// <summary>
    /// Generate info text for given setting key
    /// </summary>
    /// <param name="ski">Setting key object</param>
    private String GenerateInfoText(SettingsKeyInfo ski)
    {
        return String.Format(GetString(InfoTextResourceString), ResHelper.GetString(ski.KeyDisplayName));
    }


    /// <summary>
    /// Saves the changed settings 
    /// </summary>
    /// <param name="isSite">Indicates whether changed settings is global or site</param>
    private void Save(bool isSite)
    {
        // This action is permitted only for global administrators
        if (MembershipContext.AuthenticatedUser.IsGlobalAdministrator)
        {
            SettingsList.ForEach(key => EnableSettingByActualScope(key, isSite));

            // Reload UI if necessary
            if (ReloadUIWhenModuleEnabled)
            {
                URLHelper.Redirect(RequestContext.CurrentURL);
            }
        }
    }


    /// <summary>
    /// Checks config keys, if there are no config keys to check, false is returned.
    /// </summary>
    /// <returns>If all settings are true, true is returned. If one of the settings is false, false is returned.</returns>
    private bool IsModuleEnabledByAppConfigKeys()
    {
        if (AtLeastOne)
        {
            return ConfigKeysList.Any(key => ValidationHelper.GetBoolean(SettingsHelper.AppSettings[key], false));
        }

        return !String.IsNullOrEmpty(ConfigKeys) && !ConfigKeysList.Exists(key => !ValidationHelper.GetBoolean(SettingsHelper.AppSettings[key], false));
    }


    /// <summary>
    /// Checks all setting keys, if are enabled in actual scope.
    /// If no settings are specified, true is returned.
    /// </summary>
    /// <returns>If no settings are specified, true is returned, otherwise all settings have to be true in actual scope.</returns>
    private bool IsModuleEnabledBySettingKeys()
    {
        if (AtLeastOne)
        {
            return SettingsList.Any(key => IsSettingEnabledInActualScope(key));
        }

        return !SettingsList.Exists(key => !IsSettingEnabledInActualScope(key));
    }


    /// <summary>
    /// Shows or hides disabled module info, depends on SettingsEnabled property.
    /// </summary>
    private void DisplayErrorText()
    {
        // If module is disabled, then display disabled module info, otherwise hide
        if (ParentPanel != null)
        {
            ParentPanel.Visible = !SettingsEnabled;
        }
        Visible = !SettingsEnabled;
        lblText.Visible = !SettingsEnabled;

        SetButtonVisibility();

        // If settings enabled, info text is hidden, there is no point in generating info text
        if (SettingsEnabled)
        {
            return;
        }

        // If InfoText is set ignore everything else
        if (!String.IsNullOrEmpty(InfoText))
        { 
            lblText.Text += InfoText;
            return;
        }

        foreach (string key in SettingsList)
        {
            // If module disabled for given key, write text about given setting
            if (!IsSettingEnabledInActualScope(key))
            {
                string text = GenerateInfoText(SettingsKeyInfoProvider.GetSettingsKeyInfo(key));

                if (!String.IsNullOrEmpty(lblText.Text))
                {
                    lblText.Text += "<br />";
                }

                // Add new text to label
                lblText.Text += text;
            }
        }
    }


    /// <summary>
    /// Is setting enabled in actual scope of disabled module info check.
    /// </summary>
    /// <param name="key">Key to check.</param>
    /// <returns>
    /// Depend on scope, if scope is Both and site setting is true, true is returned, otherwise false.  
    /// If scope is Site, true is return if site setting is true.
    /// If scope i Global, true is return if global setting is true.
    /// If scope is CurrentSiteAndGlobal, current site setting and global setting are true, true is returned.
    /// If scope is AllSitesAndGlobal, all sites must have site setting true and global setting is true, true is returned.
    /// </returns>
    private bool IsSettingEnabledInActualScope(string key)
    {
        switch (KeyScope)
        {
            case DisabledModuleScope.Both:
            case DisabledModuleScope.Site: 
                return SettingsKeyInfoProvider.GetBoolValue(key, SiteName);

            case DisabledModuleScope.Global: 
                return SettingsKeyInfoProvider.GetBoolValue(key);

            case DisabledModuleScope.CurrentSiteAndGlobal: 
                return SettingsKeyInfoProvider.GetBoolValue(key, SiteName) && SettingsKeyInfoProvider.GetBoolValue(key);

            case DisabledModuleScope.AllSitesAndGlobal: 
                return CheckAllSitesSetting(key) && SettingsKeyInfoProvider.GetBoolValue(key);
        }

        return true;
    }


    /// <summary>
    /// Enables the setting for actual scope and by which button user has chosen.
    /// If scope is Both, and global button is pressed then global setting is set and site setting inherits, otherwise only site setting is set.
    /// If scope is Site, only for current site is setting set.
    /// If scope is Global, only global setting is set.
    /// If scope is CurrentSiteAndGlobal, current site's setting and global setting are set.
    /// If scope is AllSiteAndGlobal, all sites settings and global setting are set.
    /// </summary>
    /// <param name="isSite">Site button or global button.</param>
    /// <param name="key">Setting to enable.</param>
    private void EnableSettingByActualScope(string key, bool isSite)
    {
        // If scope is only global, then set only global setting
        if (KeyScope == DisabledModuleScope.Global)
        {
            SettingsKeyInfoProvider.SetGlobalValue(key, true);
            return;
        }

        // If setting is only global setting, set global setting
        var ski = SettingsKeyInfoProvider.GetSettingsKeyInfo(key);
        if (ski.KeyIsGlobal)
        {
            SettingsKeyInfoProvider.SetGlobalValue(key, true);
            return;
        }

        switch (KeyScope)
        {
            case DisabledModuleScope.Both:
                if (isSite)
                {
                    SettingsKeyInfoProvider.SetValue(key, SiteName, true);
                    break;
                }

                //If global button was pressed, set global setting, and inherit site setting
                SettingsKeyInfoProvider.SetGlobalValue(key, true);
                SettingsKeyInfoProvider.SetValue(key, SiteName, null);
                break;

            case DisabledModuleScope.Site:
                SettingsKeyInfoProvider.SetValue(key, SiteName, true);
                break;

            case DisabledModuleScope.CurrentSiteAndGlobal:
                SettingsKeyInfoProvider.SetGlobalValue(key, true);
                SettingsKeyInfoProvider.SetValue(key, SiteName, true);
                break;

            case DisabledModuleScope.AllSitesAndGlobal:
                SiteInfoProvider.GetSites().ForEachObject(site => SettingsKeyInfoProvider.SetValue(key, SiteName, true));
                SettingsKeyInfoProvider.SetGlobalValue(key, true);
                break;
        }
    }


    /// <summary>
    /// Sets the button visibility, if SettingsEnabled true, buttons will be hidden, otherwise sets visibility by current KeyScope.
    /// </summary>
    private void SetButtonVisibility()
    {
        btnSite.Visible = false;
        btnGlobal.Visible = false;

        if (SettingsEnabled)
        {
            return;
        }

        switch (KeyScope)
        {
            case DisabledModuleScope.Global:
            case DisabledModuleScope.CurrentSiteAndGlobal:
            case DisabledModuleScope.AllSitesAndGlobal:
                btnGlobal.Visible = true;
                break;

            case DisabledModuleScope.Site:
                btnSite.Visible = true;
                break;

            case DisabledModuleScope.Both:
                btnSite.Visible = true;
                btnGlobal.Visible = true;
                break;
        }

        if (!MembershipContext.AuthenticatedUser.IsGlobalAdministrator || !ShowButtons)
        {
            btnGlobal.Visible = false;
            btnSite.Visible = false;
        }

        if (btnGlobal.Visible && btnSite.Visible)
        {
            btnGlobal.CssClass = "DisabledModuleButtons";
        }
    }


    /// <summary>
    /// Check site settings of all sites.
    /// </summary>
    /// <param name="settingKey">Setting key to be checked for all sites.</param>
    /// <returns> Return true if all sites have that site setting set true. Otherwise returns false.</returns>
    private bool CheckAllSitesSetting(string settingKey)
    {
        return !SiteInfoProvider.GetSites().Any(site => !SettingsKeyInfoProvider.GetBoolValue(settingKey, site.SiteName));
    }

    #endregion
}


#region "DisabledModuleScopeEnum"

/// <summary>
/// Scope of disabled module key check
/// </summary>
public enum DisabledModuleScope : int
{
    /// <summary>
    /// If global btn is pressed, global setting is saved and site setting inherits.
    /// If site btn is pressed, only site setting is saved.
    /// Both buttons are shown.
    /// </summary>
    [EnumStringRepresentation("Both")]
    Both = 0,

    /// <summary>
    /// Only site button is shown, and enables only current site setting.
    /// </summary>
    [EnumStringRepresentation("Site")]
    Site = 1,

    /// <summary>
    /// Only global button is shown, and enables only global setting.
    /// </summary>
    [EnumStringRepresentation("Global")]
    Global = 2,

    /// <summary>
    /// Site setting for current site and global setting is set,
    /// Only one button is shown in disabled module info.
    /// This is used e.g. in staging in Objects tab, on All Objects node,
    /// where you check both current site setting (for site objects) and global setting (for global objects).
    /// </summary>
    [EnumStringRepresentation("CurrentSiteAndGlobal")]
    CurrentSiteAndGlobal = 3,

    /// <summary>
    /// Site settings for all sites  are set and global setting is set,
    /// Only one button is shown in disabled module info.
    /// This is used e.g. in banned IPs, when in dropdownlist is set All.
    /// </summary>
    [EnumStringRepresentation("AllSitesAndGlobal")]
    AllSitesAndGlobal = 4,
}

#endregion
