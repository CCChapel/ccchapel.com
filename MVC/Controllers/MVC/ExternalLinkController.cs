using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC.Controllers.MVC
{
    public class ExternalLinkController : Controller
    {
        // GET: ExternalLink
        public RedirectResult Index(string url)
        {
            //Encode URL and Redirect
            return Redirect(HttpUtility.UrlDecode(url));
        }
    }
}