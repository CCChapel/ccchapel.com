using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using CMS.CustomTables;
using CMS.CustomTables.Types;

using CCC.Helpers;

using MVC;

namespace CMS.DocumentEngine.Types
{
    /// <summary>
    /// Represents a content item of type Sermon.
    /// </summary>
    public partial class Sermon : TreeNode
    {
        #region Public Properties
        /// <summary>
        /// The Speaker of the Sermon
        /// </summary>
        public Person MessageSpeaker
        {
            get
            {
                return PersonProvider.GetPerson(MessageSpeakerNodeID, SiteHelpers.SiteCulture, SiteHelpers.SiteName);
            }
        }

        /// <summary>
        /// The Series the Sermon belongs to
        /// </summary>
        public Series MessageSeries
        {
            get
            {
                Guid parent = Parent.NodeGUID;
                return SeriesProvider.GetSeries(parent, SiteHelpers.SiteCulture, SiteHelpers.SiteName);
            }
        }

        /// <summary>
        /// The coalesced date of the Message from all campuses
        /// </summary>
        public DateTime MessageDate
        {
            get
            {
                return MessageHudsonDate.Coalesce(MessageAuroraDate, MessageStowDate, MessageHighlandSquareDate);
            }
        }

        /// <summary>
        /// The latest Message for all campuses
        /// </summary>
        public static Sermon Latest
        {
            get
            {
                //Setup default
                string campusCodeName = "Hudson";

                try
                {
                    if (!string.IsNullOrWhiteSpace(MiscellaneousHelpers.CurrentCampusCodeName))
                    {
                        campusCodeName = MiscellaneousHelpers.CurrentCampus.CampusCodeName;
                    }
                }
                catch { }

                return LatestForCampus(campusCodeName);
            }
        }

        /// <summary>
        /// The latest Message for the specified campus
        /// </summary>
        /// <param name="campusName">Campus to look for Message in</param>
        /// <returns>Latest Message for specified campus</returns>
        public static Sermon LatestForCampus(string campusCodeName = "Hudson")
        {
            //Get Campus
            CampusesItem campus = CampusesItem.GetFromCodeName(campusCodeName);

            string where = string.Format("Message{0}Date IS NOT NULL", campus.CampusSQLCodeName);
            string orderby = string.Format("Message{0}Date", campus.CampusSQLCodeName);

            var sermon = SermonProvider.GetSermons().Path(Series.SERMONS_PATH, PathTypeEnum.Children).Published()
                            .Where(where)
                            .OrderByDescending(orderby)
                            .FirstOrDefault();

            return sermon;
        }


        /// <summary>
        /// Returns a string representation to be used in the data-campus attribute for filtering
        /// </summary>
        public string CampusDataAttribute
        {
            get
            {
                string data = "";

                if (!MessageHudsonDate.IsNull())
                {
                    data += "hudson ";
                }

                if (!MessageAuroraDate.IsNull())
                {
                    data += "aurora ";
                }

                if (!MessageStowDate.IsNull())
                {
                    data += "stow ";
                }

                if (!MessageHighlandSquareDate.IsNull())
                {
                    data += "highland-square";
                }

                return data.Trim();
            }
        }

        /// <summary>
        /// Route Values for the Sermon
        /// </summary>
        public object RouteValues
        {
            get
            {
                return new
                {
                    controller = "MediaIndividual",
                    action = "Index",
                    seriesTitle = MessageSeries.NodeAlias,
                    year = MessageDate.Year.ToString("00"),
                    month = MessageDate.Month.ToString("00"),
                    day = MessageDate.Day.ToString("00"),
                    sermonTitle = NodeAlias
                };
            }
        }
        
        /// <summary>
        /// Gets the Sermon Series Background
        /// </summary>
        public Background Background
        {
            get
            {
                //Get Background Graphic
                Background background = new Background();

                try
                {
                    return BackgroundProvider.GetBackground(
                        MessageSeries.Fields.BackgroundGraphic.FirstOrDefault().NodeGUID,
                        SiteHelpers.SiteCulture,
                        SiteHelpers.SiteName
                    );
                }
                catch
                {
                    return new Background();
                }
            }
        }

