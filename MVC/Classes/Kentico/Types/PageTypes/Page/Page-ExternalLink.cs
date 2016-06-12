using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using CCC.Helpers;

namespace CMS.DocumentEngine.Types
{
    /// <summary>
    /// Represents a content item of type ExternalLink.
    /// </summary>
    public partial class ExternalLink : TreeNode
    {
        /// <summary>
        /// Route Values for the External Link
        /// </summary>
        public object RouteValues
        {
            get
            {
                return new
                {
                    controller = "ExternalLink",
                    action = "Index",
                    url = Fields.URL
                };
            }
        }

        /// <summary>
        /// Get a URL to the External Link
        /// </summary>
        public string RouteUrl
        {
            get
            {
                return UrlHelpers.UrlHelper.RouteUrl(RouteValues);
            }
        }
    }
}