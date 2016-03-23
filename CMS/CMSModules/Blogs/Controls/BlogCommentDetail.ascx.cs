using System;
using System.Web.UI;

using CMS.Blogs;
using CMS.ExtendedControls;
using CMS.Globalization;
using CMS.Helpers;
using CMS.SiteProvider;
using CMS.Membership;
using CMS.DataEngine;


public partial class CMSModules_Blogs_Controls_BlogCommentDetail : BlogCommentDetail, IPostBackEventHandler
{
    #region "Variables"

    private CMSAdminControls_UI_UserPicture userPict;
    private CMSModules_AbuseReport_Controls_InlineAbuseReport ucInlineAbuseReport;

    #endregion


    /// <summary>
    /// Comment ID.
    /// </summary>
    public int CommentID
    {
        get
        {
            return Comment != null ? Comment.CommentID : 0;
        }
    }


    #region "Page events"

    protected void Page_Init(object sender, EventArgs e)
    {
        // Load controls dynamically
        userPict = (CMSAdminControls_UI_UserPicture)LoadControl("~/CMSAdminControls/UI/UserPicture.ascx");
        plcUserPicture.Controls.Add(userPict);

        ucInlineAbuseReport = (CMSModules_AbuseReport_Controls_InlineAbuseReport)LoadControl("~/CMSModules/AbuseReport/Controls/InlineAbuseReport.ascx");
        ucInlineAbuseReport.ReportObjectType = "blog.comment";
        plcInlineAbuseReport.Controls.Add(ucInlineAbuseReport);
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        // Controls initialization
        lnkApprove.Text = GetString("general.approve");
        lnkReject.Text = GetString("general.reject");
        lnkEdit.Text = GetString("general.edit");
        lnkDelete.Text = GetString("general.delete");

        lnkEdit.Visible = ShowEditButton;
        lnkDelete.Visible = ShowDeleteButton;
        lnkApprove.Visible = ShowApproveButton;
        lnkReject.Visible = ShowRejectButton;

        LoadData();

        ScriptHelper.RegisterDialogScript(Page);
        ScriptHelper.RegisterClientScriptBlock(this, typeof(string), "DeleteCommentConfirmation", ScriptHelper.GetScript("function ConfirmDelete(){ return confirm(" + ScriptHelper.GetString(GetString("BlogCommentDetail.DeleteConfirmation")) + ");}"));
    }

    #endregion


    #region "Public methods"

    /// <summary>
    /// Reload data.
    /// </summary>
    public void LoadData()
    {
        if (Comment == null)
        {
            return;
        }

        // Set user picture
        if (BlogProperties.EnableUserPictures)
        {
            userPict.UserID = Comment.CommentUserID;
            userPict.Width = BlogProperties.UserPictureMaxWidth;
            userPict.Height = BlogProperties.UserPictureMaxHeight;
            userPict.Visible = true;
            userPict.RenderOuterDiv = true;
            userPict.OuterDivCSSClass = "CommentUserPicture";

            // Gravatar support
            string avType = SettingsKeyInfoProvider.GetValue(SiteContext.CurrentSiteName + ".CMSAvatarType");
            if (avType == AvatarInfoProvider.USERCHOICE)
            {
                UserInfo ui = UserInfoProvider.GetUserInfo(Comment.CommentUserID);
                avType = ui != null ? ui.UserSettings.UserAvatarType : AvatarInfoProvider.GRAVATAR;
            }

            userPict.UserEmail = Comment.CommentEmail;
            userPict.UserAvatarType = avType;
        }
        else
        {
            userPict.Visible = false;
        }

        if (!String.IsNullOrEmpty(Comment.CommentUrl))
        {
            lnkName.Text = HTMLHelper.HTMLEncode(Comment.CommentUserName);
            lnkName.NavigateUrl = Comment.CommentUrl;
            // Add no follow attribute if it is required
            if (HTMLHelper.UseNoFollowForUsersLinks(SiteContext.CurrentSiteName))
            {
                lnkName.Attributes.Add("rel", "nofollow");
            }
            lblName.Visible = false;
        }
        else
        {
            lblName.Text = HTMLHelper.HTMLEncode(Comment.CommentUserName);
            lnkName.Visible = false;
        }

        lblText.Text = HTMLHelper.HTMLEncodeLineBreaks(Comment.CommentText);
        lblDate.Text = TimeZoneMethods.ConvertDateTime(Comment.CommentDate, this).ToString();

        string url = "~/CMSModules/Blogs/Controls/Comment_Edit.aspx";
        if (IsLiveSite)
        {
            url = "~/CMSModules/Blogs/CMSPages/Comment_Edit.aspx";
        }

        lnkEdit.OnClientClick = string.Format("EditComment('{0}?commentID={1}'); return false;", ResolveUrl(url), CommentID);
        lnkDelete.OnClientClick = string.Format("if(ConfirmDelete()) {{ {0}; }} return false;", GetPostBackEventReference("delete"));
        lnkApprove.OnClientClick = string.Format("{0}; return false;", GetPostBackEventReference("approve"));
        lnkReject.OnClientClick = string.Format("{0}; return false;", GetPostBackEventReference("reject"));

        // Initialize report abuse
        ucInlineAbuseReport.ReportTitle = ResHelper.GetString("BlogCommentDetail.AbuseReport", SettingsKeyInfoProvider.GetValue(SiteContext.CurrentSiteName + ".CMSDefaultCulture")) + Comment.CommentText;
        ucInlineAbuseReport.ReportObjectID = CommentID;
        ucInlineAbuseReport.CMSPanel.Roles = AbuseReportRoles;
        ucInlineAbuseReport.CMSPanel.SecurityAccess = AbuseReportSecurityAccess;
        ucInlineAbuseReport.CMSPanel.OwnerID = AbuseReportOwnerID;
    }


    public void RaisePostBackEvent(string eventArgument)
    {
        var parts = eventArgument.Split(';');
        FireOnCommentAction(parts[0], parts[1]);
    }

    #endregion


    #region "Private methods and event handlers"

    private string GetPostBackEventReference(string actionName)
    {
        return ControlsHelper.GetPostBackEventReference(this, string.Format("{0};{1}", actionName, CommentID));
    }

    #endregion
}