using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using CCC.Helpers;

namespace CMS.DocumentEngine.Types
{
    /// <summary>
    /// Represents a content item of type StaffTeam.
    /// </summary>
    public partial class StaffTeam : TreeNode
    {
        /// <summary>
        /// Gets the team members of the staff team
        /// </summary>
        public DocumentQuery<StaffTeamMember> TeamMembers
        {
            get
            {
                return StaffTeamMemberProvider.GetStaffTeamMembers()
                                              .OnCurrentSite()
                                              .Path(NodeAliasPath, PathTypeEnum.Children)
                                              .NestingLevel(1)
                                              .OrderBy(CCC.Helpers.DatabaseHelpers.OrderByCmsTree)
                                              .Published();
            }
        }

        /// <summary>
        /// Route Values for the Staff Team
        /// </summary>
        public object RouteValues
        {
            get
            {
                return new
                {
                    controller = "StaffTeam",
                    action = "Index",
                    staffTeam = NodeAlias
                };
            }
        }

        /// <summary>
        /// Returns the Route for the Staff Team
        /// </summary>
        public string RouteUrl
        {
            get
            {
                return UrlHelpers.UrlHelper.RouteUrl(RouteValues);
            }
        }
    }
}