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
        #endregion
    }
}