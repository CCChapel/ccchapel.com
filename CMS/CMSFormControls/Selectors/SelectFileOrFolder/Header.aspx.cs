using System;

using CMS.Helpers;
using CMS.UIControls;

public partial class CMSFormControls_Selectors_SelectFileOrFolder_Header : CMSModalPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (QueryHelper.ValidateHash("hash"))
        {
            if (QueryHelper.GetBoolean("show_folders", false))
            {
                PageTitle.TitleText = GetString("dialogs.header.title.selectfolder");
            }
            else
            {
                PageTitle.TitleText = GetString("dialogs.header.title.selectfiles");
            }
        }
        else
        {
            string url = ResolveUrl(UIHelper.GetErrorPageUrl("dialogs.badhashtitle", "dialogs.badhashtext", true));
            ltlScript.Text = ScriptHelper.GetScript("if (window.parent != null) { window.parent.location = '" + url + "' }");
        }
    }
}