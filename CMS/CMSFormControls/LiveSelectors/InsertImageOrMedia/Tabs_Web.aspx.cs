using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CMS.ExtendedControls;
using CMS.Helpers;
using CMS.Membership;
using CMS.UIControls;
using CMS.Base;

public partial class CMSFormControls_LiveSelectors_InsertImageOrMedia_Tabs_Web : CMSLiveModalPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        bool checkUI = ValidationHelper.GetBoolean(SettingsHelper.AppSettings["CKEditor:PersonalizeToolbarOnLiveSite"], false);
        if (checkUI)
        {
            string output = QueryHelper.GetString("output", "");
            OutputFormatEnum outputFormat = CMSDialogHelper.GetOutputFormat(output, QueryHelper.GetBoolean("link", false));

            // Check UIProfile
            if ((outputFormat == OutputFormatEnum.HTMLMedia) && !MembershipContext.AuthenticatedUser.IsAuthorizedPerUIElement("CMS.WYSIWYGEditor", "InsertImageOrMedia"))
            {
                RedirectToUIElementAccessDenied("CMS.WYSIWYGEditor", "InsertImageOrMedia");
            }
            else if (!MembershipContext.AuthenticatedUser.IsAuthorizedPerUIElement("CMS.MediaDialog", "WebTab"))
            {
                RedirectToUIElementAccessDenied("CMS.MediaDialog", "WebTab");
            }
        }

        if (QueryHelper.ValidateHash("hash"))
        {
            ScriptHelper.RegisterJQuery(Page);
            CMSDialogHelper.RegisterDialogHelper(Page);
        }
        else
        {
            webContentSelector.StopProcessing = true;
            webContentSelector.Visible = false;
            string url = ResolveUrl(UIHelper.GetErrorPageUrl("dialogs.badhashtitle", "dialogs.badhashtext", true));
            ltlScript.Text = ScriptHelper.GetScript("if (window.parent != null) { window.parent.location = '" + url + "' }");
        }
    }
}