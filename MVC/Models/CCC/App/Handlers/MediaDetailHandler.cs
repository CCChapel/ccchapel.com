using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using CCC.Models.App.Objects;
using CCC.Models.App.Items;
using Newtonsoft.Json;

namespace CCC.Models.App.Handlers
{
    public partial class MediaDetailHandler : DetailHandler
    {
        /// <summary>
        /// An array of strings defining extra subtitle content for this object. This is your secret weapon for those times when one subtitle just isn't enough.
        /// </summary>
        [JsonProperty(Order = 11)]
        public IEnumerable<string> ExtraSubtitles { get; set; }

        /// <summary>
        /// A date, rendered according to the device's native locale settings, and displayed along with extraSubtitles content. If a date is provided, it may reduce the number of extraSubtitles strings which can be displayed on the screen.
        /// </summary>
        [JsonProperty(Order = 11)]
        public DateTime Date { get; set; }

        /// <summary>
        /// An array of audio and/or video MEDIA items. The app will select the appropriate url from the MEDIA objects defined here to match the requested user action and device context.        /// </summary>
        [JsonProperty(Order = 20)]
        public IEnumerable<Media> Media { get; set; }

        /// <summary>
        /// Multiple ITEMGROUPs may be combined to create groupings of buttons or other items. For the Media Detail page in the app, the ITEMGROUP at position = 0 describes buttons which appear above the description text, while the ITEMGROUP at position = 1 describes buttons which appear below the description text.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, Order = 21)]
        public IEnumerable<Item> ItemGroups { get; set; }
    }
}