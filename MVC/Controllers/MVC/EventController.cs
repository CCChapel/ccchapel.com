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
        //public string Index(int year, int month, int day, string eventName)
        {
            //Create DateTime
            DateTime date = new DateTime(year, month, day);

            string test = string.Format("<h1>{0}</h1><h2>{1}</h2><br />",
                eventName,
                date.ToShortDateString());

            try
            {
                //Get Kentico Object
                //Kentico Object with Retrieve CCB data and store in the CcbEventData property
                var events = (from e in CcbEventProvider.GetCcbEvents().Published()
                              where e.NodeAlias == eventName
                              select e).ToList();

                test += string.Format("Total Events with Name: {0}<hr />",
                    events.Count);

                //In case there are more than one event with that name,
                //loop through them and match date.
                //
                //foreach loop is required because each event needs to instantiate to
                //get date from CCB data
                for (int i = events.Count - 1; i >= 0; i--)
                {
                    //Get Event
                    var ev = events.ElementAt(i);

                    //test += string.Format("Name: {0}<br/>StartDate: {1}<br />",
                    //    ev.EventName,
                    //    ev.StartDateTime.ToString());

                    if (ev.StartDateTime.Date != date.Date)
                    {
                        events.RemoveAt(i);
                        //test += "NO MATCH! REMOVING...<br />";
                    }

                    //test += "<hr />";
                }

                //test += string.Format("<h3>Event Count: {0}",
                //    events.Count);

                //If there are still more than one event, check for campus
                if (events.Count > 1)
                {
                    //test += "<hr />MORE THAN ONE EVENT<hr />";

                    foreach (var ev in events)
                    {
                        //test += string.Format("Name: {0}<br />Campus: {1}<br />Current Campus: {2}",
                        //    ev.EventName,
                        //    ev.CampusInfo().CampusCodeName,
                        //    MiscellaneousHelpers.CurrentCampusCodeName);

                        if (ev.CampusInfo().CampusCodeName.ToLower() == MiscellaneousHelpers.CurrentCampusCodeName.ToLower())
                        {
                            //test += "MATCHED CAMPUS!!!";
                            return View(ev);
                        }

                        test += "<hr />";
                    }
                }
                else
                {
                    //test += "ONLY ONE EVENT! MATCH!!!";
                    return View(events.First());
                }

                //We haven't found an event with that name and date
                throw new HttpException(404, "Page Not Found");
            }
            catch (InvalidOperationException)
            {
                //test += string.Format("Exception: {0}", ex.Message);
                throw new HttpException(404, "Event Not Found");
            }

            //return test;
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