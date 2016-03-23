using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

using Kentico.Web.Mvc;

namespace MVC
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            // Enables and configures the selected Kentico ASP.NET MVC integration features
            ApplicationConfig.RegisterFeatures(ApplicationBuilder.Current);

            //Register Bundles
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //Configure DateTime
            CultureInfo newCulture = new CultureInfo(CultureInfo.CurrentCulture.Name, true);
            newCulture.DateTimeFormat.DateSeparator = ".";
            newCulture.DateTimeFormat.AMDesignator = "a.m.";
            newCulture.DateTimeFormat.PMDesignator = "p.m.";
            newCulture.DateTimeFormat.AbbreviatedMonthNames = 
                new string[] { "Jan.", "Feb.", "Mar.", "Apr.", "May", "Jun.", "Jul.",
                               "Aug.", "Sep.", "Oct.", "Nov.", "Dec.", "" };
            CultureInfo.DefaultThreadCurrentCulture = newCulture;
            CultureInfo.CurrentCulture = newCulture;
        }
    }
}
