using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using CMS.DocumentEngine.Types;
using CCC.Helpers;

namespace MVC.Controllers
{
    public class MediaSeriesController : Controller
    {
        // GET: MessageSeries
        [OutputCache(CacheProfile = "Cache15min")]
        public ActionResult Index(string seriesTitle)
        {
            //Get Sermon
            var series = (from s in SeriesProvider.GetSeries().Published()
                          where s.NodeAlias.ToLower() == seriesTitle.ToLower()
                          select s).FirstOrDefault();

            return View(series);
        }

        [ChildActionOnly]
        public ViewResult _SeriesTable(Series series)
        {
            return View("_SeriesTable", series);
        }

        [ChildActionOnly]
        [ActionName("CrossSell")]
        public ViewResult CrossSell(string guid, string title, string description)
        {
            //Store Extra Data
            ViewData["crossSellTitle"] = title;
            ViewData["crossSellDescription"] = description;

            //Get Series
            var series = SeriesProvider.GetSeries(
                            new Guid(guid),
                            SiteHelpers.SiteCulture,
                            SiteHelpers.SiteName).FirstOrDefault();

            //return "I'm a sermon!";
            return View("_CrossSellItem", series);
        }
    }
}