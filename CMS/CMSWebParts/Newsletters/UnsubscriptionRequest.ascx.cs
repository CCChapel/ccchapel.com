using System;
using System.Linq;

using CMS.Core;
using CMS.EmailEngine;
using CMS.EventLog;
using CMS.Helpers;
using CMS.Newsletters;
using CMS.PortalControls;
using CMS.SiteProvider;

using EmailTemplateProvider = CMS.EmailEngine.EmailTemplateProvider;
using EmailTemplateInfo = CMS.EmailEngine.EmailTemplateInfo;
using CMS.DataEngine;

public partial class CMSWebParts_Newsletters_UnsubscriptionRequest : CMSAbstractWebPart
{
    #region "Public properties"

    /// <summary>
    /// Gets or sets submit button text.
    /// </summary>
    public string ButtonText
    {
        get
        {
            return ValidationHelper.GetString(GetValue("ButtonText"), String.Empty);
        }
        set
        {
            SetValue("ButtonText", value);
        }
    }


    /// <summary>
    /// Gets or sets newsletter name.
    /// </summary>
    public string NewsletterName
    {
        get
        {
            return ValidationHelper.GetString(GetValue("NewsletterName"), null);
        }
        set
        {
            SetValue("NewsletterName", value);
        }
    }


    /// <summary>
    /// Gets or sets info message.
    /// </summary>
    public string InformationText
    {
        get
        {
            return ValidationHelper.GetString(GetValue("InformationText"), null);
        }
        set
        {
            SetValue("InformationText", value);
        }
    }


    /// <summary>
    /// Gets or sets error message.
    /// </summary>
    public string ErrorText
    {
        get
        {
            return ValidationHelper.GetString(GetValue("ErrorText"), null);
        }
        set
        {
            SetValue("ErrorText", value);
        }
    }


    /// <summary>
    /// Gets or sets message that will be shown after successful unsubscription.
    /// </summary>
    public string ResultText
    {
        get
        {
            return ValidationHelper.GetString(GetValue("ResultText"), null);
        }
        set
        {
            SetValue("ResultText", value);
        }
    }

    #endregion


    /// <summary>
    /// Content loaded event handler.
    /// </summary>
    public override void OnContentLoaded()
    {
        base.OnContentLoaded();
        SetupControl();
    }


    /// <summary>
    /// Reloads data for partial caching.
    /// </summary>
    public override void ReloadData()
    {
        base.ReloadData();
        SetupControl();
    }


    /// <summary>
    /// Initializes the control properties.
    /// </summary>
    protected void SetupControl()
    {
        if (StopProcessing)
        {
            return;
        }

        if (!String.IsNullOrEmpty(InformationText))
        {
            lblInfo.Text = InformationText;
            lblInfo.Visible = true;
        }
        else
        {
            lblInfo.Visible = false;
        }

        btnSubmit.Text = !String.IsNullOrEmpty(ButtonText) ? ButtonText : GetString("general.ok");
        btnSubmit.Click += btnSubmit_Click;
    }


    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        string email = txtEmail.Text.Trim();
        string result = new Validator().IsEmail(email, GetString("unsubscribe.invalidemailformat")).Result;

        if (String.IsNullOrEmpty(result))
        {
            bool requestSent = false;
            var site = SiteContext.CurrentSite;

            var newsletter = NewsletterInfoProvider.GetNewsletterInfo(NewsletterName, site.SiteID);
            if (newsletter != null)
            {
                var unsubscriptionProvider = Service<IUnsubscriptionProvider>.Entry();

                if (!unsubscriptionProvider.IsUnsubscribedFromSingleNewsletter(email, newsletter.NewsletterID, newsletter.NewsletterSiteID))
                {
                    var subscriber = SubscriberInfoProvider.GetSubscriberByEmail(email, site.SiteID);
                    if (subscriber != null)
                    {
                        SendUnsubscriptionRequest(subscriber, newsletter, site.SiteName);
                        requestSent = true;
                    }
                }
            }

            // Unsubscription failed if none confirmation e-mail was sent
            if (!requestSent)
            {
                // Use default error message if none is specified
                result = String.IsNullOrEmpty(ErrorText) ? GetString("unsubscribe.notsubscribed") : ErrorText;
            }
        }

        // Display error message if set
        if (!string.IsNullOrEmpty(result))
        {
            lblError.Text = result;
            lblError.Visible = true;
        }
        else
        {
            // Display unsubscription confirmation
            lblInfo.Visible = true;
            lblInfo.Text = String.IsNullOrEmpty(ResultText) ? GetString("unsubscribe.confirmtext") : ResultText;
            lblError.Visible = false;
            txtEmail.Visible = false;
            btnSubmit.Visible = false;
        }
    }


    /// <summary>
    /// Creates and sends unsubscription e-mail.
    /// </summary>
    /// <param name="subscriber">Subscriber to be unsubscribed</param>
    /// <param name="news">Newsletter object</param>
    /// <param name="siteName">Name of site that subscriber is being unsubscribed from</param>
    protected void SendUnsubscriptionRequest(SubscriberInfo subscriber, NewsletterInfo news, string siteName)
    {
        var emailTemplate = EmailTemplateProvider.GetEmailTemplate("newsletter.unsubscriptionrequest", siteName);
        if (emailTemplate == null)
        {
            EventLogProvider.LogEvent(EventType.ERROR, "UnsubscriptionRequest", "Unsubscription request e-mail template is missing.");
            return;
        }

        string body = emailTemplate.TemplateText;
        string plainBody = emailTemplate.TemplatePlainText;

        // Resolve newsletter macros (first name, last name etc.)
        var issueHelper = new IssueHelper();
        if (issueHelper.LoadDynamicFields(subscriber, news, null, null, false, siteName, null, null, null))
        {
            body = issueHelper.ResolveDynamicFieldMacros(body, news);
            plainBody = issueHelper.ResolveDynamicFieldMacros(plainBody, news);
        }

        // Create e-mail
        var subscriberEmailRetriever = Service<ISubscriberEmailRetriever>.Entry();
        var emailMessage = new EmailMessage
        {
            EmailFormat = EmailFormatEnum.Default,
            From = EmailHelper.GetSender(emailTemplate, news.NewsletterSenderEmail),
            Recipients = subscriberEmailRetriever.GetSubscriberEmail(subscriber.SubscriberID),
            BccRecipients = emailTemplate.TemplateBcc,
            CcRecipients = emailTemplate.TemplateCc,
            Subject = ResHelper.LocalizeString(emailTemplate.TemplateSubject),
            Body = URLHelper.MakeLinksAbsolute(body),
            PlainTextBody = URLHelper.MakeLinksAbsolute(plainBody)
        };

        // Add attachments and send e-mail
        EmailHelper.ResolveMetaFileImages(emailMessage, emailTemplate.TemplateID, EmailTemplateInfo.OBJECT_TYPE, ObjectAttachmentsCategories.TEMPLATE);
        EmailSender.SendEmail(siteName, emailMessage);
    }
}