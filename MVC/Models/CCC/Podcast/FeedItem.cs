using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CCC.Models.Podcast
{
    /// <summary>
    /// Represents an item in a web feed
    /// </summary>
    public class FeedItem
    {
        #region Constructor

        /// <summary>
        /// Constructs a feed item with a required title. Valid RSS.
        /// </summary>
        /// <param name="title">The title of the feed item</param>
        public FeedItem(string title)
        {
            Feed.CheckRequiredValue(title, "title");
            this.Title = title;
        }

        /// <summary>
        /// Constructs a feed item with a required title and link. Valid RSS and Atom.
        /// </summary>
        /// <param name="title">The title of the feed item</param>
        public FeedItem(string title, string link)
            : this(title)
        {
            Feed.CheckRequiredValue(link, "link");
            this.Link = link;
        }

        #endregion

        #region Public properties

        /// <summary>
        /// The title of the feed item
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// The description shown in the feed item
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// The url to the feed item
        /// </summary>
        public string Link { get; set; }
        /// <summary>
        /// The date of the feed item
        /// </summary>
        public DateTime? PublishDate { get; set; }
        /// <summary>
        /// The author of the feed item
        /// </summary>
        public FeedEmailAddress Author { get; set; }
        /// <summary>
        /// An enclosed item (sound, movie, image, pdf file, etc.)
        /// </summary>
        public FeedItemEnclosure Enclosure { get; set; }
        /// <summary>
        /// Category name
        /// </summary>
        public string Category { get; set; }
        /// <summary>
        /// Url of image to display together with the item, primarily in iTunes
        /// </summary>
        public string ImageUrl { get; set; }

        #endregion
    }
}