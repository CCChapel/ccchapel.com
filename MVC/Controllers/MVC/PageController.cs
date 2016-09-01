using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using CMS.DocumentEngine.Types;
using CCC.Helpers;

namespace MVC.Controllers
{
    public class PageController : Controller
    {
        // GET: Page
        [OutputCache(Duration = 900, VaryByParam = "path")]
        public ActionResult Index(string path)
        {
            //Get Page
            string nodeAliasPath = String.Format("{0}{1}", Page.PathPrefix, path);

            //Throw error if no pages are found
            if (!PageProvider.GetPages()
                    .Where(p => p.NodeAliasPath == nodeAliasPath)
                    .Any())
            {
                throw new HttpException(404, "Page Not Found");
            }

            //Get Page
            Page pageItem = PageProvider.GetPage(nodeAliasPath, "en-US", "CCChapelMVC")
                .OnCurrentSite()
                //.Path("/", CMS.DocumentEngine.PathTypeEnum.Children)
                .Published();

            return View(pageItem);
        }

        [ChildActionOnly]
        [ActionName("CrossSell")]
        [OutputCache(Duration = 900, VaryByParam = "guid")]
        public ViewResult CrossSell(string guid, string title, string description)
        {
            //Store Extra Data
            ViewData["crossSellTitle"] = title;
            ViewData["crossSellDescription"] = description;

            //Get Page
            var page = PageProvider.GetPage(
                            new Guid(guid),
                            SiteHelpers.SiteCulture,
                            SiteHelpers.SiteName).FirstOrDefault();

            return View("_CrossSellItem", page);
        }
    }
}