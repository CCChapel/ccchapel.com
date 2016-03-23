using System;
using System.Web.UI;
using System.Collections;

using CMS.FormControls;
using CMS.Helpers;
using CMS.PortalControls;
using CMS.DocumentEngine;
using CMS.Taxonomy;

public partial class CMSModules_Content_FormControls_Tags_TagSelector : FormEngineUserControl
{
    #region "Variables"

    private bool mEnabled = true;

    #endregion


    #region "Properties"

    /// <summary>
    /// Gets dialog identifier.
    /// </summary>
    private string DialogIdentifier
    {
        get
        {
            if (String.IsNullOrEmpty(hdnDialogIdentifier.Value))
            {
                if (String.IsNullOrEmpty(Request.Form[hdnDialogIdentifier.UniqueID]))
                {
                    hdnDialogIdentifier.Value = Guid.NewGuid().ToString();
                }
            }

            return HTMLHelper.HTMLEncode(hdnDialogIdentifier.Value);
        }
    }


    /// <summary>
    /// Enable/disable control.
    /// </summary>
    public override bool Enabled
    {
        get
        {
            return mEnabled;
        }
        set
        {
            mEnabled = value;
            btnSelect.Enabled = value;
        }
    }


    /// <summary>
    /// Gets or sets the field value.
    /// </summary>
    public override object Value
    {
        get
        {
            return TagHelper.GetTagsForSave(txtTags.Text.Trim());
        }
        set
        {
            txtTags.Text = ValidationHelper.GetString(value, "");
        }
    }


    /// <summary>
    /// Loads the other fields values to the state of the form control
    /// </summary>
    public override void LoadOtherValues()
    {
        // Currently does not support loading other values explicitly
    }


    /// <summary>
    /// Make sure that the DocumentTagGroupID always reflects the Group ID specified in form control.
    /// Otherwise it will use the default Group ID specified in metadata.
    /// </summary>
    public override object[,] GetOtherValues()
    {
        object[,] values = new object[1, 2];
        values[0, 0] = "DocumentTagGroupID";

        // Make sure that when no GroupId is set,
        // the null value will be used
        if (GroupId <= 0)
        {
            values[0, 1] = null;
        }
        else
        {
            values[0, 1] = GroupId;
        }

        return values;
    }


    /// <summary>
    /// Tag Group ID.
    /// </summary>
    public int GroupId
    {
        get
        {
            int mGroupId = ValidationHelper.GetInteger(GetValue("TagGroupID"), 0);
            if ((mGroupId == 0) && (Form != null))
            {
                TreeNode node = (TreeNode)Form.EditedObject;

                // When inserting new document
                if (Form.IsInsertMode)
                {
                    var parent = Form.ParentObject as TreeNode;
                    if (parent != null)
                    {
                        // Get path and groupID of the parent node
                        mGroupId = parent.DocumentTagGroupID;
                        // If nothing found try get inherited value
                        if (mGroupId == 0)
                        {
                            mGroupId = ValidationHelper.GetInteger(parent.GetInheritedValue("DocumentTagGroupID", false), 0);
                        }
                    }
                }
                // When editing existing document
                else if (node != null)
                {
                    // Get path and groupID of the parent node
                    mGroupId = node.DocumentTagGroupID;
                    // If nothing found try get inherited value
                    if (mGroupId == 0)
                    {
                        mGroupId = ValidationHelper.GetInteger(node.GetInheritedValue("DocumentTagGroupID", false), 0);
                    }
                }
            }

            return mGroupId;
        }
        set
        {
            SetValue("TagGroupID", value);
        }
    }

    #endregion


    #region "Page events"

    /// <summary>
    /// Init event.
    /// </summary>
    protected override void OnInit(EventArgs e)
    {
        // Ensure the script manager
        PortalHelper.EnsureScriptManager(Page);
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        RegisterScripts();
        
        btnSelect.Click += btnSelect_Click;
        btnSelect.Text = GetString("general.select");
    }


    protected void Page_PreRender(object sender, EventArgs e)
    {
        // Enable / Disable control
        txtTags.Enabled = Enabled;
        btnSelect.Enabled = Enabled;
        if (Enabled)
        {
            autoComplete.ContextKey = GroupId.ToString();
        }
    }


    protected void btnSelect_Click(object sender, EventArgs e)
    {
        SetDialogParameters();

        ScriptHelper.RegisterStartupScript(Page, typeof(string), "tagSelect", ScriptHelper.GetScript(String.Format("tagSelect('{0}');", DialogIdentifier)));
    }

    #endregion


    #region "Methods"

    /// <summary>
    /// Sets the dialog parameters to the context.
    /// </summary>
    private void SetDialogParameters()
    {
        var p = new Hashtable();

        p["textbox"] = txtTags.ClientID;
        p["group"] = GroupId;
        p["tags"] = txtTags.Text;

        WindowHelper.Add(DialogIdentifier, p);
    }


    /// <summary>
    /// Registers required scripts
    /// </summary>
    private void RegisterScripts()
    {
        // Ensure Script Manager is the first control on the page
        using (ScriptManager sMgr = ScriptManager.GetCurrent(Page))
        {
            if (sMgr != null)
            {
                sMgr.Services.Add(new ServiceReference("~/CMSModules/Content/FormControls/Tags/TagSelectorService.asmx"));
            }
        }

        // Register the dialog script
        ScriptHelper.RegisterDialogScript(Page);

        // Register tag script 
        ScriptHelper.RegisterStartupScript(this, typeof(string), "tagScript", ScriptHelper.GetScript(GetTagScript()));

        // Create script for valid inserting into textbox
        ScriptHelper.RegisterStartupScript(this, typeof(string), "tagSelectScript", @"
function itemSelected(source, eventArgs) {
    var txtBox = source.get_element();
    if (txtBox) {
        txtBox.value = eventArgs.get_text().replace(/\'""/,'""').replace(/""\'/,'""');
    }
}
", true);
    }


    /// <summary>
    /// Returns tag JS script.
    /// </summary>
    private string GetTagScript()
    {
        string baseUrl = IsLiveSite ? "~/CMSFormControls/LiveSelectors/TagSelector.aspx" : "~/CMSFormControls/Selectors/TagSelector.aspx";

        // Build script with modal dialog opener and set textbox functions
        return String.Format(@"
function tagSelect(id) {{
    modalDialog('{0}?params='+ id, 'TagSelector', 790, 670);
}}

function setTagsToTextBox(textBoxId, tagString) {{
    if (textBoxId != '') {{
        var textbox = document.getElementById(textBoxId);
        if (textbox != null){{
            textbox.value = decodeURI(tagString);
            if (window.Changed != null) {{
                window.Changed();
            }}
        }}
    }}
}}", ResolveUrl(baseUrl));
    }

    #endregion
}