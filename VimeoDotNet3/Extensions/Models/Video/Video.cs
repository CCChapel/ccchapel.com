using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace Vimeo.Types
{
    //public class Size2
    //{
    //    public int width { get; set; }
    //    public int height { get; set; }
    //    public string link { get; set; }
    //}

    //public class Pictures3
    //{
    //    public string uri { get; set; }
    //    public bool active { get; set; }
    //    public string type { get; set; }
    //    public List<Size2> sizes { get; set; }
    //}

    //public class Size3
    //{
    //    public int width { get; set; }
    //    public int height { get; set; }
    //    public string link { get; set; }
    //}

    //public class Pictures5
    //{
    //    public string uri { get; set; }
    //    public bool active { get; set; }
    //    public string type { get; set; }
    //    public List<Size3> sizes { get; set; }
    //}

    //public class Website2
    //{
    //    public object name { get; set; }
    //    public string link { get; set; }
    //    public object description { get; set; }
    //}

    //public class Activities2
    //{
    //    public string uri { get; set; }
    //    public List<string> options { get; set; }
    //}

    //public class Albums2
    //{
    //    public string uri { get; set; }
    //    public List<string> options { get; set; }
    //    public int total { get; set; }
    //}

    //public class Channels2
    //{
    //    public string uri { get; set; }
    //    public List<string> options { get; set; }
    //    public int total { get; set; }
    //}

    //public class Feed2
    //{
    //    public string uri { get; set; }
    //    public List<string> options { get; set; }
    //}

    //public class Followers2
    //{
    //    public string uri { get; set; }
    //    public List<string> options { get; set; }
    //    public int total { get; set; }
    //}

    //public class Following2
    //{
    //    public string uri { get; set; }
    //    public List<string> options { get; set; }
    //    public int total { get; set; }
    //}

    //public class Groups2
    //{
    //    public string uri { get; set; }
    //    public List<string> options { get; set; }
    //    public int total { get; set; }
    //}

    //public class Likes3
    //{
    //    public string uri { get; set; }
    //    public List<string> options { get; set; }
    //    public int total { get; set; }
    //}

    //public class Portfolios2
    //{
    //    public string uri { get; set; }
    //    public List<string> options { get; set; }
    //    public int total { get; set; }
    //}

    //public class Videos4
    //{
    //    public string uri { get; set; }
    //    public List<string> options { get; set; }
    //    public int total { get; set; }
    //}

    //public class Watchlater3
    //{
    //    public string uri { get; set; }
    //    public List<string> options { get; set; }
    //    public int total { get; set; }
    //}

    //public class Shared2
    //{
    //    public string uri { get; set; }
    //    public List<string> options { get; set; }
    //    public int total { get; set; }
    //}

    //public class Pictures6
    //{
    //    public string uri { get; set; }
    //    public List<string> options { get; set; }
    //    public int total { get; set; }
    //}

    //public class Connections4
    //{
    //    public Activities2 activities { get; set; }
    //    public Albums2 albums { get; set; }
    //    public Channels2 channels { get; set; }
    //    public Feed2 feed { get; set; }
    //    public Followers2 followers { get; set; }
    //    public Following2 following { get; set; }
    //    public Groups2 groups { get; set; }
    //    public Likes3 likes { get; set; }
    //    public Portfolios2 portfolios { get; set; }
    //    public Videos4 videos { get; set; }
    //    public Watchlater3 watchlater { get; set; }
    //    public Shared2 shared { get; set; }
    //    public Pictures6 pictures { get; set; }
    //}

    //public class Metadata4
    //{
    //    public Connections4 connections { get; set; }
    //}

    //public class Videos5
    //{
    //    public string privacy { get; set; }
    //}

    //public class Preferences2
    //{
    //    public Videos5 videos { get; set; }
    //}

    public partial class Video
    {
        public string uri { get; set; }
        public string name { get; set; }
        public object description { get; set; }
        public string link { get; set; }

        /// <summary>
        /// The videos duration in seconds
        /// </summary>
        public int duration { get; set; }
        public int width { get; set; }
        public object language { get; set; }
        public int height { get; set; }
        public Embed embed { get; set; }
        public string created_time { get; set; }
        public string modified_time { get; set; }
        public List<string> content_rating { get; set; }
        public string license { get; set; }
        public Privacy privacy { get; set; }
        public Pictures pictures { get; set; }
        public List<object> tags { get; set; }
        public Stats stats { get; set; }
        public Metadata metadata { get; set; }
        public User user { get; set; }
        public string review_link { get; set; }
        public List<Download> download { get; set; }
        public List<File> files { get; set; }
        public object app { get; set; }
        public string status { get; set; }
        public EmbedPresets embed_presets { get; set; }

        #region Sub-Classes
        public class Embed
        {
            public string uri { get; set; }
            public string html { get; set; }
            public Buttons buttons { get; set; }
            public Logos logos { get; set; }
            public Title title { get; set; }
            public bool playbar { get; set; }
            public bool volume { get; set; }
            public string color { get; set; }

            #region Sub-Classes
            public class Buttons
            {
                public bool like { get; set; }
                public bool watchlater { get; set; }
                public bool share { get; set; }
                public bool embed { get; set; }
                public bool hd { get; set; }
                public bool fullscreen { get; set; }
                public bool scaling { get; set; }
            }

            public class Logos
            {
                public bool vimeo { get; set; }
                public CustomLogo custom { get; set; }

                #region Sub-Classes
                public class CustomLogo
                {
                    public bool active { get; set; }
                    public string link { get; set; }
                    public bool sticky { get; set; }
                }
                #endregion
            }

            public class Title
            {
                public string name { get; set; }
                public string owner { get; set; }
                public string portrait { get; set; }
            }
            #endregion

        }

        public class Privacy
        {
            public string view { get; set; }
            public string embed { get; set; }
            public bool download { get; set; }
            public bool add { get; set; }
            public string comments { get; set; }
        }

        public class Stats
        {
            public int plays { get; set; }
        }

        public class Metadata
        {
            public Connections connections { get; set; }
            public Interactions interactions { get; set; }

            #region Sub-Classes
            public class Connections
            {
                public Comments comments { get; set; }
                public Credits credits { get; set; }
                public Types.Metadata.Connections.Likes likes { get; set; }
                public Types.Metadata.Connections.Pictures pictures { get; set; }
                public TextTracks texttracks { get; set; }
                public object related { get; set; }

                #region Sub-Classes
                public class Comments
                {
                    public string uri { get; set; }
                    public List<string> options { get; set; }
                    public int total { get; set; }
                }

                public class Credits
                {
                    public string uri { get; set; }
                    public List<string> options { get; set; }
                    public int total { get; set; }
                }

                public class TextTracks
                {
                    public string uri { get; set; }
                    public List<string> options { get; set; }
                    public int total { get; set; }
                }
                #endregion
            }

            public class Interactions
            {
                public WatchLater watchlater { get; set; }

                #region Sub-Classes
                public class WatchLater
                {
                    public bool added { get; set; }
                    public object added_time { get; set; }
                    public string uri { get; set; }
                }
                #endregion
            }
            #endregion
        }

        public class Download
        {
            public string quality { get; set; }
            public string type { get; set; }
            public int width { get; set; }
            public int height { get; set; }
            public string expires { get; set; }
            public string link { get; set; }
            public string created_time { get; set; }
            public int fps { get; set; }
            public int size { get; set; }
            public string md5 { get; set; }
        }

        public class File
        {
            public string quality { get; set; }
            public string type { get; set; }
            public int width { get; set; }
            public int height { get; set; }
            public string link { get; set; }
            public string created_time { get; set; }
            public int fps { get; set; }
            public int size { get; set; }
            public string md5 { get; set; }
            public string link_secure { get; set; }
        }

        public class EmbedPresets
        {
            public string uri { get; set; }
            public string name { get; set; }
            public Settings settings { get; set; }
            public Metadata metadata { get; set; }
            public User user { get; set; }

            #region Sub-Classes
            public class Settings
            {
                public Buttons buttons { get; set; }
                public Logos logos { get; set; }
                public string outro { get; set; }
                public string portrait { get; set; }
                public string title { get; set; }
                public string byline { get; set; }
                public bool badge { get; set; }
                public bool byline_badge { get; set; }
                public bool collections_button { get; set; }
                public bool playbar { get; set; }
                public bool volume { get; set; }
                public bool fullscreen_button { get; set; }
                public bool scaling_button { get; set; }
                public bool autoplay { get; set; }
                public bool autopause { get; set; }
                public bool loop { get; set; }
                public string color { get; set; }
                public bool link { get; set; }
                public string custom_logo_url { get; set; }
                public string custom_logo_link_url { get; set; }
                public bool custom_logo_use_link { get; set; }
                public int logo_width { get; set; }
                public int logo_height { get; set; }
                public StreamClips stream_clips { get; set; }

                #region Sub-Classes
                public class Buttons
                {
                    public bool like { get; set; }
                    public bool watchlater { get; set; }
                    public bool share { get; set; }
                    public bool embed { get; set; }
                    public bool vote { get; set; }
                    public bool hd { get; set; }
                }

                public class Logos
                {
                    public bool vimeo { get; set; }
                    public bool custom { get; set; }
                    public bool sticky_custom { get; set; }
                }

                public class StreamClips
                {
                    public List<object> video { get; set; }
                }
                #endregion
            }

            public class Metadata
            {
                public Connections connections { get; set; }

                #region Sub-Classes
                public class Connections
                {
                    public Types.Metadata.Connections.Video videos { get; set; }
                }
                #endregion
            }
            #endregion

        }
        #endregion
    }

    #region Requests
    public static partial class Requests
    {
        public static Video GetVideo(this VimeoClient client, long vimeoID)
        {
            string url = string.Format("/videos/{0}", vimeoID);

            return JsonConvert.DeserializeObject<Video>(client.RequestJSON(url, null, "GET"));
        }
    }
    #endregion
}