using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS.CustomTables.Types
{
    /// <summary>
    /// Represents a content item of type LocationsItem.
    /// </summary>
    public partial class LocationsItem : CustomTableItem
    {
        #region Properties
        public StatesItem LocationState
        {
            get
            {
                return CustomTableItemProvider.GetItem<StatesItem>(this.LocationStateID);
            }
        }
        #endregion
    }
}