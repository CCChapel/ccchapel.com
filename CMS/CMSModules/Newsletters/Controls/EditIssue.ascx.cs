using System;
using System.Linq;
using System.Text;
using System.Web.UI;

using CMS.DataEngine;
using CMS.ExtendedControls;
using CMS.Helpers;
using CMS.LicenseProvider;
using CMS.MacroEngine;
using CMS.Newsletters;
using CMS.SiteProvider;
using CMS.Membership;
using CMS.UIControls;
using CMS.WebAnalytics;

public partial class CMSModules_Newsletters_Controls_EditIssue : CMSAdminControl
{
    #region "Variables"

    private bool mLoaded;
    private bool mValidated;
    private int mTemplateID;
    private const string DEFAULT_UTM_MEDIUM = "email";
    private NewsletterInfo mNewsletter;
    private bool mAreCampaignsAvailable;
    private CMSTextBox mUTMCampaignTextBox;

    #endregion


    #region "Properties"

    /// <summary>
    /// ID of newsletter issue that should be edited, required when editing an issue.
    /// </summary>
    public int IssueID
    {
        get;
        set;
    }


    /// <summary>
    /// Newsletter ID, required when creating new issue.
    /// </summary>
    public int NewsletterID
    {
        get;
        set;
    }


    /// <summary>
    /// ID of newsletter template that should be used for new issue.
    /// If not set, template from newsletter configuration is used.
    /// </summary>
    public int TemplateID
    {
        get
        {
            if (mTemplateID == 0)
            {
                // Try to get value from selector
                mTemplateID = ValidationHelper.GetInteger(issueTemplate.Value, 0);
                if (mTemplateID == 0)
                {
                    // Try to get value from hidden field
                    mTemplateID = ValidationHelper.GetInteger(hdnTemplateID.Value, 0);
                }
            }

            return mTemplateID;
        }
        set
        {
            mTemplateID = value;
        }
    }


    /// <summary>
    /// Gets or sets value that indicates whether control is enabled.
    /// </summary>
    public bool Enabled
    {
        get;
        set;
    }


    /// <summary>
    /// Indicates if advanced options should be displayed.
    /// </summary>
    protected bool ShowAdvancedOptions
    {
        get
        {
            return ValidationHelper.GetBoolean(ViewState["ShowAdvancedOptions"], false);
        }
        set
        {
            ViewState["ShowAdvancedOptions"] = value;
        }
    }


    /// <summary>
    /// Gets the newsletter.
    /// </summary>
    public NewsletterInfo Newsletter
    {
        get
        {
            return mNewsletter ?? (mNewsletter = NewsletterInfoProvider.GetNewsletterInfo(NewsletterID));
        }
    }

    #endregion


    #region "Page events"

    protected void Page_Load(object sender, EventArgs e)
    {
        if (StopProcessing || ((NewsletterID <= 0) && (IssueID <= 0)))
        {
            return;
        }

        mAreCampaignsAvailable = LicenseKeyInfoProvider.IsFeatureAvailable(RequestContext.CurrentDomain, FeatureEnum.CampaignAndConversions);
        ToggleUTMCampaignInput();
        mUTMCampaignTextBox = GetUTMCampaignTextBox();

        ReloadData(false);
    }


    protected void Page_PreRender(object sender, EventArgs e)
    {
        // Preserve ID of selected template
        hdnTemplateID.Value = ValidationHelper.GetString(issueTemplate.Value, string.Empty);

        if (!StopProcessing)
        {
            RegisterScript();
        }
    }


    /// <summary>
    /// Swithes simple and advanced options.
    /// </summary>
    protected void lnkToggleAdvanced_Click(object sender, EventArgs e)
    {
        ShowAdvancedOptions = !plcAdvanced.Visible;

        // Switch simple and advanced
        InitSimpleAdvancedOptions();

        // JS function for resizing is specified in Newsletter_ContentEditor.ascx
        ScriptManager.RegisterStartupScript(Page, typeof(string), "ResizeContent_" + ClientID, "if (SetIFrameHeight) { SetIFrameHeight(); }", true);
    }


    /// <summary>
    /// Shows or hides UTM parameters settings.
    /// </summary>
    protected void chkIssueUseUTM_CheckedChanged(object sender, EventArgs e)
    {
        pnlUTMParameters.Visible = chkIssueUseUTM.Checked;
    }

    #endregion


    #region "Public methods"

