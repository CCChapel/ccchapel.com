using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CCC.Models.App.Objects
{
    public partial class Image
    {
        /// <summary>
        /// Hex string defining an associated color for the IMAGE. This is often the pre-computed average color of the image, and may be used as a background while the image loads or as a semi-transparent overlay.
        /// </summary>
        public string Color { get; set; }

        /// <summary>
        /// Defines the location from which to load the image.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Provides the precomputed width of the image, allowing for efficient image selection by the device. Example: If more than one IMAGE is provided in an ITEM's images array, the device will choose the image size which is most appropriate for the current visual context on that specific device.
        /// </summary>
        public int Width { get; set; }
    }
}