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
        public ActionResult Teams(string staffTeam)
        {
            //Load Campus Leadership Team By Default
            if (string.IsNullOrWhiteSpace(staffTeam))
            {
                switch (MiscellaneousHelpers.CurrentCampusName)
                {
                    case "Hudson":
                        staffTeam = "hudson-campus-leadership";
                        break;
                    case "Aurora":
                        staffTeam = "aurora-campus-leadership";
                        break;
                    case "Stow":
                        staffTeam = "stow-campus-leadership";
                        break;
                    case "Highland Square":
                        staffTeam = "highland-square-campus-leadership";
                        break;
                    default:
                        staffTeam = "hudson-campus-leadership";
                        break;
                }
            }

            //Get Staff Team
            var team = (from t in StaffTeamProvider.GetStaffTeams().Published()
                        where t.NodeAlias.ToLower() == staffTeam.ToLower()
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
    }
}