using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS.DocumentEngine.Types
{
    /// <summary>
    /// Represents a content item of type AmazonMp3.
    /// </summary>
    public partial class AmazonMp3 : TreeNode
    {
        #region Private Properties
        protected string S3BaseUrl = "http://download.communitychapel.com";
        #endregion

        #region Public Properties
        /// <summary>
        /// The full URL to the MP3 file
        /// </summary>
        public string Url
        {
            get
            {
                return string.Format("{0}{1}", S3BaseUrl, Fields.Path);
            }
        }
        #endregion
    }
}