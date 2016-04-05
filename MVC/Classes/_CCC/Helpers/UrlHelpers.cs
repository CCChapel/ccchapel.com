using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CCC.Helpers
{
    public static partial class UrlHelpers
    {
        public static UrlHelper UrlHelper
        {
            get
            {
                return new UrlHelper(HttpContext.Current.Request.RequestContext);
            }
        }

        public static string CurrentSubdomain
        {
            get
            {
                string subdomain = null;

                Uri url = CMS.Helpers.RequestContext.URL;

                if (url.HostNameType == UriHostNameType.Dns)
                {
                    string host = url.Host;

                    var nodes = host.Split('.');
                    if (nodes.Length < 3)
                    {
                        //No Subdomain
                        subdomain = null;
                    }
                    else
                    {
                        int startNode = 0;
                        if (nodes[0] == "www")
                        {
                            startNode = 1;
                        }

                        subdomain = string.Format("{0}", nodes[startNode]).ToLower();
                    }
                }

                return subdomain;
            }
        }

        public static string CurrentDomainName
        {
            get
            {
                Uri uri = CMS.Helpers.RequestContext.URL;

                if (uri.HostNameType == UriHostNameType.Dns)
                {
                    return string.Format("{0}{1}{2}",
                        uri.Scheme,
                        Uri.SchemeDelimiter,
                        uri.Host);
                }

                return null;
            }
        }

        public static string CurrentRoute
        {
            get
            {
                Uri uri = CMS.Helpers.RequestContext.URL;

                if (uri.HostNameType == UriHostNameType.Dns)
                {
                    return uri.AbsolutePath;
                }

                return null;
            }
        }
    }
}