using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

using Kentico.Search;

using CCC.Helpers;
using CCC.Models;

namespace MVC.Controllers.WebAPI
{
    public class SearchController : ApiController
    {
        // GET api/<controller>
        public SearchResults.AjaxSearchResults Get(string query)
        {
            query = HttpUtility.UrlDecode(query);

            return new SearchResults(query).AjaxResults;
        }

        public SearchResults.AjaxSearchResults Get(string query, int maxResults)
        {
            query = HttpUtility.UrlDecode(query);

            return new SearchResults(query).AjaxResults;
        }
    }
}