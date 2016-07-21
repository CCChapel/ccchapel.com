using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CCC.Models.App.Items
{
    public partial class ListItem : Item
    {
        /// <summary>
        /// If images and position are both null or empty, the app will display a localized date string in place of the normal image content for this item.
        /// </summary>
        public DateTime? Date { get; set; }

        /// <summary>
        /// If images is null or empty, the app will display this string in place of the normal image content for the item. This property is a content value only, and no object sorting is done based on this value.
        /// Note: while this value is often an integer string, it does not have to be a number and can be any short string.
        /// </summary>
        public string Position { get; set; }
    }
}