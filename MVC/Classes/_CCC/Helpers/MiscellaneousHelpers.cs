using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using CMS.Helpers;
using CMS.CustomTables.Types;

namespace CCC.Helpers
{
    public static partial class MiscellaneousHelpers
    {
        /// <summary>
        /// The Current Campus Name based on subdomain, query string or cookie.
        /// </summary>
        public static string CurrentCampusName
        {
            get
            {
                string currentCampus = null;

                //Check Subdomain
                string subdomain = UrlHelpers.CurrentSubdomain;
                if (!String.IsNullOrWhiteSpace(subdomain))
                {
                    //check for beta
                    if (subdomain != "beta")
                    {
                        currentCampus = subdomain;
                    }
                }

                //Check QueryString
                if (QueryHelper.Contains("campus"))
                {
                    currentCampus = QueryHelper.GetString("campus", "");
                }

                //Check Cookie
                if (CookieHelper.ResponseCookieExists("CCChapel"))
                {
                    currentCampus = CookieHelper.GetExistingCookie("CCChapel").Values["DefaultCampus"];
                }

                //Return result
                return currentCampus;
            }
        }

        /// <summary>
        /// The Current Campus based on subdomain, query string or cookie. Returns an empty CampusesItem object
        /// when null.
        /// </summary>
        public static CampusesItem CurrentCampus
        {
            get
            {
                if (!String.IsNullOrWhiteSpace(CurrentCampusName))
                {
                    return CampusesItem.GetFromName(CurrentCampusName);
                }

                return new CampusesItem();
            }
        }
    }
}