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
        public SpeakersItem MessageSpeaker
        {
            get
            {
                return CustomTableItemProvider.GetItem<SpeakersItem>(this.MessageSpeakerID);
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
                string campusName = "Hudson";

                if (!string.IsNullOrWhiteSpace(MiscellaneousHelpers.CurrentCampusName))
                {
                    campusName = MiscellaneousHelpers.CurrentCampus.CampusSQLCodeName;
                }

                return LatestForCampus(campusName);
            }
        }

        /// <summary>
        /// The latest Message for the specified campus
        /// </summary>
        /// <param name="campusName">Campus to look for Message in</param>
        /// <returns>Latest Message for specified campus</returns>
        public static Sermon LatestForCampus(string campusName = "Hudson")
        {
            //Get Campus
            CampusesItem campus = CampusesItem.GetFromName(campusName);

            string where = string.Format("Message{0}Date IS NOT NULL", campus.CampusSQLCodeName);
            string orderby = string.Format("Message{0}Date", campus.CampusSQLCodeName);

            var sermon = SermonProvider.GetSermons().Published()
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
                    controller = "Message",
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
        /// Gets the Vimeo Video Related to this Sermon or returns and empty object if no video was found
        /// </summary>
        public Vimeo.Types.Video Video
        {
            get
            {
                try
                {
                    var video = VimeoVideoProvider.GetVimeoVideo(
                        Fields.MessageMedia.Where(v => v.ClassName == VimeoVideo.CLASS_NAME).FirstOrDefault().NodeGUID,
                        SiteHelpers.SiteCulture,
                        SiteHelpers.SiteName).First();

                    //Check Cache for Video
                    string cacheID = CachingHelpers.CachingID("VimeoVideo", video.VimeoID);

                    if (CachingHelpers.Cache.Contains(cacheID))
                    {
                        //Get Data From Cache
                        Vimeo.Types.Video vimeo = (Vimeo.Types.Video)CachingHelpers.Cache.Get(cacheID);

                        return vimeo;
                    }
                    else
                    {
                        //Get Data From CCB & Add to Cache
                        Vimeo.Types.Video vimeo = VimeoHelpers.Api.GetVideo(video.VimeoID);

                        CachingHelpers.Cache.Add(cacheID, vimeo, CachingHelpers.Policy);

                        return vimeo;
                    }
                }
                catch
                {
                    return new Vimeo.Types.Video();
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
                    if (!string.IsNullOrWhiteSpace(Video.name))
                    {
                        return Video.download
                            .Where(d => d.quality == "hd")
                            .FirstOrDefault()
                            .link;
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
                catch
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
        #endregion
    }
}