using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI;

using CMS.Core;
using CMS.DataEngine;
using CMS.FormEngine;
using CMS.Helpers;
using CMS.MacroEngine;
using CMS.Membership;
using CMS.Modules;
using CMS.OnlineMarketing;
using CMS.PortalEngine;
using CMS.SiteProvider;
using CMS.UIControls;

[UIElement(ModuleName.ONLINEMARKETING, "ContactImport")]
public partial class CMSModules_ContactManagement_Pages_Tools_Contact_ImportCSV : CMSContactManagementPage
{
    private SiteInfo mCurrentSite;


    private SiteInfo CurrentSite
    {
        get
        {
            // SiteContext.CurrentSite cannot be used, because it recognizes site based on domain name, so even if global is selected in site selector, SiteContext.CurrentSite never shows global site
            return mCurrentSite ?? (mCurrentSite = SiteInfoProvider.GetSiteInfo(QueryHelper.GetInteger("siteId", SiteContext.CurrentSiteID)));
        }
    }


    protected string YouTubeVideoLinkID
    {
        get;
        private set;
    }


    private object DataFromServer
    {
        get
        {
            var contactFieldsAndCategories = new ContactImportFieldsProvider().GetCategoriesAndFieldsAvailableForImport();

            return new
            {
                ResourceStrings = new Dictionary<string, string>
                {
                    {"om.contact.importcsv.notsupportedbrowser", GetString("om.contact.importcsv.notsupportedbrowser")},
                    {"om.contact.importcsv.selectfilebuttontext", GetString("om.contact.importcsv.selectfilebuttontext")},
                    {"om.contact.importcsv.mapstepmessage", GetString("om.contact.importcsv.mapstepmessage")},
                    {"om.contact.importcsv.messagetext", GetString("om.contact.importcsv.messagetext")},
                    {"om.contact.importcsv.noemailmapping", GetString("om.contact.importcsv.noemailmapping")},
                    {"om.contact.importcsv.belongsto", GetString("om.contact.importcsv.belongsto")},
                    {"om.contact.importcsv.importingstepmessage", GetString("om.contact.importcsv.importingstepmessage")},
                    {"om.contact.importcsv.importcontactsbuttontext", GetString("om.contact.importcsv.importcontactsbuttontext")},
                    {"om.contact.importcsv.donotimport", GetString("om.contact.importcsv.donotimport")},
                    {"om.contact.importcsv.importfinished", GetString("om.contact.importcsv.importfinished")},
                    {"om.contact.importcsv.finishedstepmessage", GetString("om.contact.importcsv.finishedstepmessage")},
                    {"om.contact.importcsv.duplicatescount", GetString("om.contact.importcsv.duplicatescount")},
                    {"om.contact.importcsv.notimported", GetString("om.contact.importcsv.notimported")},
                    {"om.contact.importcsv.importing", GetString("om.contact.importcsv.importing")},
                    {"om.contact.importcsv.emptyfile", GetString("om.contact.importcsv.emptyfile")},
                    {"om.contact.importcsv.importerror", GetString("om.contact.importcsv.importerror")},
                    {"om.contact.importcsv.importedcount", GetString("om.contact.importcsv.importedcount")},
                    {"om.contact.importcsv.invalidcsv", GetString("om.contact.importcsv.invalidcsv")},
                    {"om.contact.importcsv.confirmleave", GetString("om.contact.importcsv.confirmleave")},
                    {"om.contact.importcsv.badfiletypeorformat", GetString("om.contact.importcsv.badfiletypeorformat")},
                    {"om.contact.importcsv.unknownerrorclientside", GetString("om.contact.importcsv.unknownerrorclientside")},
                    {"om.contact.importcsv.notimported.link", GetString("om.contact.importcsv.notimported.link")},
                    {"general.loading", GetString("general.loading")},
                    {"general.close", GetString("general.close")},
                    {"om.contact.importcsv.segmentation.message", GetString("om.contact.importcsv.segmentation.message")},
                    {"om.contact.importcsv.segmentation.nocontactgroupname", GetString("om.contact.importcsv.segmentation.nocontactgroupname")},
                    {"om.contact.importcsv.segmentation.createnew", GetString("om.contact.importcsv.segmentation.createnew")},
                    {"om.contact.importcsv.segmentation.existing", GetString("om.contact.importcsv.segmentation.existing")},
                    {"om.contact.importcsv.segmentation.none", GetString("om.contact.importcsv.segmentation.none")},
                    {"om.contact.importcsv.segmentation.title", GetString("om.contact.importcsv.segmentation.title")},
                    {"om.contact.importcsv.segmentation.nocontactgroupselected", GetString("om.contact.importcsv.segmentation.nocontactgroupselected")},
                    {"om.contact.importcsv.segmentation.contactgroupname", GetString("om.contact.importcsv.segmentation.contactgroupname")},
                    {"om.contact.importcsv.segmentation.selectcontactgroup", GetString("om.contact.importcsv.segmentation.selectcontactgroup")},
                },
                ContactFields = PrepareCategoriesAndFields(contactFieldsAndCategories),
                ContactGroups = ContactGroupInfoProvider.GetContactGroups()
                                                        .OnSite(CurrentSite.SiteID)
                                                        .Select(g => new { g.ContactGroupGUID, g.ContactGroupDisplayName }),
                SiteGuid = CurrentSite.SiteGUID
            };
        }
    }


