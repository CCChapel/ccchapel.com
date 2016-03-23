using System;
using System.Web.UI.WebControls;

using CMS.FormControls;
using CMS.Helpers;

public partial class CMSFormControls_System_SelectCMSVersion : FormEngineUserControl
{
    #region "Properties"

    /// <summary>
    /// Indicates if value of form control could be empty.
    /// </summary>
    public bool AllowEmpty
    {
        get;
        set;
    }


    /// <summary>
    /// Selected version (e.g. '5.5', '5.5R2',...).
    /// </summary>
    public override object Value
    {
        get
        {
            EnsureList();
            return (drpVersion.SelectedItem == null) ? "" : drpVersion.SelectedItem.Value;
        }
        set
        {
            EnsureList();
            drpVersion.SelectedValue = (value == null) ? "" : value.ToString();
        }
    }

    #endregion


    #region "Control methods"

    private void EnsureList()
    {
        EnsureChildControls();

        if (drpVersion.Items.Count == 0)
        {
            // Fill the combo with versions
            drpVersion.Items.Add(new ListItem("(none)", ""));

            foreach (string version in new [] { "3.0", "3.1", "3.1a", "4.0", "4.1", "5.0", "5.5", "5.5R2", "6.0", "7.0", "8.0", "8.1", "8.2", "9.0" })
            {
                drpVersion.Items.Add(new ListItem(String.Format("CMS {0}", version), version));
            }
        }
    }

    protected override void OnInit(EventArgs e)
    {
        EnsureList();
        base.OnInit(e);
    }


    protected void Page_Load(object sender, EventArgs e)
    {
    }


    /// <summary>
    /// Validates the return value of form control.
    /// </summary>
    public override bool IsValid()
    {
        if (!AllowEmpty && (drpVersion.SelectedValue == ""))
        {
            ValidationError = ResHelper.GetString("general.requirescmsversion");
            return false;
        }

        return true;
    }

    #endregion
}