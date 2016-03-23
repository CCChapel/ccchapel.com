using System;
using System.Linq;

using CMS;
using CMS.Core;
using CMS.ExtendedControls;
using CMS.FormControls;
using CMS.Modules;
using CMS.Newsletters;
using CMS.PortalEngine;
using CMS.Helpers;
using CMS.SiteProvider;


[assembly: RegisterCustomClass("UnsubscriptionsNewItemExtender", typeof(UnsubscriptionsNewItemExtender))]

/// <summary>
/// Extends unsubscription new item form.
/// </summary>
public class UnsubscriptionsNewItemExtender : ControlExtender<UIForm>
{
    /// <summary>
    /// Initializes extender.
    /// </summary>
    public override void OnInit()
    {
        Control.OnBeforeSave += Control_OnBeforeSave;
    }

    /// <summary>
    /// Checks whether the given email is already unsubscribed for the current site. In this case, display proper error since unsubscribing the same email again does not make sense.
    /// Sets up URL to redirect to listing page after creating of the unsubscription.
    /// </summary>
    void Control_OnBeforeSave(object sender, EventArgs e)
    {
        var unsubscription = (UnsubscriptionInfo)Control.Data;
        
        // Adds the unsubscribed mail to the redirect URL in order to be able to display it in the confirmation message on the listing page.
        var redirectUrl = URLHelper.AddParameterToUrl(GetRedirectUrl(), "unsubscriptionEmail", unsubscription.UnsubscriptionEmail);

        if (Service<IUnsubscriptionProvider>.Entry().IsUnsubscribedFromAllNewsletters(unsubscription.UnsubscriptionEmail, SiteContext.CurrentSiteID))
        {
            // Add query parameter, so information message that email is already unsubscribed can be displayed
            redirectUrl = URLHelper.AddParameterToUrl(redirectUrl, "alreadyUnsubscribed", "true");
            
            URLHelper.Redirect(redirectUrl);
        }

        // After a save action the page is automatically redirected to the page representing the Edit page of the created object. Since unsubscriptions are not editable,
        // no such page exists. Therefore the URL has to be set manually to navigate to the unsubscriptions listing. 
        Control.RedirectUrlAfterCreate = redirectUrl;
    }


    /// <summary>
    /// Gets URL where the page should be redirected after the unsubscription save.
    /// </summary>
    /// <returns>Relative URL navigating to the unsubscription listing page.</returns>
    private string GetRedirectUrl()
    {
        // Get URL of the unsubscription listing page
        var uiElement = UIElementInfoProvider.GetUIElementInfo(new Guid("C0B4B02A-F628-4204-9390-6CD9D9AE7125"));

        return UIContextHelper.GetElementUrl(
            uiElement,
            displayTitle: false    /* Ensure the black strip header won't be displayed */
        );
    }
}
