using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace CCC.Models.App.Handlers
{
    /// <summary>
    /// Describes a list of items which may be rendered in a variety of styles. The List Handler is the most common feed object used by the Subsplash apps.
    /// </summary>
    public partial class ListHandler : Handler
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public enum GridLayouts
        {
            [EnumMember(Value = "square")]
            Square
        }

        /// <summary>
        /// When grid-titleposition = below, the title will appear below the grid image.
        /// When grid-titleposition = overlay, the title will appear over top of the grid image.
        /// When grid-titleposition = none, the title will not appear.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public enum GridTitlePositions
        {
            [EnumMember(Value = "below")]
            Below,
            [EnumMember(Value = "overlay")]
            Overlay,
            [EnumMember(Value = "none")]
            None
        }

        /// <summary>
        /// An array of ITEM objects to be displayed to the user. These items will be rendered according to the style specified in the LISTACTION object used to trigger this feed.
        /// </summary>
        [JsonProperty(Order = 10)]
        public string Title { get; set; }

        /// <summary>
        /// Some list styles such as reader include support for locally tracking which items a user has visited in the past, and representing these items in a visually distinct way. Setting trackViewedItems = true enables this tracking behavior.
        /// </summary>
        [JsonProperty(Order = 11)]
        public bool TrackViewedItems { get; set; }

        /// <summary>
        /// An array of ITEM objects to be displayed to the user. These items will be rendered according to the style specified in the LISTACTION object used to trigger this feed.
        /// </summary>
        [JsonProperty(Order = 12)]
        public IEnumerable<Items.Item> Items { get; set; }

        /// <summary>
        /// This property is valid only when the triggering LISTACTION style = grid. At this time, the only valid value is square.
        /// </summary>
        [JsonProperty(PropertyName = "grid-layout", Order = 13)]
        public GridLayouts GridLayout { get; set; }

        /// <summary>
        /// This property is valid only when the triggering LISTACTION style = grid.
        /// </summary>
        [JsonProperty(PropertyName = "grid-titleposition", Order = 14)]
        public GridTitlePositions GridTitlePosition { get; set; }
    }
}