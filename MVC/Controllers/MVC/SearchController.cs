using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Kentico.Search;

using CCC.Helpers;
using CCC.Models;

namespace MVC.Controllers
{
    public class SearchController : Controller
    {
        // GET: Search
        [ValidateInput(false)]
        public ActionResult Index(string query)
        {
            var model = new SearchResults(query);

            return View(model);
        }
    }
}