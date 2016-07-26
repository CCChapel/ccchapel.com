using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
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
            var serieses = SeriesProvider.GetSeries().Path(CMS.DocumentEngine.Types.Series.SERMONS_PATH, PathTypeEnum.Children).Published().OrderByDescending(DatabaseHelpers.OrderByCmsTree);
            foreach (var s in serieses)
            {
                items.Add(s.ListItem());
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
        public ListHandler Series(string seriesAlias)
        {
            //Get Series
            var serieses = (from s in SeriesProvider.GetSeries().Published()
                            where s.NodeAlias.ToLower() == seriesAlias.ToLower()
                            select s);

            //Check that we have something
            if (serieses.Any())
            {
                var series = serieses.FirstOrDefault();

                //Set Header Items
                var headerItems = new List<Item>();
                headerItems.Add(new Item()
                {
                    Title = series.Fields.Title,
                    Images = series.ImageSet()
                });

                //Get Items
                var items = new List<Item>();
                int i = 1;
                foreach (var s in series.GetSermons())
                {
                    items.Add(s.ListItem());

                    i++;
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

        [AcceptVerbs("GET")]
        public MediaDetailHandler Message(string messageAlias)
        {
            //Get Message
            var messages = (from s in SermonProvider.GetSermons().Published()
                            where s.NodeAlias == messageAlias
                            select s);

            if (messages.Any())
            {
                var message = messages.First();

                //Setup Media
                List<Media> media = new List<Media>();

                if (message.HasVideo)
                {
                    media.Add(new Media()
                    {
                        //Video
                        SapID = message.DocumentGUID,
                        Url = string.Format("{0}{1}",
                                UrlHelpers.CurrentDomainName,
                                message.DownloadUrlVideo),
                        Format = Media.Formats.MP4,
                        Downloadable = true,
                        Duration = message.Video.duration,
                        Width = message.Video.width,
                        Images = message.MessageSeries.ImageSet()
                    });
                }

                if (!string.IsNullOrWhiteSpace(message.DownloadUrlAudio))
                {
                    media.Add(new Media()
                    {
                        SapID = message.DocumentGUID,
                        Url = message.DownloadUrlAudio,
                        Format = Media.Formats.MP3,
                        Downloadable = true
                    });
                }

                //Create Handler
                MediaDetailHandler handler = new MediaDetailHandler()
                {
                    Header = new Header()
                    {
                        Title = message.Fields.MessageTitle
                    },
                    Title = message.MessageTitle,
                    Subtitle = message.MessageDate.ToShortDateString(),
                    Body = message.MessageDescription.RemoveHtml(),
                    Date = message.MessageDate,
                    Media = media,
                    Images = message.MessageSeries.ImageSet(),
                    //ActionSheet
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
