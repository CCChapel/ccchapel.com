using System;
using System.Linq;

using CMS.UIControls;
using CMS.Membership;

public partial class CMSAdminControls_UI_SmartTip : CMSUserControl
{
    private readonly UserSmartTipDismissalManager mUserSmartTipDismissalManager = new UserSmartTipDismissalManager(MembershipContext.AuthenticatedUser);
    private bool mIsDismissable = true;


    /// <summary>
    /// Gets or sets the identifier of the smart tip used for storing the dismissed state. If multiple smart tips with the same
    /// identifier are created, closing one will result in closing all of them. This property must be set if smart tip is dismissable.
    /// 
    /// </summary>
    public string DismissedStateIdentifier
    {
        get;
        set;
    }


    /// <summary>
    /// If true, smart tip is dismissable. Close button is shown. DismissedStateIdentifier must be set.
    /// Otherwise close button is hidden.
    /// Default is true.
    /// </summary>
    public bool IsDismissable
    {
        get
        {
            return mIsDismissable;
        }
        set
        {
            mIsDismissable = value;
        }
    }


    /// <summary>
    /// Sets the content of the smart tip.
    /// Use HTML.
    /// </summary>
    public string Content
    {
        get
        {
            return ltlContent.Text;
        }
        set
        {
            ltlContent.Text = value;
        }
    }


    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (IsDismissable && String.IsNullOrEmpty(DismissedStateIdentifier))
        {
            throw new Exception("[SmartTip]: DismissedStateIdentifier state property has to be set");
        }

        btnClose.Visible = IsDismissable;

        if (IsDismissable && mUserSmartTipDismissalManager.IsSmartTipDismissed(DismissedStateIdentifier))
        {
            pnlContainer.Visible = false;
        }
    }


    protected void btnClose_Click(object sender, EventArgs e)
    {
        if (String.IsNullOrEmpty(DismissedStateIdentifier))
        {
            throw new Exception("[SmartTip]: DismissedStateIdentifier state property has to be set");
        }

        pnlContainer.Visible = false;

        mUserSmartTipDismissalManager.StoreDismissedSmartTip(DismissedStateIdentifier);
    }
}