        /// <summary>
        /// Gets the Vimeo ID of the attached media. If no Vimeo video is available, -1 is returned.
        /// </summary>
        public long VimeoID
        {
            get
            {
                //Check for Video Attachments
                var media = Fields.MessageMedia.Where(v => v.ClassName == VimeoVideo.CLASS_NAME);

                if (media.Any())
                {
                    var videos = VimeoVideoProvider.GetVimeoVideo(
                        media.First().NodeGUID,
                        SiteHelpers.SiteCulture,
                        SiteHelpers.SiteName);

                    if (videos.Any())
                    {
                        var video = videos.First();

                        return video.VimeoID;
                    }
                }

                return -1;
            }
        }

        /// <summary>
        /// Gets the Vimeo Video Related to this Sermon or returns and empty object if no video was found
        /// </summary>
        public Vimeo.Types.Video Video
        {
            get
            {
                try
                {
                    if (VimeoID > 0)
                    {

                        //Check Cache for Video
                        string cacheID = CachingHelpers.CachingID("VimeoVideo", VimeoID);

                        if (CachingHelpers.Cache.Contains(cacheID))
                        {
                            //Get Data From Cache
                            Vimeo.Types.Video vimeo = (Vimeo.Types.Video)CachingHelpers.Cache.Get(cacheID);

                            return vimeo;
                        }
                        else
                        {
                            //Get Data From Vimeo & Add to Cache
                            Vimeo.Types.Video vimeo = VimeoHelpers.Api.GetVideo(VimeoID);

                            CachingHelpers.Cache.Add(cacheID, vimeo, CachingHelpers.Policy);

                            return vimeo;
                        }
                    }

                    return new Vimeo.Types.Video();
                }
                catch
                {
                    return new Vimeo.Types.Video();
                }
            }
        }

        public bool HasVideo
        {
            get
            {
                try
                {
                    var video = VimeoVideoProvider.GetVimeoVideo(
                                    Fields.MessageMedia.Where(v => v.ClassName == VimeoVideo.CLASS_NAME).FirstOrDefault().NodeGUID,
                                    SiteHelpers.SiteCulture,
                                    SiteHelpers.SiteName);

                    if (video.Any())
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// The Kentico Document representing an MP3
        /// </summary>
        public AmazonMp3 MP3
        {
            get
            {
                return AmazonMp3Provider.GetAmazonMp3(
                    Fields.MessageMedia.Where(a => a.ClassName == AmazonMp3.CLASS_NAME).FirstOrDefault().NodeGUID,
                    SiteHelpers.SiteCulture,
                    SiteHelpers.SiteName);
            }
        }

        /// <summary>
        /// Returns the URL needed to download the video and null if there is no video to download
        /// </summary>
        public string DownloadUrlVideo
        {
            get
            {
                //Start by looking for Vimeo
                try
                {
                    if (VimeoID > 0)
                    {
                        //download/video/164987098

                        object apiRoute = new {
                                controller = "DownloadVideo",
                                id = VimeoID
                            };

                        return UrlHelpers.UrlHelper.HttpRouteUrl("DownloadVideoApi", apiRoute);
                    }
                }
                catch
                { }

                return null;
            }
        }

        /// <summary>
        /// Returns the URL needed to download the MP3 and null if there is no video to download
        /// </summary>
        public string DownloadUrlAudio
        {
            get
            {
                try
                {
                    if (!string.IsNullOrWhiteSpace(MP3.Fields.Title))
                    {
                        return MP3.Url;
                    }
                }
                catch { }

                return null;
            }
        }

        /// <summary>
        /// Returns the HTML necessary to embed a player and play the video or audio file
        /// </summary>
        public string EmbedHtml
        {
            get
            {
                try
                {
                    if (!string.IsNullOrWhiteSpace(Video.name))
                    {
                        return VimeoHelpers.CustomEmbedHtml(Video.embed.html);
                    }
                }
                catch (Exception ex)
                { }

                return null;
            }
        }
        
        public string RouteUrl
        {
            get
            {
                return UrlHelpers.UrlHelper.RouteUrl(RouteValues);
            }
        }
        
        //public CCC.Models.Podcast.FeedItem ToVideoFeedItem()
        //{
        //    return new CCC.Models.Podcast.FeedItem(MessageTitle, RouteUrl)
        //    {
        //        Description = MessageDescription,
        //        PublishDate = MessageDate,
        //        Author = new CCC.Models.Podcast.FeedEmailAddress(
        //            //MessageSpeaker.Email ?? "info@ccchapel.com",
        //            "info@ccchapel.com",
        //            MessageSpeaker.FullName),
        //        Enclosure = new CCC.Models.Podcast.FeedItemEnclosure(
        //            DownloadUrlVideo,
        //            "video/mp4",
        //            Video.duration)
        //    };
        //}
        #endregion
    }
}