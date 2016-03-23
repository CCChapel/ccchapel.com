using System;

using CMS.UIControls;
using CMS.Helpers;

public partial class CMSModules_Blogs_CMSPages_SubscriptionApproval : LivePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        var blogPostPath = (subscriptionApproval.SubscriptionSubject != null) ? ScriptHelper.GetString(subscriptionApproval.SubscriptionSubject.DocumentNamePath) : null;

        PageTitle.TitleText = HTMLHelper.HTMLEncode(string.Format(GetString("blog.subscriptionconfirmation"), blogPostPath));
    }
}
