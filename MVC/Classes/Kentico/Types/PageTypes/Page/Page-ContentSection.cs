using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using CCC.Helpers;

namespace CMS.DocumentEngine.Types
{
    public partial class ContentSection : TreeNode
    {
        /// <summary>
        /// Route Values for the ContentSection
        /// </summary>
        public object RouteValues
        {
            get
            {
                return new
                {
                    controller = "PageSection",
                    action = "Index",
                    section = this
                };
            }
        }

        /// <summary>
        /// Get a URL to the ContentSection
        /// </summary>
        public string RouteUrl
        {
            get
            {
                //Get Parent Page
                Page page = PageProvider.GetPage(
                    this.Parent.NodeGUID,
                    SiteHelpers.SiteCulture,
                    SiteHelpers.SiteName);
                string url = page.PageUrl;

                //Add anchor
                url += string.Format("#{0}", NodeAlias.ToLower());

                return url;
            }
        }
    }
}