    protected override void OnLoad(EventArgs e)
    {
        PrepareHowToImportSmartTip();
        PrepareContinueToSmartTip();
    }


    protected override void OnPreRender(EventArgs e)
    {
        if (CheckSiteAndPermissions())
        {
            ScriptHelper.RegisterAngularModule("CMS.OnlineMarketing/ContactImport/Module", DataFromServer);

            base.OnPreRender(e);

            CurrentMaster.PanelContent.CssClass = "";

            PrepareTooltips();
        }
    }


    private void PrepareHowToImportSmartTip()
    {
        var linkBuilder = new MagnificPopupYouTubeLinkBuilder();
        YouTubeVideoLinkID = Guid.NewGuid().ToString();
        var link = linkBuilder.GetLink("KcTM0ur0MH4", YouTubeVideoLinkID, GetString("om.contact.importcsv.howto.howtoimportcontacts.link"));

        smrtpHowToImport.Content = string.Format(@"
<h4>{0}</h4>
<div class=""content-block-50"">
    <p>{1}<br />{2}</p>
</div>        
<div class=""content-block-50"">
    <p class=""lead"">{3}</p>
    <ul class=""om-import-csv-list"">
        <li>{4}</li>
        <li>{5}</li>
    </ul>
</div>", GetString("om.contact.importcsv.selectfilestepmessage.main"),
         GetString("om.contact.importcsv.selectfilestepmessage.message"),
         link,
         GetString("om.contact.importcsv.selectfilestepmessage.filerequirements"),
         GetString("om.contact.importcsv.selectfilestepmessage.header"),
         GetString("om.contact.importcsv.selectfilestepmessage.encoding"));

        smrtpHowToImport.ClientIDMode = ClientIDMode.Static;
        smrtpHowToImport.IsDismissable = false;
    }


