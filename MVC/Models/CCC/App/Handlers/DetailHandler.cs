using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Newtonsoft.Json;

using CCC.Models.App.Objects;
using CCC.Models.App.Items;

namespace CCC.Models.App.Handlers
{
    public partial class DetailHandler : Handler
    {
        /// <summary>
        /// Title text content for this Detail screen.
        /// </summary>
        [JsonProperty(Order = 10)]
        public string Title { get; set; }

        /// <summary>
        /// Subtitle text content for this Detail screen.
        /// </summary>
        [JsonProperty(Order = 11, NullValueHandling = NullValueHandling.Ignore)]
        public string Subtitle { get; set; }

        /// <summary>
        /// Defines the main text content used for this Detail screen, such as a long-form description.
        /// </summary>
        [JsonProperty(Order = 12)]
        public string Body { get; set; }

        /// <summary>
        /// An array of IMAGE objects defining image resources for the Detail screen. If more than one image object is provided, the device will choose the image size which is most appropriate for the current visual context on that specific device.
        /// </summary>
        [JsonProperty(Order = 13)]
        public IEnumerable<Image> Images { get; set; }

        protected ActionSheet _actionSheet = new ActionSheet();
        /// <summary>
        /// Simple container object holding a single array of ITEMs. Items defined here are used to populate the sharing menu, allowing the user to share content on various services (Email, SMS, Facebook, Twitter, etc).
        /// In most cases, the actionSheet defines exactly two items, with handlers = defaultShare and htmlShare.
        /// </summary>
        [JsonProperty(Order = 14)]
        public ActionSheet ActionSheet
        {
            get
            {
                return _actionSheet;
            }
            set
            {
                _actionSheet = value;
            }
        }
    }
}