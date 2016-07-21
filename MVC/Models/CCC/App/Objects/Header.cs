using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CCC.Models.App.Objects
{
    public partial class Header
    {
        /// <summary>
        /// If style == banner, the ITEMs definied in the LIST will be used to populate the banner image.If more than one ITEM is defined, the app will rotate through the items one at a time.
        /// If style == featured, the first two ITEMs in the LIST will be used to populate the two half-width Featured Items.
        /// If list is null or empty, this value is ignored.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public enum HeaderStyles
        {
            [EnumMember(Value = "banner")]
            Banner,
            [EnumMember(Value = "featured")]
            Featured
        };

        /// <summary>
        /// Defines the title for the entire screen
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// If style == banner, the ITEMs definied in the LIST will be used to populate the banner image.If more than one ITEM is defined, the app will rotate through the items one at a time.
        /// If style == featured, the first two ITEMs in the LIST will be used to populate the two half-width Featured Items.
        /// If list is null or empty, this value is ignored.
        /// </summary>
        public HeaderStyles Style { get; set; }

        /// <summary>
        /// The array of items which compose the HEADER.
        /// </summary>
        public IEnumerable<Items.Item> Items { get; set; }

        /// <summary>
        /// Hex string defining a background color for the HEADER item.
        /// </summary>
        public string Color { get; set; }
    }
}