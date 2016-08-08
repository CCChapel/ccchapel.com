using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using CCC.Helpers;
using CCC.Models.App;
using CCC.Models.App.Handlers;
using CCC.Models.App.Items;
using CCC.Models.App.Objects;

using CMS.DocumentEngine;
using CMS.DocumentEngine.Types;

namespace MVC.Controllers.WebAPI.App
{
    public class EventsController : ApiController
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

            //Create Banner Items
            List<Item> bannerItems = new List<Item>();
            bannerItems.Add(new Item()
            {
                Images = images,
                Title = "Banner"
            });

            //CookieHelpers.SetGlobalCookie(1);

            //Get Items
            List<CCC.Models.App.Actions.ListAction> actions = new List<CCC.Models.App.Actions.ListAction>();
            actions.Add(new CCC.Models.App.Actions.ListAction()
            {
                Handler = CCC.Models.App.Actions.Action.Handlers.List,
                Style = CCC.Models.App.Actions.ListAction.Styles.Plain,
                Type = CCC.Models.App.Actions.Action.ActionTypes.Navigation,
                Url = "http://beta.ccchapel.com/feeds/app/events/list"
            });
            List<ListItem> items = new List<ListItem>();
            items.Add(new ListItem()
            {
                Title = CookieHelpers.DefaultCampusID.ToString(), //"Carousel Test",
                Subtitle = CookieHelpers.LastVisit.ToShortDateString(), //"Testing",
                Actions = actions
            });

            //Create Handler
            ListHandler handler = new ListHandler()
            {
                Header = new Header()
                {
                    Title = "Events",
                    Style = Header.HeaderStyles.Banner,
                    Items = bannerItems
                },
                Items = items,
                Title = "Events",
                TrackViewedItems = false,
                GridLayout = ListHandler.GridLayouts.Square,
                GridTitlePosition = ListHandler.GridTitlePositions.Below
            };

            return handler;
        }

        [AcceptVerbs("GET")]
        public ListHandler List()
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

            //Create Banner Items
            List<Item> bannerItems = new List<Item>();
            bannerItems.Add(new Item()
            {
                Images = images,
                Title = "Banner"
            });

            //Get Items
            List<ListItem> items = new List<ListItem>();
            var events = CCB.PublicCalendar.GetEvents();

            foreach (var ev in events.Items)
            {
                items.Add(ev.ListItem());
            }

            //Create Handler
            ListHandler handler = new ListHandler()
            {
                Header = new Header()
                {
                    Title = "Events",
                    Style = Header.HeaderStyles.Banner,
                    Items = bannerItems
                },
                Items = items,
                Title = "Events",
                TrackViewedItems = false,
            };

            return handler;
        }

        [AcceptVerbs("GET")]
        public Content.CarouselContent Carousel()
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

            //Create items array
            List<CCC.Models.App.Actions.Action> actions = new List<CCC.Models.App.Actions.Action>();
            actions.Add(new CCC.Models.App.Actions.BrowserAction()
            {
                Handler = CCC.Models.App.Actions.BrowserAction.Handlers.Browser,
                Style = CCC.Models.App.Actions.BrowserAction.Styles.Internal,
                ContentUrl = "http://google.com"
                
            });
            List<Item> items = new List<Item>();
            items.Add(new Item()
            {
                Title = "Item",
                Actions = actions
            });

            var carousel = new Content.CarouselContent()
            {
                Title = "Title",
                Body = "Body",
                Images = images,
                Items = items
            };

            return carousel;
        }
    }
}
