using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Newtonsoft.Json;

namespace CCC.Models.App
{
    /// <summary>
    /// CONTENT feed objects are like HANDLER objects, except that they are intended to describe auxiliary content within a host 
    /// page rather than defining the entire content of a page. An ACTION object with type == content can be used to query CONTENT 
    /// feeds in cases where there is too much item-level data to realistically include it in the host feed.
    /// </summary>
    public abstract partial class Content
    {
        /// <summary>
        /// Title text for this content.
        /// </summary>
        [JsonProperty(Order = 1)]
        public string Title { get; set; }

        /// <summary>
        /// Content describing HTML data to be loaded in a browser. Using an ACTION with handler = browser, style = internal, 
        /// and type = content enables us to specify a web page to be loaded in an Internal Browser along with title/sharing 
        /// data for that web page.
        /// </summary>
        public partial class BrowserContent : Content
        {
            /// <summary>
            /// Simple container object holding a single array of ITEMs. Items defined here are used to populate the sharing menu, allowing the user to share content on various services (Email, SMS, Facebook, Twitter, etc).
            /// </summary>
            public Objects.ActionSheet actionSheet { get; set; }

            /// <summary>
            /// Url pointing to the HTML data to be loaded in the browser.
            /// </summary>
            public string ContentUrl { get; set; }
        }

        /// <summary>
        /// Content describing image detail for the photo carousel. This content is loaded when the user clicks on a thumbnail, or swipes to a new item in the carousel list.
        /// </summary>
        public partial class CarouselContent : Content
        {
            /// <summary>
            /// Text content to be displayed alongside (or over top of) the image.
            /// </summary>
            [JsonProperty(Order = 10)]
            public string Body { get; set; }

            /// <summary>
            /// An array of IMAGE objects, defining full-resolution images to be downloaded after then user has clicked on a thumbnail.
            /// </summary>
            [JsonProperty(Order = 11)]
            public IEnumerable<Objects.Image> Images { get; set; }

            /// <summary>
            /// An array of ITEM objects describing additional actionable or non-actionable content associated with this image in the photo carousel.
            /// </summary>
            [JsonProperty(Order = 12)]
            public IEnumerable<Items.Item> Items { get; set; }
        }
    }
}