    /// <summary>
    /// Reloads control data.
    /// </summary>
    /// <param name="forceReload">Indicates if force reload should be used</param>
    public override void ReloadData(bool forceReload)
    {
        var isABTest = false;
        if (!mLoaded || forceReload)
        {
            IssueInfo issue = null;
            if (IssueID > 0)
            {
                // Get issue object
                issue = IssueInfoProvider.GetIssueInfo(IssueID);
                if (issue != null)
                {
                    if (NewsletterID == 0)
                    {
                        // Set newsletter ID
                        NewsletterID = issue.IssueNewsletterID;
                    }

                    if (string.IsNullOrEmpty(txtSubject.Text) || forceReload)
                    {
                        txtSubject.Text = issue.IssueSubject;
                        chkShowInArchive.Checked = issue.IssueShowInNewsletterArchive;
                        chkIssueUseUTM.Checked = issue.IssueUseUTM;
                    }

                    isABTest = issue.IssueIsABTest;
                }
            }

            // Get newsletter object
            if (Newsletter != null)
            {
                // Modify where condition of template selector if issue exists
                string issueTemplateWhere = (issue != null) ? string.Format(" OR TemplateID IN (SELECT IssueTemplateID From Newsletter_NewsletterIssue WHERE IssueID={0})", issue.IssueID) : string.Empty;

                // Initialize template selector
                issueTemplate.WhereCondition = String.Format("(TemplateType='{0}') AND (TemplateID IN (SELECT NewsletterTemplateID FROM Newsletter_Newsletter WHERE NewsletterID={1})" +
                    " OR TemplateID IN (SELECT TemplateID FROM Newsletter_EmailTemplateNewsletter WHERE NewsletterID={1}){2}) AND (TemplateSiteID={3})",
                    EmailTemplateType.Issue, NewsletterID, issueTemplateWhere, Newsletter.NewsletterSiteID);

                if (TemplateID > 0)
                {
                    // Set selected value
                    issueTemplate.Value = TemplateID;
                }

                if ((forceReload || (TemplateID <= 0)) && (issue != null) && (issue.IssueTemplateID != TemplateID))
                {
                    // Change selected value
                    issueTemplate.Value = TemplateID = issue.IssueTemplateID;

                    issueTemplate.Reload(forceReload);
                }

                if (TemplateID <= 0)
                {
                    // Get ID of default template
                    issueTemplate.Value = TemplateID = Newsletter.NewsletterTemplateID;
                }

                // Prevent selecting none value in campaign selector if there is no campaign
                if (mAreCampaignsAvailable && CampaignInfoProvider.GetCampaigns().OnSite(SiteContext.CurrentSiteID).Count == 0)
                {
                    radUTMCampaignExisting.Checked = false;
                    radUTMCampaignExisting.Enabled = false;
                    selectorUTMCampaign.Enabled = false;
                    radUTMCampaignNew.Checked = true;
                    mUTMCampaignTextBox.Enabled = true;
                }

                // Initialize inputs and content controls
                if (!RequestHelper.IsPostBack() || forceReload)
                {
                    txtSenderName.Text = (issue != null ? issue.IssueSenderName : Newsletter.NewsletterSenderName);
                    txtSenderEmail.Text = (issue != null ? issue.IssueSenderEmail : Newsletter.NewsletterSenderEmail);
                    txtIssueUTMSource.Text = (issue != null ? issue.IssueUTMSource : string.Empty);

                    if (issue != null)
                    {
                        if (mAreCampaignsAvailable && (CampaignInfoProvider.GetCampaignByUTMCode(issue.IssueUTMCampaign, SiteContext.CurrentSiteName) != null))
                        {
                            selectorUTMCampaign.Value = issue.IssueUTMCampaign;
                            selectorUTMCampaign.Reload(forceReload);

                            selectorUTMCampaign.Enabled = true;

                            radUTMCampaignExisting.Checked = true;
                            radUTMCampaignNew.Checked = false;
                            mUTMCampaignTextBox.Enabled = false;
                        }
                        else
                        {
                            mUTMCampaignTextBox.Text = issue.IssueUTMCampaign;
                            mUTMCampaignTextBox.Enabled = true;

                            radUTMCampaignExisting.Checked = false;
                            radUTMCampaignNew.Checked = true;
                            selectorUTMCampaign.Enabled = false;
                        }
                    }
                }
                else
                {
                    if (issue != null)
                    {
                        if (string.IsNullOrEmpty(txtIssueUTMSource.Text.Trim()))
                        {
                            txtIssueUTMSource.Text = Normalize(Newsletter.NewsletterName + "_" + txtSubject.Text.Trim());
                        }

                        if (string.IsNullOrEmpty(mUTMCampaignTextBox.Text.Trim()))
                        {
                            mUTMCampaignTextBox.Text = Newsletter.NewsletterName.ToLower();
                        }
                    }
                }

                contentBody.NewsletterID = NewsletterID;
                contentBody.IssueID = IssueID;
                contentBody.TemplateID = TemplateID;
                contentBody.Enabled = Enabled;
                contentBody.ReloadData(forceReload);

                // Set simple/advanced options visibility
                InitSimpleAdvancedOptions();

                mUTMCampaignTextBox.Attributes["placeholder"] = Newsletter.NewsletterName.ToLower();

                // Set flag
                mLoaded = true;
            }
        }

        txtIssueUTMMedium.Text = DEFAULT_UTM_MEDIUM;
        chkShowInArchive.Enabled = txtSubject.Enabled = txtSenderEmail.Enabled = txtSenderName.Enabled = issueTemplate.Enabled = Enabled;
        chkIssueUseUTM.Enabled = pnlIssueUTMCampaign.Enabled = pnlIssueUTMMedium.Enabled = pnlIssueUTMSource.Enabled = Enabled;
        pnlUTMParameters.Visible = chkIssueUseUTM.Checked;

        InitTooltips(isABTest);
    }


