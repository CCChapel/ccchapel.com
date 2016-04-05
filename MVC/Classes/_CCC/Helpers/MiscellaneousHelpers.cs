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
        public static string CurrentCampusCodeName
        { 
            get
            {
                string currentCampus = null;

                try
                {
                    //Check QueryString
                    //Since QueryString would be manually added, it trumps everything
                    if (QueryHelper.Contains("campus"))
                    {
                        currentCampus = QueryHelper.GetString("campus", "");
                        return currentCampus;
                    }

                    //Check Subdomain
                    string subdomain = UrlHelpers.CurrentSubdomain;
                    if (!string.IsNullOrWhiteSpace(subdomain))
                    {
                        try
                        {
                            //check for beta
                            //if (string.IsNullOrWhiteSpace(currentCampus))
                            //{
                            //}
                            //else if (subdomain != "beta")
                            if (subdomain != "beta")
                            {
                                if (subdomain == "highlandsquare")
                                {
                                    currentCampus = "highland-square";
                                    return currentCampus;
                                }

                                currentCampus = subdomain;

                                return currentCampus;
                            }
                        }
                        catch { }
                    }

                    //Check Cookie
                    if (CookieHelpers.CookieExists)
                    {
                        //Get Campus from Cookie
                        int campusID = CookieHelpers.DefaultCampusID;

                        try
                        {
                            var campus = CampusesItem.GetFromID(campusID);
                            currentCampus = campus.CampusCodeName;

                            return currentCampus;
                        }
                        catch { }
                    }
                }
                catch { }

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
                if (!string.IsNullOrWhiteSpace(CurrentCampusCodeName))
                {
                    
                    return CampusesItem.GetFromCodeName(CurrentCampusCodeName);
                }

                return new CampusesItem();
            }
        }

        /// <summary>
        /// The Current Campus Name
        /// </summary>
        public static string CurrentCampusName
        {
            get
            {
                try
                {
                    if (!string.IsNullOrWhiteSpace(CurrentCampusCodeName))
                    {
                        return CampusesItem.GetFromCodeName(CurrentCampusCodeName).CampusName;
                    }
                }
                catch { }

                return null;
            }
        }
    }
}