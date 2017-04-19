using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

using Kentico.Web.Mvc;

using CMS.DocumentEngine.Types;

namespace MVC
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.LowercaseUrls = true;
            routes.AppendTrailingSlash = false;

            // Maps routes to Kentico HTTP handlers first as some Kentico URLs be matched by the default ASP.NET MVC route, resulting in displaying pages without images.
            routes.Kentico().MapRoutes();

            //Robots.txt
            routes.MapRoute(
                "Robots.txt",
                "robots.txt",
                new { controller = "Robots", action = "Index" }
            );

            //External Links
            routes.MapRoute(
                name: "ExternalLinks",
                url: "link",
                defaults: new { controller = "ExternalLink", action = "Index" }
            );

            //Group Finder
            routes.MapRoute(
                name: "GroupFinder",
                url: "groups",
                defaults: new { controller = "GroupFinder", action = "Index" }
            );

            //Staff
            routes.MapRoute(
                name: "StaffTeam",
                url: "staff/{action}/{name}",
                defaults: new { controller = "StaffTeam", action = "Teams", name = UrlParameter.Optional }
            );

            //Jobs
            routes.MapRoute(
                name: "JobApplication",
                url: "employment/application",
                defaults: new { controller = "Page", action = "Index", path = "employment/application" }
            );
            routes.MapRoute(
                name: "InternApplication",
                url: "employment/summer-intern-application",
                defaults: new { controller = "Page", action = "Index", path = "employment/summer-intern-application" }
            );
            routes.MapRoute(
                name: "TwoYearInternApplication",
                url: "employment/two-year-intern-application",
                defaults: new { controller = "Page", action = "Index", path = "employment/two-year-intern-application" }
            );
            routes.MapRoute(
                name: "Jobs",
                url: "employment/{jobTitle}",
                defaults: new { controller = "Jobs", action = "Index", jobTitle = UrlParameter.Optional }
            );

            //Search
            routes.MapRoute(
                name: "SearchResults",
                url: "search",
                defaults: new { controller = "Search", action = "Index" }
            );

            //Media
            routes.MapRoute(
                name: "Media",
                url: "media",
                defaults: new { controller = "Media", action = "Index" }
            );

            //Media Series
            routes.MapRoute(
                name: "MediaSeries",
                url: "media/{seriesTitle}",
                defaults: new { controller = "MediaSeries", action = "Index" }
            );

            //Media
            routes.MapRoute(
                name: "MediaIndividual",
                url: "media/{seriesTitle}/{year}/{month}/{day}/{sermonTitle}",
                defaults: new { controller = "MediaIndividual", action = "Index" },
                constraints: new { year = @"^(\d{4})$", month = @"^(\d{2})$", day = @"^(\d{2})$" }
            );

            #region Old Messages
            /******************************************************
             * Eventually remove these. Using now for redirects.
             ******************************************************/
            //Messages
            routes.MapRoute(
                name: "Messages",
                url: "messages",
                defaults: new { controller = "Messages", action = "Index" }
            );

            //Message Series
            routes.MapRoute(
                name: "MessageSeries",
                url: "messages/{seriesTitle}",
                defaults: new { controller = "MessageSeries", action = "Index" } 
            );

            //Message
            routes.MapRoute(
                name: "Message",
                url: "messages/{seriesTitle}/{year}/{month}/{day}/{sermonTitle}",
                defaults: new { controller = "Message", action = "Index" }, 
                constraints: new { year = @"^(\d{4})$", month = @"^(\d{2})$", day = @"^(\d{2})$" }
            );
            #endregion

            //Event
            routes.MapRoute(
                name: "Events",
                url: "events/{year}/{month}/{day}/{eventName}",
                defaults: new { controller = "Event", action = "Index" },
                constraints: new { year = @"^(\d{4})$", month = @"^(\d{2})$", day = @"^(\d{2})$" }
            );

            //Events
            routes.MapRoute(
                name: "UpcomingEvents",
                url: "events/{startDate}/{endDate}",
                defaults: new { controller = "Events", action = "Index", startDate = UrlParameter.Optional, endDate = UrlParameter.Optional }
            );

            //Me
            routes.MapRoute(
                name: "Me",
                url: "me",
                defaults: new { controller = "Page", action = "Index", path = "Special-Pages/Welcome" }
            );

            //Pages
            routes.MapRoute(
                name: "Page",
                url: "{*path}",
                defaults: new { controller = "Page", action = "Index", path = "Home"}
            );

            ////Generic Error
            //routes.MapRoute(
            //    name: "GenericError",
            //    url: "oops",
            //    defaults: new { controller = "HttpErrors", action = "GenericError" }
            //);

            //Leave for Sections ?? Refactor Later
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Page", action = "Index", id = UrlParameter.Optional }
            );

            //Not Found
            routes.MapRoute(
                name: "NotFound",
                url: "{*url}",
                defaults: new { controller = "HttpErrors", action = "NotFound" }
            );
        }
    }
}