    /// <summary>
    /// Toggles form control for selecting the UTM campaign code of the issue.
    /// </summary>
    /// <remarks>
    /// If campaigns are available for the current license, advanced control for selecting from existing campaigns is be used.
    /// Otherwise, simple textbox is displayed.
    /// </remarks>
    private void ToggleUTMCampaignInput()
    {
        pnlIssueUTMCampaignTextBox.Visible = !mAreCampaignsAvailable;
        pnlIssueUTMCampaign.Visible = mAreCampaignsAvailable;
    }


    /// <summary>
    /// Gets reference to the textbox specifying the issue UTM campaign code. 
    /// </summary>
    /// <remarks>
    /// There are two candidates for the textbox depending on whether campaigns are available for the current license or not.
    /// </remarks>
    /// <returns>Reference to the appropriate textbox</returns>
    private CMSTextBox GetUTMCampaignTextBox()
    {
        if (mAreCampaignsAvailable)
        {
            return txtIssueUTMCampaign;
        }

        return txtIssueUTMCampaignTextBox;
    }


    /// <summary>
    /// Validates dialog's content before saving.
    /// </summary>
    public bool IsValid()
    {
        // Check subject field for emptyness
        ErrorMessage = new Validator().NotEmpty(txtSubject.Text.Trim(), GetString("NewsletterContentEditor.SubjectRequired")).Result;

        // Check sender email format if entered
        if (string.IsNullOrEmpty(ErrorMessage) && !string.IsNullOrEmpty(txtSenderEmail.Text.Trim()) && !ValidationHelper.IsEmail(txtSenderEmail.Text.Trim()))
        {
            ErrorMessage = GetString("Newsletter_Edit.ErrorEmailFormat");
        }

        return mValidated = String.IsNullOrEmpty(ErrorMessage);
    }


