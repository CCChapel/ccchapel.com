using System;
using System.Linq;
using System.Collections.Generic;

using CMS;
using CMS.DataEngine;
using CMS.ExtendedControls;
using CMS.FormControls;
using CMS.FormEngine;
using CMS.Helpers;

[assembly: RegisterCustomClass("FormUserControlEditExtender", typeof(FormUserControlEditExtender))]

/// <summary>
/// Form User Control UIForm extender
/// </summary>
public class FormUserControlEditExtender : ControlExtender<UIForm>
{
    #region "Constants"

    private const string SHOWIN_PAGETYPE = "pagetype";
    private const string SHOWIN_FORM = "form";
    private const string SHOWIN_CUSTOMTABLE = "customtable";
    private const string SHOWIN_SYSTEMTABLE = "systemtable";
    private const string SHOWIN_REPORT = "report";
    private const string SHOWIN_CONTROL = "control";

    private const string FIELD_PRIORITY = "UserControlPriorityBool";
    private const string FIELD_FOR = "UserControlFor";
    private const string FIELD_SHOWIN = "UserControlShowIn";
    private const string FIELD_SHOWIN2 = "UserControlShowIn2";

    #endregion


    #region "Control events"

    public override void OnInit()
    {
        Control.OnAfterDataLoad += Control_OnAfterDataLoad;
        Control.OnBeforeSave += Control_OnBeforeSave;
    }


    private void Control_OnAfterDataLoad(object sender, EventArgs e)
    {
        UIForm form = (UIForm)sender;
        if (form.IsInsertMode)
        {
            return;
        }

        FormUserControlInfo formControl = form.EditedObject as FormUserControlInfo;
        if (formControl != null)
        {
            // Set control's priority
            var priorityControl = form.FieldControls[FIELD_PRIORITY];
            if (priorityControl != null)
            {
                priorityControl.Value = (formControl.UserControlPriority == (int)ObjectPriorityEnum.High);
            }

            // Set which (data) types the control can be used for
            var controlFor = form.FieldControls[FIELD_FOR];
            if (controlFor != null)
            {
                controlFor.Value = GetControlForValue(formControl);
            }

            // Set which resources the control can be shown in
            var controlShowIn = form.FieldControls[FIELD_SHOWIN];
            if (controlShowIn != null)
            {
                controlShowIn.Value = GetControlShowInValue(formControl, true);
            }
            var controlShowIn2 = form.FieldControls[FIELD_SHOWIN2];
            if (controlShowIn2 != null)
            {
                controlShowIn2.Value = GetControlShowInValue(formControl, false);
            }
        }
    }


    private void Control_OnBeforeSave(object sender, EventArgs e)
    {
        UIForm form = (UIForm)sender;
        FormUserControlInfo formControl = form.EditedObject as FormUserControlInfo;
        if (formControl != null)
        {
            if (form.IsInsertMode)
            {
                FormEngineUserControl fileName = form.FieldControls["UserControlFileName"];
                FormEngineUserControl parentId = form.FieldControls["UserControlParentID"];

                if ((fileName != null) && (parentId != null) && fileName.Visible && !parentId.Visible && (ValidationHelper.GetInteger(parentId.Value, 0) > 0))
                {
                    // Reset inheritance setting if it's not visible
                    formControl.SetValue("UserControlParentID", null);
                }
            }
            else
            {
                // Set control's priority
                formControl.UserControlPriority = ValidationHelper.GetBoolean(form.GetFieldValue(FIELD_PRIORITY), false) ? (int)ObjectPriorityEnum.High : (int)ObjectPriorityEnum.Low;

                // Set which (data) types the control can be used for. Individual values are field types
                var values = GetFieldValues(form, FIELD_FOR);

                foreach (var group in DataTypeManager.GetFieldGroups())
                {
                    var col = FormHelper.GetDataTypeColumnForGroup(group);

                    formControl.SetValue(col, values.Contains(group));
                }

                // Set which resources the control can be shown in
                values = GetFieldValues(form, FIELD_SHOWIN);

                formControl.UserControlShowInDocumentTypes = values.Contains(SHOWIN_PAGETYPE);
                formControl.UserControlShowInBizForms = values.Contains(SHOWIN_FORM);

                values = GetFieldValues(form, FIELD_SHOWIN2);

                formControl.UserControlShowInCustomTables = values.Contains(SHOWIN_CUSTOMTABLE);
                formControl.UserControlShowInSystemTables = values.Contains(SHOWIN_SYSTEMTABLE);
                formControl.UserControlShowInReports = values.Contains(SHOWIN_REPORT);
                formControl.UserControlShowInWebParts = values.Contains(SHOWIN_CONTROL);
            }
        }
    }

    #endregion


    #region "Private methods"

    private static string GetControlForValue(FormUserControlInfo formControl)
    {
        var values = new List<string>();

        // Build the list of enabled field types from the control properties
        foreach (var group in DataTypeManager.GetFieldGroups())
        {
            var col = FormHelper.GetDataTypeColumnForGroup(group);

            if (ValidationHelper.GetBoolean(formControl.GetValue(col), false))
            {
                values.Add(group);
            }
        }

        return String.Join("|", values);
    }


    private static string GetControlShowInValue(FormUserControlInfo formControl, bool firstSet)
    {
        var values = new List<string>();
        if (firstSet)
        {
            if (formControl.UserControlShowInDocumentTypes)
            {
                values.Add(SHOWIN_PAGETYPE);
            }
            if (formControl.UserControlShowInBizForms)
            {
                values.Add(SHOWIN_FORM);
            }
        }
        else
        {
            if (formControl.UserControlShowInCustomTables)
            {
                values.Add(SHOWIN_CUSTOMTABLE);
            }
            if (formControl.UserControlShowInSystemTables)
            {
                values.Add(SHOWIN_SYSTEMTABLE);
            }
            if (formControl.UserControlShowInReports)
            {
                values.Add(SHOWIN_REPORT);
            }
            if (formControl.UserControlShowInWebParts)
            {
                values.Add(SHOWIN_CONTROL);
            }
        }

        return String.Join("|", values);
    }


    /// <summary>
    /// Gets the list of values from the given field
    /// </summary>
    /// <param name="form">Editing form</param>
    /// <param name="fieldName">Field name</param>
    private HashSet<string> GetFieldValues(UIForm form, string fieldName)
    {
        var values = ValidationHelper.GetString(form.GetFieldValue(fieldName), string.Empty);
        var items = values.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

        return new HashSet<string>(items, StringComparer.InvariantCultureIgnoreCase);
    }

    #endregion
}