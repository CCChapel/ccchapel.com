using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using CMS.CustomTables;
using CMS.CustomTables.Types;
using ChurchCommunityBuilder.Api.Groups.Entity;

namespace CCB
{
    //public static partial class PublicGroup
    //{
    //}

    #region Extensions
    public static partial class Extensions
    {
        public static Group GroupDetails(this PublicGroup input)
        {
            var groupDetails = Groups.GetGroupDetails(input.ID);

            return groupDetails;
        }

        public static CampusesItem CampusInfo(this PublicGroup input)
        {
            var details = Groups.GetGroup(input.ID);

            return details.CampusInfo();
        }
    }
    #endregion
}