    /// <summary>
    /// Prepares the "Continue to ..." smart tip, which is displayed at the end of the importing process.
    /// </summary>
    /// <remarks>
    /// This method automatically filter out all the application according to the current user permissions.
    /// If user is not eligible for any of the applications, smart tip remains hidden.
    /// </remarks>
    private void PrepareContinueToSmartTip()
    {
        smrtpContinueTo.IsDismissable = false;
        
        var listItems = new Dictionary<Guid, string>
        {
            { ApplicationsGuidList.ContactGroup, GetString("om.contact.importcsv.continuetosmarttip.contactgroup") },
            { ApplicationsGuidList.EmailMarketing, GetString("om.contact.importcsv.continuetosmarttip.emailmarketing") },
            { ApplicationsGuidList.Pages, GetString("om.contact.importcsv.continuetosmarttip.pages") },
        };

        // Filter out the applications current user has no permissions for
        var filteredApplications = GetFilteredApplications(listItems.Keys.ToList());
        if (!filteredApplications.Any())
        {
            smrtpContinueTo.Visible = false;
            return;
        }

        // Build HTML list code containing all filtered applications
        // Iterating through original collection with application guids in order to preserve the order
        var applicationList = new StringBuilder();
        foreach (var applicationGuid in listItems)
        {
            // If application is not present in the filtered collection, the user does not have proper persmissions
            var application = filteredApplications.SingleOrDefault(app => app.ElementGUID == applicationGuid.Key);
            if(application != null)
            { 
                applicationList.Append(GetContinueToSmartTipListContent(application, listItems[application.ElementGUID]));
            }
        }

        smrtpContinueTo.Content = string.Format(@"
<div class=""om-import-csv-next-steps"">
    <h4>{0}</h4>
    <div class=""content-block-50"">
        <ul>
            {1}
        </ul>
    </div>
</div>", GetString("om.contact.importcsv.continuetosmarttip.caption"), applicationList);

        smrtpContinueTo.ClientIDMode = ClientIDMode.Static;
    }


    /// <summary>
    /// Gets applications matching the given <paramref name="guidCollection"/> and filters them according to the current user permission.
    /// </summary>
    /// <param name="guidCollection">Collection of application Guids to be filtered</param>
    /// <returns>Collection of filtered applications</returns>
    private List<UIElementInfo> GetFilteredApplications(ICollection<Guid> guidCollection)
    {
        var applicationsWhere = new WhereCondition().WhereIn("ElementGUID", guidCollection);
        var applications = ApplicationHelper.FilterApplications(ApplicationHelper.LoadApplications(applicationsWhere), CurrentUser, false);
        
        return applications.Tables[0].Rows.Cast<DataRow>().Select(row => new UIElementInfo(row)).ToList();
    }


    /// <summary>
    /// Gets HTML list code representing the <paramref name="uiElement"/> where can visitors go after the import has finished.
    /// </summary>
    /// <param name="uiElement">UI element the caption, icon and link are loaded from</param>
    /// <param name="description">Description of the element (i.e. some clarification what the application is good for)</param>
    /// <returns>HTML list code representing the <paramref name="uiElement"/> with given <paramref name="description"/></returns>
    private string GetContinueToSmartTipListContent(UIElementInfo uiElement, string description)
    {
        string applicationUrl = UIContextHelper.GetApplicationUrl(UIContextHelper.GetResourceName(uiElement.ElementResourceID), uiElement.ElementName);
        return string.Format(@"
<li>
    <div class=""om-import-csv-next-steps-initial"">
        <i aria-hidden=""true"" class=""{0} cms-icon-100""></i>
    </div>
    <div class=""om-import-csv-next-steps-description"">
        <p class=""lead"">
            <a href=""{1}"" target=""_blank"">{2}</a>
        </p>
        <p>
            {3}
        </p>
    </div>
</li>", uiElement.ElementIconClass, URLHelper.ResolveUrl(applicationUrl), MacroResolver.Resolve(uiElement.ElementCaption), description);
    }


    private void PrepareTooltips()
    {
        var downloadErrorCSVToolTip = GetString("om.contact.importcsv.downloaderrorcsvtooltip");
        labelDownloadErrorCSV.Text = downloadErrorCSVToolTip;
        iconDownloadErrorCSV.ToolTip = downloadErrorCSVToolTip;

        // Ensures that bootstrap scripts are included in page. This is needed because of the tooltips on the page.
        ScriptHelper.RegisterBootstrapScripts(Page);
    }


    private IEnumerable<object> PrepareCategoriesAndFields(Dictionary<FormCategoryInfo, List<FormFieldInfo>> categoriesAndFields)
    {
        var categoriesList = new List<object>();

        foreach (var category in categoriesAndFields)
        {
            var categoryMembers = category.Value.Select(formField => new
            {
                formField.Name,
                DisplayName = ResHelper.LocalizeString(formField.GetDisplayName(MacroResolver.GetInstance()))
            });

            string categoryName = string.IsNullOrEmpty(category.Key.CategoryName) ? GetString("om.contact.importcsv.nofieldcategory") : ResHelper.LocalizeString(category.Key.GetPropertyValue(FormCategoryPropertyEnum.Caption, MacroResolver.GetInstance()));

            categoriesList.Add(new
            {
                CategoryName = categoryName,
                CategoryMembers = categoryMembers
            });
        }
        return categoriesList;
    }


    private bool CheckSiteAndPermissions()
    {
        if (CurrentSite == null)
        {
            ShowInformation(GetString("om.contact.importcsv.selectsite"));
            return false;
        }

        if (!MembershipContext.AuthenticatedUser.IsAuthorizedPerUIElement("CMS.OnlineMarketing", new[] { "ContactsFrameset", "ContactImport" }, CurrentSite.SiteName))
        {
            RedirectToUIElementAccessDenied("CMS.OnlineMarketing", "ContactsFrameset;ContactImport");
        }

        string[] requiredPermissions =
            {
                "ReadContacts",
                "ModifyContacts",
                "ReadContactGroups", 
                "ModifyContactGroups"
            };

        foreach (var permission in requiredPermissions)
        {
            if (!CurrentUser.IsAuthorizedPerResource(ModuleName.CONTACTMANAGEMENT, permission, CurrentSite.SiteName))
            {
                RedirectToAccessDenied(ModuleName.CONTACTMANAGEMENT, permission);
            }
        }

        return true;
    }
}