using System;

using CMS.Base;
using CMS.Core;
using CMS.CustomTables;
using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.FormEngine;
using CMS.Helpers;
using CMS.IO;
using CMS.OnlineForms;
using CMS.UIControls;

public partial class CMSAdminControls_UI_Development_Generators_ContentItemCodeGenerator : CMSAdminControl
{
    private DataClassInfo mDataClass;
    private string mFolderBasePath;

 
    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        btnSaveCode.Click += SaveCode;

        try
        {
            mDataClass = GetDataClassFromContext();
            mFolderBasePath = String.Format("~/{0}/CMSClasses", SystemContext.IsWebApplicationProject ? "Old_App_Code" : "App_Code");

            if (!RequestHelper.IsPostBack())
            {

                if (ContentItemCodeGenerator.Internal.CanGenerateItemClass(mDataClass))
                {
                    txtItemCode.Text = ContentItemCodeGenerator.Internal.GenerateItemClass(mDataClass);
                }

                if (ContentItemCodeGenerator.Internal.CanGenerateItemProviderClass(mDataClass))
                {
                    txtProviderCode.Text = ContentItemCodeGenerator.Internal.GenerateItemProviderClass(mDataClass);
                }
                else
                {
                    pnlGeneratedCode.CssClass = null;
                    pnlItemCode.CssClass = null;
                    pnlProviderCode.Visible = false;
                }

                ucSavePath.Value = mFolderBasePath;
                if (Directory.Exists(mFolderBasePath))
                {
                    ucSavePath.DefaultPath = mFolderBasePath;
                }
            }
        }
        catch (Exception exception)
        {
            CoreServices.EventLog.LogException("Content item code generator", "Initialize", exception);

            btnSaveCode.Enabled = false;

            var message = GetString("general.exception");
            ShowError(message);
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

            ContentItemCodeFileGenerator.Internal.GenerateFiles(mDataClass, baseFolderPath);

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


    private DataClassInfo GetDataClassFromContext()
    {
        var item = UIContext.EditedObject as BaseInfo;

        if (item == null)
        {
            throw new Exception("Context does not contain content item.");
        }

        var dataClass = FindDataClass(item);

        if (dataClass == null)
        {
            throw new Exception("Content item class was not found.");
        }

        return dataClass;
    }


    private DataClassInfo FindDataClass(BaseInfo item)
    {
        switch (item.TypeInfo.ObjectType)
        {
            case BizFormInfo.OBJECT_TYPE:
                var classId = ValidationHelper.GetInteger(item.GetValue("FormClassID"), 0);
                return DataClassInfoProvider.GetDataClassInfo(classId);
            case DocumentTypeInfo.OBJECT_TYPE_DOCUMENTTYPE:
            case CustomTableInfo.OBJECT_TYPE_CUSTOMTABLE:
                var className = ValidationHelper.GetString(item.GetValue("ClassName"), String.Empty);
                return DataClassInfoProvider.GetDataClassInfo(className);
        }

        return null;
    }
}
