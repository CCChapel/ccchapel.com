using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using CCC.Helpers;
using CCC.Models;
using CMS.DocumentEngine;
using CMS.DocumentEngine.Types;
using CMS.Helpers;

namespace MVC.Controllers
{
    public class EventsController : Controller
    {
        // GET: Events
        [OutputCache(CacheProfile = "Cache15min")]
        public ActionResult Index(string startDate, string endDate)
        {
            DateTime start = DateTime.Now;

            EventsPage eventPage = new EventsPage(start);

            if (!string.IsNullOrWhiteSpace(startDate) && !string.IsNullOrWhiteSpace(endDate))
            {
                start = ValidationHelper.GetDate(startDate, DateTime.Now);
                DateTime end = ValidationHelper.GetDate(endDate, start.Add(EventsPage.EventList.defaultSpan));

                eventPage = new EventsPage(start, end);
            }
            else if (!string.IsNullOrWhiteSpace(startDate))
            {
                start = ValidationHelper.GetDate(startDate, DateTime.Now);

                eventPage = new EventsPage(start);
            }

            return View(eventPage);
        }
    }
}