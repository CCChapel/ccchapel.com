using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using CCC.Helpers;

using CMS.CustomTables;
using CMS.CustomTables.Types;
using ChurchCommunityBuilder.Api.Groups.Entity;

namespace CCB
{
    public static partial class Groups
    {
        public static readonly string CACHING_PREFIX = "CcbGroupDetails";

        public static bool IsGroupInCache(int id)
        {
            string cacheID = CachingHelpers.CachingID(CACHING_PREFIX, id);

            return CachingHelpers.Cache.Contains(cacheID);
        }

        /// <summary>
        /// Get the full CCB Group Data
        /// </summary>
        /// <param name="ccbID">The ID of the group</param>
        /// <returns>A Group object representing all group data</returns>
        public static Group GetGroupDetails(int ccbID)
        {
            string cacheID = CachingHelpers.CachingID(CACHING_PREFIX, ccbID);

            if (CachingHelpers.Cache.Contains(cacheID))
            {
                //Get Data From Cache
                Group group =
                    (Group)CachingHelpers.Cache.Get(cacheID);

                return group;
            }
            else
            {
                //Get Data From CCB & Add to Cache
                Group group = Api.Client.Groups.GroupProfiles.Get(ccbID);

                CachingHelpers.Cache.Add(cacheID, group, CachingHelpers.Policy);

                return group;
            }
        }

        /// <summary>
        /// Get Group information
        /// </summary>
        /// <param name="groupID">The ID of the group to get</param>
        /// <returns>Information for the specified group</returns>
        public static GroupsItem GetGroup(int ccbID)
        {
            //Check Kentico First
            if (GroupsItem.Exists(ccbID) == true)
            {
                //Group Exists in Kentico -> Do Nothing
            }
            else
            {
                //Group Doesn't Exist in Kentio -> Get Group from CCB
                Group group = Api.Client.Groups.GroupProfiles.Get(ccbID);

                //Add Group to Kentico
                group.AddToKentico();
            }

            //Get & Return Kentico Item
            var groupInfo = CustomTableItemProvider.GetItems<GroupsItem>()
                    .Where(g => g.CcbID == ccbID);

            if (groupInfo.Any())
            {
                return groupInfo.First();
            }
            else
            {
                return new GroupsItem();
            }
        }
    }

    public static partial class Extension
    {
        /// <summary>
        /// Add Group item to Kentico Database
        /// </summary>
        /// <param name="input">CCB API Group Item</param>
        public static void AddToKentico(this ChurchCommunityBuilder.Api.Groups.Entity.Group input, bool checkForDuplicates = true)
        {
            bool add = true;

            if (checkForDuplicates == true)
            {
                //Check if group already exists
                if (input.ExistsInKentico() == true)
                {
                    add = false;
                }
            }

            if (add == true)
            {
                //Create new GroupsItem
                GroupsItem groupsItem = new GroupsItem(
                    input.ID,
                    input.Name,
                    input.Campus.CCBID ?? default(int));

                groupsItem.AddToKentico();
            }
        }

        /// <summary>
        /// Check if the CCB Group has a corresponding representation in Kentico
        /// </summary>
        /// <param name="input">CCB Group</param>
        /// <returns>Returns true if an entry with the same CCB ID exists in the Kentico Custom Table</returns>
        public static bool ExistsInKentico(this ChurchCommunityBuilder.Api.Groups.Entity.Group input)
        {
            return GroupsItem.Exists(input.ID);
        }
    }
}