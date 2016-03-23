using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using CCC.Helpers;

namespace CMS.CustomTables.Types
{
    /// <summary>
    /// Represents a content item of type GroupsItem.
    /// </summary>
    public partial class GroupsItem : CustomTableItem
    {
        #region Methods
        /// <summary>
        /// Creates the appropriate Dictionary item for all Field values
        /// </summary>
        /// <returns>Dictionary of fields and values</returns>
        protected Dictionary<string, object> GenerateCustomTableValues()
        {
            //Create Dictionary
            Dictionary<string, object> values = new Dictionary<string, object>();

            values.Add("CcbID", this.Fields.CcbID);
            values.Add("Name", Fields.Name);
            values.Add("CampusCcbID", Fields.CampusCcbID);

            return values;
        }

        /// <summary>
        /// Adds GroupsItem to Kentico
        /// </summary>
        public void AddToKentico()
        {
            DatabaseHelpers.CustomTables.AddRecord(CLASS_NAME, GenerateCustomTableValues());
        }

       /// <summary>
        /// Checks if an entry with the given CCB ID exists in the Custom Table
        /// </summary>
        /// <param name="ccbID">CCB ID to check</param>
        /// <returns>Returns true if an entry with that CCB ID exists in the Custom Table</returns>
        public static bool Exists(int ccbID)
        {
            var existingGroup = CustomTableItemProvider.GetItems<GroupsItem>()
                    .Where(g => g.CcbID == ccbID)
                    .Count();

            if (existingGroup > 0)
            {
                return true;
            }

            return false;
        }
        #endregion

        #region Constructor
        public GroupsItem(long ccbID, string name, int campusCcbID) : base(CLASS_NAME)
        {
            mFields = new GroupsItemFields(this);

            //Set Field Values
            CcbID = ccbID;
            Name = name;
            CampusCcbID = campusCcbID;
        }
        #endregion
    }
}