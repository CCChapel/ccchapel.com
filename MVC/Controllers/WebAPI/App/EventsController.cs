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

            //Get Items
            

            //Create Handler
            ListHandler handler = new ListHandler()
            {
                Header = new Header()
                {
                    Title = "Events",
                    Style = Header.HeaderStyles.Banner,
                    Items = bannerItems
                },
                //Items = items,
                Title = "Events",
                TrackViewedItems = false,
                GridLayout = ListHandler.GridLayouts.Square,
                GridTitlePosition = ListHandler.GridTitlePositions.Below
            };

            return handler;
        }
    }
}
