using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using CCC.Helpers;

namespace CMS.DocumentEngine.Types
{
    /// <summary>
    /// Represents a content item of type Person.
    /// </summary>
    public partial class Person : TreeNode
    {
        public string FullName
        {
            get
            {
                return string.Format("{0} {1}", FirstName, LastName);
            }
        }

        /// <summary>
        /// Route Values for the person
        /// </summary>
        public object RouteValues
        {
            get
            {
                return new
                {
                    controller = "StaffTeam",
                    action = "People",
                    name = NodeAlias
                };
            }
        }

        /// <summary>
        /// Returns the Route for the Person
        /// </summary>
        public string RouteUrl
        {
            get
            {
                return UrlHelpers.UrlHelper.RouteUrl(RouteValues);
            }
        }

        //Returns an object of strings containing all of the titles applicable to the person
        public IQueryable<string> Titles
        {
            get
            {
                var titles = (from s in StaffTeamMemberProvider.GetStaffTeamMembers()
                              where s.Fields.PersonNodeID == this.NodeID
                              select s.Fields.Title).Distinct();

                return titles;
            }
        }
    }
}