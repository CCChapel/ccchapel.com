using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using CMS.DocumentEngine.Types;

using CCC.Helpers;


namespace MVC.Controllers
{
    public class PageSectionController : Controller
    {
        // GET: PageSection
        [ChildActionOnly]
        public ViewResult Index(CMS.DocumentEngine.TreeNode section)
        {
            if (section.ClassName == "pages.crossSellSection")
            {
                //Get and cast section
                CrossSellSection crossSell = CrossSellSectionProvider.GetCrossSellSection(section.NodeAliasPath, SiteHelpers.SiteCulture, SiteHelpers.SiteName)
                    .Published();

                //Check that Macro Condition is true
                if (DocumentHelpers.ResolveMacroCondition(crossSell.Fields.SectionMacroCondition))
                {
                    return View("_CrossSell", crossSell);
                }
            }
            else if (section.ClassName == "pages.imageSection")
            {
                //Get and cast section
                ImageSection image = ImageSectionProvider.GetImageSection(section.NodeAliasPath, SiteHelpers.SiteCulture, SiteHelpers.SiteName)
                    .Published();

                //Check that Macro Condition is true
                if (DocumentHelpers.ResolveMacroCondition(image.Fields.SectionMacroCondition))
                {
                    return View("_Image", image);
                }
            }
            else if (section.ClassName == "pages.contentSection")
            {
                //Get and cast section
                ContentSection content = ContentSectionProvider.GetContentSection(section.NodeAliasPath, SiteHelpers.SiteCulture, SiteHelpers.SiteName)
                    .Published();

                //Check that Macro Condition is true
                if (DocumentHelpers.ResolveMacroCondition(content.Fields.SectionMacroCondition))
                {
                    return View("_Content", content);
                }
            }
            //else if (section.ClassName == "pages.formStackSection")
            //{
            //    FormStackSection formStack = FormStackSectionProvider.GetFormStackSection(section.NodeAliasPath, SiteHelpers.SiteCulture, SiteHelpers.SiteName)
            //        .Published();

            //    if (DocumentHelpers.ResolveMacroCondition(formStack.Fields.SectionMacroCondition))
            //    {
            //        return View("_FormStack", formStack);
            //    }
            //}

            return View("_Empty");
            //throw new NotImplementedException("A view for this type of section is not available.");
        }
    }
}