using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using CMS.DocumentEngine.Types;
using CCC.Helpers;

namespace MVC.Controllers
{
    public class EventController : Controller
    {
        // GET: Event
        public ActionResult Index(int year, int month, int day, string eventName)
        {
            //Create DateTime
            DateTime date = new DateTime(year, month, day);

            try
            {
                //Get Kentico Object
                //Kentico Object with Retrieve CCB data and store in the CcbEventData property
                var ev = (from e in CcbEventProvider.GetCcbEvents().Published()
                          where e.NodeAlias == eventName
                          select e).First();

                return View(ev);
            }
            catch (InvalidOperationException)
            {
                throw new HttpException(404, "Event Not Found");
            }
        }

        [ChildActionOnly]
        [ActionName("CrossSell")]
        public ViewResult CrossSell(string guid, string title, string description)
        {
            //Store Extra Data
            ViewData["crossSellTitle"] = title;
            ViewData["crossSellDescription"] = description;

            //Get Event
            var ev = CcbEventProvider.GetCcbEvent(
                            new Guid(guid),
                            SiteHelpers.SiteCulture,
                            SiteHelpers.SiteName).FirstOrDefault();

            return View("_CrossSellItem", ev);
        }
    }
}