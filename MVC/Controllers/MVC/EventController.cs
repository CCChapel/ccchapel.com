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
                var events = (from e in CcbEventProvider.GetCcbEvents().Published()
                              where e.NodeAlias == eventName
                              select e).ToList();

                //In case there are more than one event with that name,
                //loop through them and match date.
                //
                //foreach loop is required because each event needs to instantiate to
                //get date from CCB data
                foreach (var ev in events)
                {
                    if (ev.StartDateTime.Date != date.Date)
                    {
                        events.Remove(ev);
                    }
                }

                //If there are still more than one event, check for campus
                if (events.Count > 1)
                {
                    foreach (var ev in events)
                    {
                        if (ev.CampusInfo().CampusCodeName.ToLower() == MiscellaneousHelpers.CurrentCampusCodeName.ToLower())
                        {
                            return View(ev);
                        }
                    }
                }
                else
                {
                    return View(events.First());
                }

                //We haven't found an event with that name and date
                throw new HttpException(404, "Page Not Found");
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