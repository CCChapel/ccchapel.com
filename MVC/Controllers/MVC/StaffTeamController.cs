using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using CCC.Helpers;

using CMS.DocumentEngine.Types;

namespace MVC.Controllers.MVC
{
    public class StaffTeamController : Controller
    {
        // GET: StaffTeam
        public ActionResult Teams(string name)
        {
            //Load Campus Leadership Team By Default
            if (string.IsNullOrWhiteSpace(name))
            {
                switch (MiscellaneousHelpers.CurrentCampusName)
                {
                    case "Hudson":
                        name = "hudson-campus-leadership";
                        break;
                    case "Aurora":
                        name = "aurora-campus-leadership";
                        break;
                    case "Stow":
                        name = "stow-campus-leadership";
                        break;
                    case "Highland Square":
                        name = "highland-square-campus-leadership";
                        break;
                    default:
                        name = "hudson-campus-leadership";
                        break;
                }
            }

            //Get Staff Team
            var team = (from t in StaffTeamProvider.GetStaffTeams().Published()
                        where t.NodeAlias.ToLower() == name.ToLower()
                        select t);

            if (team.Any())
            {
                return View(team.First());
            }
            else
            {
                throw new HttpException(404, "Page Not Found");
            }
        }

        public ActionResult People(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                //If no name is specified, redirect to teams page
                RedirectToAction("Teams");
            }

            var person = (from p in PersonProvider.GetPeople().Published()
                            where p.NodeAlias.ToLower() == name.ToLower()
                            select p);

            if (person.Any())
            {
                return View("Person", person.First());
            }
            else
            {
                throw new HttpException(404, "Page Not Found");
            }
        }
    }
}