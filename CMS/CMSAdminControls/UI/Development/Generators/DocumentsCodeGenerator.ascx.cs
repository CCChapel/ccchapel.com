using System;
using System.Collections.Generic;

using CMS.Base;
using CMS.Core;
using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.FormEngine;
using CMS.Helpers;
using CMS.IO;
using CMS.SiteProvider;
using CMS.UIControls;

public partial class CMSAdminControls_UI_Development_Generators_DocumentsCodeGenerator : CMSAdminControl
{
    private string mFolderBasePath;

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        btnSaveCode.Click += SaveCode;

        mFolderBasePath = String.Format("~/{0}/CMSClasses", SystemContext.IsWebApplicationProject ? "Old_App_Code" : "App_Code");
        if (!RequestHelper.IsPostBack())
        {
            ucSite.SiteID = SiteContext.CurrentSiteID;

            ucSavePath.Value = mFolderBasePath;
            if (Directory.Exists(mFolderBasePath))
            {
                ucSavePath.DefaultPath = mFolderBasePath;
            }
        }
    }


    private void SaveCode(object sender, EventArgs e)
    {
        try
        {
            var path = ValidationHelper.GetString(ucSavePath.Value, String.Empty);
            if (String.IsNullOrEmpty(path))
            {
                path = mFolderBasePath;
                ucSavePath.Value = path;
            }

            var baseFolderPath = URLHelper.GetPhysicalPath(path);

            ContentItemCodeFileGenerator.Internal.GenerateFiles(
                dataClasses: GetDataClasses(),
                baseFolderPath: baseFolderPath
            );

            var message = GetString("classes.code.filessavesuccess");
            ShowConfirmation(message);
        }
        catch (Exception exception)
        {
            CoreServices.EventLog.LogException("Content item code generator", "Save", exception);

            var message = GetString("classes.code.filessaveerror");
            ShowError(message);
        }
    }


    /// <summary>
    /// Returns a collection of classes for which their source code will be generated.
    /// </summary>
    private IEnumerable<DataClassInfo> GetDataClasses()
    {
        var classes = DocumentTypeHelper.GetDocumentTypeClasses()
            .WhereEqualsOrNull("ClassIsProduct", false)
            .OnSite(ucSite.SiteID);

        if (!chkIncludeContainerPageTypes.Checked)
        {
            // Filter-out container page types
            classes = classes.WhereTrue("ClassIsCoupledClass");
        }

        return classes.TypedResult;
    }
}
