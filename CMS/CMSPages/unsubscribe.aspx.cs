using System;
using System.Data;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using CMS.Helpers;
using CMS.UIControls;

public partial class CMSPages_unsubscribe : CMSPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string datetime = QueryHelper.GetString("datetime", string.Empty);

        // Forums
        Guid subGuid = QueryHelper.GetGuid("forumsubguid", Guid.Empty);
        int forumId = QueryHelper.GetInteger("forumid", 0);
        string forumSubscriptionHash = QueryHelper.GetString("forumsubscriptionhash", string.Empty);

        if (subGuid != Guid.Empty)
        {
            Server.Transfer(ResolveUrl("~/CMSModules/Forums/CMSPages/Unsubscribe.aspx?forumsubguid=") + subGuid + "&forumid=" + forumId);
        }
        else if (!string.IsNullOrEmpty(forumSubscriptionHash))
        {
            Server.Transfer(ResolveUrl("~/CMSModules/Forums/CMSPages/Unsubscribe.aspx?forumsubscriptionhash=") + forumSubscriptionHash + "&datetime=" + datetime);
        }

        Server.Transfer(ResolveUrl("~/CMSModules/Newsletters/CMSPages/Unsubscribe.aspx?" + CMSHttpContext.Current.Request.QueryString));
    }
}