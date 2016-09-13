using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC.Controllers
{
    public class MessagesController : Controller
    {
        // GET: Messages
        [OutputCache(CacheProfile = "Cache15min")]
        public ActionResult Index()
        {
            return View();
        }
    }
}