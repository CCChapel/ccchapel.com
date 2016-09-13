using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using CCB;
using ChurchCommunityBuilder.Api;

namespace MVC.Controllers.MVC
{
    public class GroupFinderController : Controller
    {
        // GET: GroupFinder
        [OutputCache(CacheProfile = "Cache15min")]
        public ActionResult Index()
        {
            var qo = new ChurchCommunityBuilder.Api.Groups.QueryObject.PublicGroupQO();
            var groups = Api.Client.Groups.PublicGroups.Search(qo);

            return View(groups);
        }
    }
}