    /// <summary>
    /// Creates new or updates existing newsletter issue.
    /// </summary>
    public bool Save()
    {
        if (mValidated || IsValid())
        {
            IssueInfo issue;

            if (IssueID == 0)
            {
                // Initialize new issue
                issue = new IssueInfo();
                issue.IssueUnsubscribed = 0;
                issue.IssueSentEmails = 0;
                issue.IssueNewsletterID = NewsletterID;
                issue.IssueSiteID = SiteContext.CurrentSiteID;
            }
            else
            {
                issue = IssueInfoProvider.GetIssueInfo(IssueID);
            }

            if (issue != null)
            {
                issue.IssueTemplateID = TemplateID;
                issue.IssueShowInNewsletterArchive = chkShowInArchive.Checked;
                issue.IssueSenderName = txtSenderName.Text.Trim();
                issue.IssueSenderEmail = txtSenderEmail.Text.Trim();
                issue.IssueUseUTM = chkIssueUseUTM.Checked;

                var normalizedUtmSource = Normalize(txtIssueUTMSource.Text.Trim());
                if (string.IsNullOrEmpty(normalizedUtmSource))
                {
                    normalizedUtmSource = Normalize(Newsletter.NewsletterName + "_" + txtSubject.Text.Trim());
                }
                txtIssueUTMSource.Text = issue.IssueUTMSource = normalizedUtmSource;

                if (radUTMCampaignNew.Checked)
                {
                    var normalizedUtmCampaign = Normalize(mUTMCampaignTextBox.Text.Trim());
                    if (string.IsNullOrEmpty(normalizedUtmCampaign))
                    {
                        normalizedUtmCampaign = Normalize(Newsletter.NewsletterName);
                    }
                    mUTMCampaignTextBox.Text = issue.IssueUTMCampaign = normalizedUtmCampaign;
                }
                else
                {
                    issue.IssueUTMCampaign = selectorUTMCampaign.Value.ToString().ToLower();
                }

                if (issue.IssueIsABTest)
                {
                    SynchronizeUTMParameters(issue);
                }

                // Saves content of editable region(s)
                // Get content from hidden field
                string content = hdnIssueContent.Value;
                string[] regions = null;
                if (!string.IsNullOrEmpty(content))
                {
                    // Split content for each region, separator is '#|#'
                    regions = content.Split(new[] { "#|#" }, StringSplitOptions.RemoveEmptyEntries);
                }
                issue.IssueText = IssueHelper.GetContentXML(regions);

                // Remove '#' from macros if included
                txtSubject.Text = txtSubject.Text.Trim().Replace("#%}", "%}");

                // Sign macros if included in the subject
                issue.IssueSubject = MacroSecurityProcessor.AddSecurityParameters(txtSubject.Text, MembershipContext.AuthenticatedUser.UserName, null);

                // Save issue
                IssueInfoProvider.SetIssueInfo(issue);

                // Update IssueID
                IssueID = issue.IssueID;

                return true;
            }
        }

        return false;
    }


    /// <summary>
    /// Causes an update of issue properties and content areas.
    /// </summary>
    public void UpdateContent()
    {
        // Update issue properties
        pnlUpdate.Update();
        // Update content area
        pnlBodyUpdate.Update();
    }


    /// <summary>
    /// Refreshes content of editable regions in the issue body.
    /// Can be used when validation has failed.
    /// </summary>
    public void RefreshEditableRegions()
    {
        // Init editable regions on start up
        ScriptHelper.RegisterStartupScript(Page, typeof(string), "SetRegionContent", string.Format("SetRegionContent({0});", ScriptHelper.GetString(hdnIssueContent.Value)), true);
    }

    #endregion


    #region "Protected methods"

    /// <summary>
    /// Shows or hides advanced options.
    /// </summary>
    protected void InitSimpleAdvancedOptions()
    {
        plcAdvanced.Visible = ShowAdvancedOptions;
        imgToggleAdvanced.ImageUrl = (ShowAdvancedOptions ? GetImageUrl("Design/Controls/UniGrid/Actions/SortUp.png") : GetImageUrl("Design/Controls/UniGrid/Actions/SortDown.png"));
        lnkToggleAdvanced.Text = (ShowAdvancedOptions ? GetString("newsletterissue.hideadvancedoptions") : GetString("newsletterissue.showadvancedoptions"));
    }


    /// <summary>
    /// Radio buttons for UTM campaign checked changed.
    /// </summary>
    protected void radUTMCampaign_OnCheckedChanged(object sender, EventArgs e)
    {
        txtIssueUTMCampaign.Enabled = radUTMCampaignNew.Checked;
        selectorUTMCampaign.Enabled = radUTMCampaignExisting.Checked;
    }


