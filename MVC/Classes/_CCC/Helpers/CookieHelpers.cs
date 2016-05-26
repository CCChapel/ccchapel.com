using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CCC.Helpers
{
    /// <summary>
    /// Summary description for Cookies
    /// </summary>
    public static class CookieHelpers
    {
        /* Cookie:  CCChapel
         * Values:  defaultCampus - Records the id of the last campus visited by the user
         *          lastVisit - Records the DateTime of the user's last visit
         * Expires: 1 Year
         *         
         * Cookie:  CCChapelMobile
         * Value:   useMobile - Set to false when a mobile user selects to view the desktop version
         * Expires: 1 Day
         */

        #region Private Properties
        private static string _domain = "ccchapel.com";
        private static readonly string COOKIE_NAME = "CCChapel";
        #endregion

        #region Public Properties
        public static bool CookieExists
        {
            get
            {
                try
                {
                    if (HttpContext.Current.Request.Cookies[COOKIE_NAME] != null)
                    {
                        return true;
                    }
                }
                catch { }

                return false;
            }
        }

        public static int DefaultCampusID
        {
            get
            {
                int id = -1;
                if (HttpContext.Current.Request.Cookies[COOKIE_NAME] != null)
                {
                    try
                    {
                        id = CMS.Helpers.ValidationHelper.GetInteger(
                            HttpContext.Current.Request.Cookies[COOKIE_NAME]["defaultCampus"], -1);
                    }
                    catch
                    {
                    }
                }

                return id;
            }
        }

        public static DateTime LastVisit
        {
            get
            {
                DateTime lastVisit = DateTime.Now;
                if (HttpContext.Current.Request.Cookies[COOKIE_NAME] != null)
                {
                    try
                    {
                        lastVisit = CMS.Helpers.ValidationHelper.GetDateTime(
                            HttpContext.Current.Request.Cookies[COOKIE_NAME]["lastVisit"], DateTime.Now);
                    }
                    catch
                    {
                    }
                }

                return lastVisit;
            }
        }
        #endregion

        #region Methods
        public static void SetGlobalCookie(int campusID = -1)
        {
            HttpCookie appCookie = new HttpCookie(COOKIE_NAME);
            appCookie.Domain = _domain;
            appCookie.Expires = DateTime.Now.AddYears(1);

            //Try to set Current Campus
            try
            {
                if (campusID >= 0)
                { 
                    appCookie["defaultCampus"] = campusID.ToString();
                }
            }
            catch (Exception)
            {
                //Ignore if unavailable
            }

            appCookie["lastVisit"] = DateTime.Now.ToString();
            HttpContext.Current.Response.SetCookie(appCookie);
        }

        //public static void SetMobileCookie()
        //{
        //    HttpCookie appCookie = new HttpCookie("CCChapelMobile");
        //    appCookie.Domain = _domain;
        //    appCookie.Expires = DateTime.Now.AddDays(1);
        //    appCookie["useMobile"] = "false";

        //    HttpContext.Current.Response.Cookies.Add(appCookie);
        //}

        //public static void DeleteMobileCookie()
        //{
        //    HttpCookie appCookie = new HttpCookie("CCChapelMobile");
        //    appCookie.Domain = _domain;
        //    appCookie.Expires = DateTime.Now.AddDays(-1);
        //    appCookie["useMobile"] = "true";

        //    HttpContext.Current.Response.Cookies.Add(appCookie);
        //}
        #endregion
    }
}