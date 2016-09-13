using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using CMS.DocumentEngine.Types;

namespace MVC.Controllers
{
    public class JobsController : Controller
    {
        // GET: Jobs
        [OutputCache(CacheProfile = "Cache15min")]
        public ActionResult Index(string jobTitle)
        {
            if (string.IsNullOrWhiteSpace(jobTitle))
            {
                //Job List Page
                return View("Index");
            }
            else
            {
                //Job Details Page
                var model = (from j in JobListingProvider.GetJobListings().Published()
                             where j.NodeAlias == jobTitle
                             select j);

                if (model.Any())
                {
                    return View("JobListing", model.FirstOrDefault());
                }
                else
                {
                    throw new HttpException(404, "Page Not Found");
                }
            }
        }
    }
}