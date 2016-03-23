using System;

using CMS.Base;
using CMS.Helpers;
using CMS.PortalEngine;
using CMS.UIControls;

public partial class CMSAPIExamples_Default : GlobalAdminPage
{
    protected override void OnLoad(EventArgs e)
    {
        ShowInformation(GetString("apiexamples.overview"), persistent: true);
    }
}