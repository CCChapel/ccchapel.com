using CMS.ExtendedControls;
using CMS.UIControls;

public partial class CMSFormControls_Filters_TextFilter : TextFilterControl
{
    #region "Properties"
    
    /// <summary>
    /// Text box control
    /// </summary>
    protected override CMSTextBox TextBoxControl
    {
        get
        {
            return txtText;
        }
    }


    /// <summary>
    /// Operator drop down list control
    /// </summary>
    protected override CMSDropDownList OperatorControl
    {
        get
        {
            return drpOperator;
        }
    }

    #endregion
}