    /// <summary>
    /// Registers JS script.
    /// </summary>
    protected void RegisterScript()
    {
        string script = string.Format(
@"// iframe with ID 'iframeContent' is in Newsletter_ContentEditor.ascx control
var frame = null;

function GetFrame() {{
    if (frame == null) {{
         frame = document.getElementById('iframeContent');
    }}
    return frame;
}}

// Gets content of editable regions to hidden field
function GetContent() {{
    var hdnContent = document.getElementById('{0}');
    var F = GetFrame();
    if ((hdnContent != null) && (F != null) && (F.contentWindow.GetContent != null)) {{
        hdnContent.value = F.contentWindow.GetContent();
        return true;
    }}
    return false;
}}

// Sets hidden field value to editable regions
function SetRegionContent(regcontent) {{
    var F = GetFrame();
    if ((regcontent != null) && (F != null)) {{
        // Frame onload event cannot be used, because it is fired too late
        WaitForFrame(F, regcontent);
    }}
    return false;
}}


function WaitForFrame(frame, content) {{
    if ( frame.contentWindow != null && frame.contentWindow.SetRegionContent != null ) {{
        frame.contentWindow.SetRegionContent(content);
    }} else {{
        setTimeout( function(){{ WaitForFrame(frame, content); }}, 250 );
    }}
}};

// Remembers last focused region
function RememberFocusedRegion() {{
    var F = GetFrame();
    if ((F != null) && (F.contentWindow.RememberFocusedRegion != null)) {{
        F.contentWindow.RememberFocusedRegion();
    }}
}}

// Pastes image into last focused editable region
function PasteImage(imageurl) {{
    var imageHtml = '<img src=""' + imageurl + '"" alt="""" />';
    var F = GetFrame();
    if ((F != null) && (F.contentWindow.InsertHTML != null)) {{
        return F.contentWindow.InsertHTML(imageHtml);
    }}
}}

// Prevent enter key on form as postback does not send data from issue text automatically
// Default button cannot be easily set because 'Save button' is generated dynamically
$cmsj(function () {{
    $cmsj(this).keypress(function (event) {{
        if (event.which == 13) {{
            event.preventDefault();
        }}
    }});
}});", hdnIssueContent.ClientID);

        ScriptHelper.RegisterClientScriptBlock(Page, typeof(string), "EditIssueScript_" + ClientID, script, true);
    }

    #endregion


    #region "Private methods"

    private void InitTooltips(bool isABTest)
    {
        pnlIssueUTMSource.ToolTip = lblIssueUTMSource.ToolTip = lblScreenReaderIssueUTMSource.Text = iconHelpIssueUTMSource.ToolTip = GetString("newsletterissue.utm.source.description");
        pnlIssueUTMMedium.ToolTip = lblIssueUTMMedium.ToolTip = lblScreenReaderIssueUTMMedium.Text = iconHelpIssueUTMMedium.ToolTip = GetString("newsletterissue.utm.medium.description");
        pnlIssueUTMCampaign.ToolTip = lblIssueUTMCampaign.ToolTip = GetString("newsletterissue.utm.campaign.description");
        lblSubject.ToolTip = GetString("newsletterissue.subject.description");
        pnlIssueSenderName.ToolTip = lblSenderName.ToolTip = GetString("newsletterissue.sender.name.description");
        pnlIssueSenderEmail.ToolTip = lblSenderEmail.ToolTip = GetString("newsletterissue.sender.email.description");
        pnlIssueTemplate.ToolTip = lblTemplate.ToolTip = GetString("newsletterissue.template.description");
        pnlIssueArchive.ToolTip = lblArchive.ToolTip = GetString("newslettertemplate_edit.showinarchive.description");

        var useUTMTooltipText = GetString("newsletterissue.utm.use.description") + (isABTest ? " " + GetString("newsletterissue.utm.use.description.ab") : "");
        pnlIssueUseUTM.ToolTip = lblIssueUseUTM.ToolTip = lblScreenReaderIssueUseUTM.Text = iconHelpIssueUseUTM.ToolTip = useUTMTooltipText;

        ScriptHelper.RegisterBootstrapTooltip(Page, ".info-icon > i");
    }


    /// <summary>
    /// Returns string that can be later used in URL and it is safe for the analytics.
    /// </summary>
    private string Normalize(string input)
    {
        input = input.Replace(' ', '_');
        return ValidationHelper.GetCodeName(input, "", 200, useUnicode: false).ToLower();
    }


    /// <summary>
    /// Synchronize UTM parameters between all A/B test variants.
    /// </summary>
    /// <param name="issue">Issue</param>
    private void SynchronizeUTMParameters(IssueInfo issue)
    {
        var variants = IssueInfoProvider.GetIssues()
                                        .WhereNotEquals("IssueID", issue.IssueID)
                                        .And(w => w.WhereEquals("IssueVariantOfIssueID", issue.IssueVariantOfIssueID)
                                                   .Or()
                                                   .WhereEquals("IssueID", issue.IssueVariantOfIssueID))
                                        .ToList();

        foreach (var variant in variants)
        {
            variant.IssueUTMCampaign = issue.IssueUTMCampaign;
            variant.IssueUTMSource = issue.IssueUTMSource;
            variant.IssueUseUTM = issue.IssueUseUTM;
            IssueInfoProvider.SetIssueInfo(variant);
        }
    }

    #endregion
}