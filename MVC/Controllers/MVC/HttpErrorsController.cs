using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC.Controllers
{
    public class HttpErrorsController : Controller
    {
        // GET: HttpErrors
        [OutputCache(CacheProfile = "Cache15min")]
        public ActionResult NotFound()
        {
            Response.StatusCode = 404;
            Response.TrySkipIisCustomErrors = true;

            return View();
        }

        //public ActionResult GenericError()
        //{
        //    //Response.TrySkipIisCustomErrors = true;

        //    return View("Error");
        //}
    }
}