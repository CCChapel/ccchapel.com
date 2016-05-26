using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CCC.Models.Podcast
{
    /// <summary>
    /// Represents a document (sound, movie, image, pdf file, etc.) enclosed with a feed item
    /// </summary>
    public class FeedItemEnclosure
    {
        #region Constructors

        public FeedItemEnclosure(string url, string mimeType, int length)
        {
            Feed.CheckRequiredValue(url, "url");
            Feed.CheckRequiredValue(mimeType, "mimeType");
            this.Url = url;
            this.MimeType = mimeType;
            this.Length = length;
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Url to the enclosed item
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Length (in bytes) of the enclosed item
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// Mime type of the enclosed item
        /// </summary>
        /// <example>
        /// "image/jpeg" (.jpg, .jpeg)
        /// "audio/mpeg" (.mp3)
        /// "audio/x-ms-wma" (.wma)
        /// "audio/vnd.rn-realaudio" (.ra, .rm)
        /// "audio/x-wav" (.wav)
        /// "video/mp4" (.mp4)
        /// "video/x-ms-wmv" (.wmv)
        /// "application/pdf" (.pdf)
        /// "application/msword" (.doc)
        /// "application/vnd.ms-excel" (.xls)
        /// "application/vnd.ms-powerpoint" (.ppt)
        /// </example>
        public string MimeType { get; set; }

        #endregion
    }
}