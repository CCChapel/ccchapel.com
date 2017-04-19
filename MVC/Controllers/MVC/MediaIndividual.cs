using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using CMS.DocumentEngine.Types;
using CCC.Helpers;

namespace MVC.Controllers
{
    public class MediaIndividualController : Controller
    {
        // GET: Message
        [OutputCache(CacheProfile = "Cache15min")]
        public ActionResult Index(int year, int month, int day, string seriesTitle, string sermonTitle)
        {
            //Create DateTime
            DateTime date = new DateTime(year, month, day);

            //Get Sermon
            var sermon = (from s in SermonProvider.GetSermons().Published()
                          where s.MessageDate == date &&
                                s.MessageSeries.NodeAlias.ToLower() == seriesTitle.ToLower() &&
                                s.NodeAlias.ToLower() == sermonTitle.ToLower()
                          select s).FirstOrDefault();

            return View(sermon);
        }

        [ChildActionOnly]
        [ActionName("CrossSell")]
        public ViewResult CrossSell(string guid, string title, string description)
        {
            //Store Extra Data
            ViewData["crossSellTitle"] = title;
            ViewData["crossSellDescription"] = description;

            //Get Sermon
            var sermon = SermonProvider.GetSermon(
                            new Guid(guid), 
                            SiteHelpers.SiteCulture, 
                            SiteHelpers.SiteName).FirstOrDefault();

            //return "I'm a sermon!";
            return View("_CrossSellItem", sermon);
        }
    }
}