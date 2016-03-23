using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using Kentico.Search;

using CCC.Helpers;
using CCC.Models;

namespace MVC.Controllers.WebAPI
{
    public class SearchController : ApiController
    {
        // Adds the smart search indexes that will be used when performing a search 
        private static string index = string.Format("{0}.{1}", SiteHelpers.SiteName, "CCChapel");
        public static readonly string[] searchIndexes = new string[] { index };
        private const int PAGE_SIZE = 10;

        private readonly SearchService mService =
            new SearchService(searchIndexes, SiteHelpers.SiteCulture, SiteHelpers.SiteName, false);

        // GET api/<controller>
        public IEnumerable<object> Get(string query)
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

            List<object> list = new List<object>();

            for (int i = 0; i < 5; i++)
            {
                string _title = string.Format("Page {0}", i);
                string _url = string.Format("/my/url/{0}", i);

                list.Add(new { title = _title, url = _url });
            }

            return list;
            //return new string[] { "value1", "value2" };
        }
    }
}