using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

using CCC.Models.App.Handlers;
using CCC.Models.App.Items;
using CCC.Models.App.Objects;
using CCC.Models.App.Actions;

using CMS.DocumentEngine.Types;
using Kentico.Web.Mvc;

namespace MVC.Controllers.WebAPI.App
{
    public class MessagesController : ApiController
    {
        [AcceptVerbs("GET")]
        public ListHandler Index()
        {
            //Create imag array
            List<Image> images = new List<Image>();
            images.Add(new Image()
            {
                Url = "http://beta.ccchapel.com/Content/2016-Q1/images/app/test/1024x1024.jpg",
                Width = 1024
            });

            images.Add(new Image()
            {
                Url = "http://beta.ccchapel.com/Content/2016-Q1/images/app/test/1536x560.jpg",
                Width = 1536
            });

            images.Add(new Image()
            {
                Url = "http://beta.ccchapel.com/Content/2016-Q1/images/app/test/1920x692.jpg",
                Width = 1920
            });

            images.Add(new Image()
            {
                Url = "http://beta.ccchapel.com/Content/2016-Q1/images/app/test/1920x1080.jpg",
                Width = 1920
            });

            //Get Items
            List<ListItem> items = new List<ListItem>();

            foreach (var s in SeriesProvider.GetSeries().Published())
            {
                //Create Action
                ListAction action = new ListAction()
                {
                    Type = CCC.Models.App.Actions.Action.ActionTypes.Navigation,
                    Handler = "list",
                    Style = ListAction.Styles.Grid,
                    Url = string.Format("{0}{1}/series?seriesTitle={2}", 
                        CCC.Helpers.UrlHelpers.CurrentDomainName, 
                        CCC.Helpers.UrlHelpers.CurrentRoute, 
                        s.NodeAlias)
                };
                var actions = new List<ListAction>();
                actions.Add(action);

                //Create List Item
                ListItem li = new ListItem()
                {
                    Title = s.Fields.Title,
                    Images = images,
                    Actions = actions
                };

                items.Add(li);
            }

            //Create Banner Items
            List<Item> bannerItems = new List<Item>();
            bannerItems.Add(new Item()
            {
                Images = images,
                Title = "Banner"
            });

            //Create Handler
            ListHandler handler = new ListHandler()
            {
                Header = new Header()
                {
                    Title = "Messages",
                    Style = Header.HeaderStyles.Banner,
                    Items = bannerItems
                },
                Items = items,
                Title = "Messages",
                TrackViewedItems = false,
                GridLayout = ListHandler.GridLayouts.Square,
                GridTitlePosition = ListHandler.GridTitlePositions.Below
            };

            return handler;
        }

        [AcceptVerbs("GET")]
        public ListHandler Series(string seriesTitle)
        {
            //Get Series
            var serieses = (from s in SeriesProvider.GetSeries().Published()
                            where s.NodeAlias.ToLower() == seriesTitle.ToLower()
                            select s);

            //Check that we have something
            if (serieses.Any())
            {
                var series = serieses.FirstOrDefault();

                //System.Web.Mvc.UrlHelper urlHelper = new System.Web.Mvc.UrlHelper(HttpContext.Current.Request.RequestContext);
                var imageUrl = string.Format("{0}{1}",
                    CCC.Helpers.UrlHelpers.CurrentDomainName,
                    series.Background.Fields.ImageFile.GetRoute());

                //Setup Images
                var image = new Image()
                {
                    Url = imageUrl,
                    Width = series.Background.Fields.ImageFile.ImageWidth
                };
                var images = new List<Image>();
                images.Add(image);

                //Set Header Items
                var headerItems = new List<Item>();
                headerItems.Add(new Item()
                {
                    Title = series.Fields.Title,
                    Images = images
                });

                //Get Items
                var items = new List<Item>();
                foreach (var s in series.GetSermons())
                {
                    items.Add(new Item()
                    {
                        Title = s.Fields.MessageTitle,
                        Subtitle = s.MessageDate.ToShortDateString(),
                        Images = images
                    });
                }

                //Create Handler
                ListHandler handler = new ListHandler()
                {
                    Header = new Header()
                    {
                        Title = series.Fields.Title,
                        Style = Header.HeaderStyles.Banner,
                        Items = headerItems
                    },
                    Items = items,
                    Title = series.Fields.Title,
                    TrackViewedItems = false,
                    GridLayout = ListHandler.GridLayouts.Square,
                    GridTitlePosition = ListHandler.GridTitlePositions.Below
                };

                return handler;
            }
            else
            {
                throw new HttpException(404, "Page Not Found");
            }
        }
    }
}
