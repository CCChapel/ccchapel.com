using System.Web.UI.WebControls;

using CMS.FormControls;

public partial class CMSFormControls_VisibilityControls_DropDownVisibility : FormEngineVisibilityControl
{
    #region "Public properties"

    /// <summary>
    /// Drop-down control is used for field visibility type selection.
    /// </summary>
    protected override ListControl VisibilityControl
    {
        get
        {
            return drpVisibility;
        }
    }

    #endregion
}