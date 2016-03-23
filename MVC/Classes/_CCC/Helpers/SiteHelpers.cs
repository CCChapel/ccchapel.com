using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CCC.Helpers
{
    public partial class SiteHelpers
    {
        /// <summary>
        /// Returns the default language code of the Kentico Site
        /// </summary>
        public static string SiteCulture
        {
            get
            {
                return "en-US";
            }
        }

        /// <summary>
        /// Returns the Kentico Site Name
        /// </summary>
        public static string SiteName
        {
            get
            {
                return "CCChapelMVC";
            }
        }

        public static string DomainName
        {
            get
            {
                return UrlHelpers.CurrentDomainName;
            }
        }
    }
}