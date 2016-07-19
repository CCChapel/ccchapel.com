using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Web.Http;

using CMS.DocumentEngine.Types;

using CCC.Models.Podcast;
using CCC.Helpers;

namespace MVC.Controllers.WebAPI
{
    public class PodcastController : ApiController
    {
        // GET api/<controller>
        public HttpResponseMessage Get(string media = "video")
        {
            //Create Feed
            rss rss = new rss();
            
            //Create Channel
            rssChannel channel = new rssChannel()
            {
                title = string.Format("Christ Community Chapel ({0})", media.ToLower()),
                subtitle = "",
                description = "There’s more to God than a religious experience. It’s about connecting with him, not just going through the motions. Check out our most recent weekly messages here. If you like what you hear and want more, visit our website. In addition to more messages, there’s a bunch of info about who we are, visiting our church, getting plugged in.",
                summary = "There’s more to God than a religious experience. It’s about connecting with him, not just going through the motions. Check out our most recent weekly messages here. If you like what you hear and want more, visit our website. In addition to more messages, there’s a bunch of info about who we are, visiting our church, getting plugged in.",
                author = "Christ Community Chapel",
                language = "en-us",
                copyright = string.Format("&#xA9; {0} Christ Community Chapel", DateTime.Now.Year),
                link = "http://ccchapel.com",
                owner = new owner()
                {
                    name = "Christ Community Chapel",
                    email = "info@ccchapel.com"
                }
            };

            categoryCategory catCat = new categoryCategory() { text = "Christianity" };
            category cat = new category() { category1 = catCat, text = "Religion & Spirituality" };
            channel.category = new category[] { cat };

            //Create Items
            foreach (var sermon in SermonProvider.GetSermons().Published())
            {
                rssChannelItem item = new rssChannelItem()
                {
                    author = sermon.MessageSpeaker.FullName,
                    duration = new TimeSpan(0, 0, sermon.Video.duration).ToString(@"mm\:ss"),
                    guid = sermon.NodeGUID.ToString(),
                    image = new image()
                    {
                        href = "http://ccchapel.com/cmspages/getfile.aspx?guid=2fc8dca4-eff9-4bf4-a393-1403ac477ce6&chset=e0931fcd-7817-4369-acf0-ff06344a20e4"
                    },
                    pubDate = sermon.MessageDate.AddHours(5).ToString("R"),         //Add hours for GMT offset
                    subtitle = sermon.MessageSeries.SeriesTitle,
                    summary = sermon.MessageDescription.RemoveHtml(),
                    title = string.Format("{0} | {1}", sermon.MessageSeries.SeriesTitle, sermon.MessageTitle)
                };

                if (media.ToLower() == "video" && !string.IsNullOrWhiteSpace(sermon.DownloadUrlVideo))
                {
                    item.enclosure = new rssChannelItemEnclosure()
                    {
                        url = string.Format("{0}{1}", SiteHelpers.DomainName, sermon.DownloadUrlVideo),
                        type = "video/mp4",
                        length = uint.Parse(sermon.Video.duration.ToString())
                    };

                    channel.item.Add(item);
                }
                else if (media.ToLower() == "audio" && !string.IsNullOrWhiteSpace(sermon.DownloadUrlAudio))
                {
                    item.enclosure = new rssChannelItemEnclosure()
                    {
                        url = string.Format("{1}", SiteHelpers.DomainName, sermon.DownloadUrlAudio),
                        type = "audio/mpeg"
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