using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using CCC.Helpers;

namespace CMS.DocumentEngine.Types
{
    /// <summary>
    /// Represents a content item of type QuickAccessKeywords.
    /// </summary>
    public partial class QuickAccessKeywords : TreeNode
    {
        public object RouteValues
        {
            get
            {
                return new
                {
                    controller = "Search",
                    action = "Index",
                    query = Keywords
                    //query = HttpUtility.UrlEncode(Keywords)
                };
            }
        }

        public string RouteUrl
        {
            get
            {
                return UrlHelpers.UrlHelper.RouteUrl(RouteValues);
            }
        }
        public override string ToString()
        {
            string path = "";

            if (!string.IsNullOrWhiteSpace(Fields.LinkedPage))
            {
                //Link to Page
                Page page = PageProvider.GetPage(
                    new Guid(Fields.LinkedPage), SiteHelpers.SiteCulture, SiteHelpers.SiteName);

                path = UrlHelpers.UrlHelper.RouteUrl(page.RouteValues);
            }
            else
            {
                //Link to Search with keywords
                path = RouteUrl;
            }

            string html = string.Format("<a href=\"{0}\">{1}</a>",
                path,
                Fields.Keywords);

            return html;
        }
    }

    public static partial class Extensions
    {
        public static string ToHtmlString(this IQueryable<QuickAccessKeywords> input)
        {
            string html = "";

            int i = 1;
            int count = input.Count();
            foreach (var keyword in input)
            {
                if (i > 1)
                {
                    if (i == count)
                    {
                        html += " or ";
                    }
                    else
                    {
                        html += ", ";
                    }
                }

                html += keyword.ToString();

                i++;
            }

            return html;
        }
    }
}