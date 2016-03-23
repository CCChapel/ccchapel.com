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
        // Adds the smart search indexes that will be used when performing a search 
        private static string index = string.Format("{0}.{1}", SiteHelpers.SiteName, "CCChapel");
        public static readonly string[] searchIndexes = new string[] { index };
        private const int PAGE_SIZE = 10;

        private readonly SearchService mService = 
            new SearchService(searchIndexes, SiteHelpers.SiteCulture, SiteHelpers.SiteName, false);

        // GET: Search
        [ValidateInput(false)]
        public ActionResult Index(string query)
        {
            int numberOfResults;
            var model = new SearchResults()
            {
                Items = mService.Search(
                    query,
                    page: 0,
                    pageSize: PAGE_SIZE,
                    numberOfResults: out numberOfResults),
                Query = query,
                ItemCount = numberOfResults
            };
            return View(model);
        }

        public ActionResult Json(string query)
        {
            int numberOfResults;
            var model = new SearchResults()
            {
                Items = mService.Search(
                    query,
                    page: 0,
                    pageSize: PAGE_SIZE,
                    numberOfResults: out numberOfResults),
                Query = query,
                ItemCount = numberOfResults
            };

            return View("JSONResults", model);

            //if (numberOfResults > 0)
            //{
            //    return View("JSONResults", model);
            //}
            //else
            //{
            //    return View("_Empty");
            //}
        }
    }
}