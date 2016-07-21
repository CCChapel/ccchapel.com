using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using CCC.Models.App.Objects;
using CCC.Models.App.Items;

namespace CCC.Models.App.Handlers
{
    public partial class DetailHandler : Handler
    {
        /// <summary>
        /// Simple container object holding a single array of ITEMs. Items defined here are used to populate the sharing menu, allowing the user to share content on various services (Email, SMS, Facebook, Twitter, etc).
        /// In most cases, the actionSheet defines exactly two items, with handlers = defaultShare and htmlShare.
        /// </summary>
        public IEnumerable<Item> ActionSheet { get; set; }

        /// <summary>
        /// Defines the main text content used for this Detail screen, such as a long-form description.
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// An array of IMAGE objects defining image resources for the Detail screen. If more than one image object is provided, the device will choose the image size which is most appropriate for the current visual context on that specific device.
        /// </summary>
        public IEnumerable<Image> Images { get; set; }

        /// <summary>
        /// Subtitle text content for this Detail screen.
        /// </summary>
        public string Subtitle { get; set; }

        /// <summary>
        /// Title text content for this Detail screen.
        /// </summary>
        public string Title { get; set; }
    }
}