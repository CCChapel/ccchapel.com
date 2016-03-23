using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vimeo.Types
{
    public partial class User
    {
        public string uri { get; set; }
        public string name { get; set; }
        public string link { get; set; }
        public string location { get; set; }
        public string bio { get; set; }
        public string created_time { get; set; }
        public string account { get; set; }
        public Pictures pictures { get; set; }
        public List<Website> websites { get; set; }
        public Metadata metadata { get; set; }
        public Preferences preferences { get; set; }
        public List<string> content_filter { get; set; }

        #region Sub-Classes
        public class Website
        {
            public object name { get; set; }
            public string link { get; set; }
            public object description { get; set; }
        }

        public class Metadata
        {
            public Connections connections { get; set; }

            #region Sub-Classes
            public partial class Connections
            {
                public Activities activities { get; set; }
                public Albums albums { get; set; }
                public Channels channels { get; set; }
                public Feed feed { get; set; }
                public Followers followers { get; set; }
                public Following following { get; set; }
                public Groups groups { get; set; }
                public Types.Metadata.Connections.Likes likes { get; set; }
                public Portfolios portfolios { get; set; }
                public Videos videos { get; set; }
                public WatchLater watchlater { get; set; }
                public Shared shared { get; set; }
                public Types.Metadata.Connections.Pictures pictures { get; set; }

                #region Sub-Classes
                public class Activities
                {
                    public string uri { get; set; }
                    public List<string> options { get; set; }
                }

                public class Albums
                {
                    public string uri { get; set; }
                    public List<string> options { get; set; }
                    public int total { get; set; }
                }

                public class Channels
                {
                    public string uri { get; set; }
                    public List<string> options { get; set; }
                    public int total { get; set; }
                }

                public class Feed
                {
                    public string uri { get; set; }
                    public List<string> options { get; set; }
                }

                public class Followers
                {
                    public string uri { get; set; }
                    public List<string> options { get; set; }
                    public int total { get; set; }
                }

                public class Following
                {
                    public string uri { get; set; }
                    public List<string> options { get; set; }
                    public int total { get; set; }
                }

                public class Groups
                {
                    public string uri { get; set; }
                    public List<string> options { get; set; }
                    public int total { get; set; }
                }

                public class Portfolios
                {
                    public string uri { get; set; }
                    public List<string> options { get; set; }
                    public int total { get; set; }
                }

                public class Videos
                {
                    public string uri { get; set; }
                    public List<string> options { get; set; }
                    public int total { get; set; }
                }

                public class WatchLater
                {
                    public string uri { get; set; }
                    public List<string> options { get; set; }
                    public int total { get; set; }
                }

                public class Shared
                {
                    public string uri { get; set; }
                    public List<string> options { get; set; }
                    public int total { get; set; }
                }
                #endregion
            }
            #endregion
        }

        public class Preferences
        {
            public Videos videos { get; set; }

            #region Sub-Classes
            public class Videos
            {
                public string privacy { get; set; }
            }
            #endregion
        }
        #endregion
    }
}
