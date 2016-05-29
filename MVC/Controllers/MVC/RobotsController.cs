using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC.Controllers.MVC
{
    public class RobotsController : Controller
    {
        // GET: Robots
        public ActionResult Index()
        {
            return View("Robots");
        }
    }
}