using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using CCC.Helpers;

namespace CMS.CustomTables.Types
{
    public partial class CampusesItem : CustomTableItem
    {
        #region Properties
        public string CampusSQLCodeName
        {
            get
            {
                return CampusCodeName.Replace("-", string.Empty);
            }
        }
        #endregion

        #region Methods
        public static CampusesItem GetFromName(string campusName)
        {
            return CustomTableItemProvider.GetItems<CampusesItem>()
                .Where(x => x.CampusName == campusName)
                .FirstOrDefault();
        }

        public static CampusesItem GetFromCodeName(string campusCodeName)
        {
            return CustomTableItemProvider.GetItems<CampusesItem>()
                .Where(x => x.CampusCodeName == campusCodeName)
                .FirstOrDefault();
        }
        
        public static CampusesItem GetFromID(int id)
        {
            var campus = CustomTableItemProvider.GetItems<CampusesItem>()
                .Where(x => x.ItemID == id);

            if (campus.Any())
            {
                return campus.First();
            }

            throw new NullReferenceException();
        }
        #endregion
    }
}