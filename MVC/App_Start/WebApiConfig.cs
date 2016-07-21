using System;
using System.Collections.Generic;
using System.Linq;
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

            //Return JSON, not XML
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));

            //var xml = GlobalConfiguration.Configuration.Formatters.XmlFormatter;
            //xml.UseXmlSerializer = true;

            // WebAPI when dealing with JSON & JavaScript!
            // Setup json serialization to serialize classes to camel (std. Json format)
            var formatter = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
            formatter.SerializerSettings.ContractResolver =
                new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver();

            ////XmlSerializer enabled 
            config.Formatters.XmlFormatter.UseXmlSerializer = true;
            var xmlFormatter = new CCC.Formatters.Xml.NamespacedXmlMediaTypeFormatter();
            GlobalConfiguration.Configuration.Formatters.Insert(0, xmlFormatter);
        }
    }
}