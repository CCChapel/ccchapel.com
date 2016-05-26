using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using CCC.Models.Podcast;
using CMS.DocumentEngine.Types;
using System.Net.Http.Headers;

namespace MVC.Controllers.WebAPI
{
    public class PodcastController : ApiController
    {
        // GET api/<controller>
        //public IEnumerable<string> Get(string media = "video")
        public HttpResponseMessage Get(string media = "video")
        {
            //Feed feed = new Feed(
            //    "Christ Community Chapel (Video)",
            //    "Welcome to the weekly podcast of Christ Community Chapel. Here you will find our Sunday morning sermons for the past ten weeks. For more sermons, please visit our website.",
            //    "Christ Community Chapel");

            //feed.AlternateLink = "http://ccchapel.com";
            //feed.Language = "en-us";
            //feed.Copyright = string.Format("&#xA9; {0} Christ Community Chapel", DateTime.Now.Year);
            //feed.Categories = new string[] { "Religion &amp; Spirituality" };

            //foreach (var sermon in SermonProvider.GetSermons().Published())
            //{
            //    if (sermon.HasVideo)
            //    {
            //        feed.Items.Add(sermon.ToVideoFeedItem());
            //    }
            //}

            //HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, feed, new MediaTypeHeaderValue("application/xml"));

            //return response;

            //Create Feed
            rss rss = new rss();
            
            //Create Channel
            rssChannel channel = new rssChannel();
            channel.title = "Christ Community Chapel (Video)";
            channel.description = "Welcome to the weekly podcast of Christ Community Chapel. Here you will find our Sunday morning sermons for the past ten weeks. For more sermons, please visit our website.";
            channel.author = "Christ Community Chapel";
            channel.language = "en-us";
            channel.copyright = string.Format("&#xA9; {0} Christ Community Chapel", DateTime.Now.Year);

            category cat = new category();
            cat.text = "Religion &amp; Spirituality";
            channel.category = new category[] { cat };

            channel.subtitle = "";
            channel.link = "http://ccchapel.com";
            channel.owner = new owner()
            {
                name = "Christ Community Chapel",
                email = "info@ccchapel.com"
            };
            channel.summary = channel.description;

            //Create Items
            foreach (var sermon in SermonProvider.GetSermons().Published())
            {
                if (sermon.HasVideo)
                {
                    rssChannelItem item = new rssChannelItem()
                    {
                        author = sermon.MessageSpeaker.FullName,
                        duration = sermon.Video.duration.ToString(),
                        enclosure = new rssChannelItemEnclosure()
                        {
                            url = sermon.DownloadUrlVideo,
                            type = "video/mp4",
                            length = uint.Parse(sermon.Video.duration.ToString())
                        },
                        guid = sermon.NodeGUID.ToString(),
                        //image = "",
                        pubDate = sermon.MessageDate.ToShortDateString(),
                        subtitle = sermon.MessageSeries.SeriesTitle,
                        summary = sermon.MessageDescription,
                        title = sermon.MessageTitle
                    };

                    channel.item.Add(item);
                }
            }

            rss.channel = channel;
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, rss, new MediaTypeHeaderValue("application/xml"));

            return response;
        }
    }
}