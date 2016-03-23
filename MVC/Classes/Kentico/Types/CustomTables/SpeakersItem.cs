using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS.CustomTables.Types
{
    /// <summary>
    /// Represents a content item of type SpeakersItem.
    /// </summary>
    public partial class SpeakersItem : CustomTableItem
    {
        #region Public Properties
        public string SpeakerFullName
        {
            get
            {
                return String.Format("{0} {1}", SpeakerFirstName, SpeakerLastName);
            }
        }
        #endregion
    }
}