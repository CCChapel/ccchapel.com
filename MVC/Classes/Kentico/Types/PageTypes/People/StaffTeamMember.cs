using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using CCC.Helpers;

namespace CMS.DocumentEngine.Types
{
    /// <summary>
    /// Represents a content item of type StaffTeamMember.
    /// </summary>
    public partial class StaffTeamMember : TreeNode
    {
        public Person Person
        {
            get
            {
                return PersonProvider.GetPerson(PersonNodeID, SiteHelpers.SiteCulture, SiteHelpers.SiteName);
            }
        }
    }
}