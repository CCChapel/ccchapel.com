using CCC.Formatters.JSON;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web.Http;

namespace API
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // TODO: Add any additional configuration code.

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DownloadVideoApi",
                routeTemplate: "download/video/{id}",
                defaults: new { controller = "DownloadVideo" },
                constraints: new { id = @"^[0-9]*$" }
            );

            config.Routes.MapHttpRoute(
                name: "AppApi",
                routeTemplate: "feeds/app/{controller}/{action}",
                defaults: new { controller = "Messages", action = "Index" }
            );

            config.Routes.MapHttpRoute(
                name: "PodcastApi",
                routeTemplate: "feeds/podcast/{media}",
                defaults: new { controller = "Podcast", media = "video" }
            );

            config.Routes.MapHttpRoute(
                name: "SearchApi",
                routeTemplate: "api/search/{query}",
                defaults: new { controller = "Search", query = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            //Set JSON Formatter
            var jsonFormatter = new JsonMediaTypeFormatter();

            //Set Camel Case Properties
            jsonFormatter.SerializerSettings.ContractResolver =
                new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver();

            //Replace Standard JSON Formatter with Custom for proper Content Type
            config.Services.Replace(typeof(IContentNegotiator), new JsonContentNegotiator(jsonFormatter));


            //XmlSerializer enabled 
            config.Formatters.XmlFormatter.UseXmlSerializer = true;
            var xmlFormatter = new CCC.Formatters.Xml.NamespacedXmlMediaTypeFormatter();
            GlobalConfiguration.Configuration.Formatters.Insert(0, xmlFormatter);
        }
    }
}