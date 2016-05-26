using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using CCC.Helpers;

namespace CMS.DocumentEngine.Types
{
    /// <summary>
    /// Represents a content item of type Series.
    /// </summary>
    public partial class Series : TreeNode
    {
        #region Public Properties
        public static readonly string SERMONS_PATH = "/Messages/Sermons";
        public static readonly string SPECIAL_EVENTS_PATH = "/Messages/Special-Events";

        /// <summary>
        /// Returns the Series with the most recent Sermon
        /// </summary>
        public static Series Latest
        {
            get
            {
                //var sermon = (from s in SermonProvider.GetSermons().Published()
                //              orderby s.MessageDate descending
                //              select s).First();

                //return sermon.MessageSeries;

                return Sermon.Latest.MessageSeries;
            }
        }

        public static Series LatestForCampus(string campusName = "hudson")
        {
            return Sermon.LatestForCampus(campusName).MessageSeries;
        }

        /// <summary>
        /// Returns the Series Background
        /// </summary>
        public Background Background
        {
            get
            {
                try
                {
                    return BackgroundProvider.GetBackground(
                        Fields.BackgroundGraphic.First().NodeGUID,
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
        /// Route Values for the Sermon
        /// </summary>
        public object RouteValues
        {
            get
            {
                return new
                {
                    controller = "MessageSeries",
                    action = "Index",
                    seriesTitle = NodeAlias
                };
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

        #region Public Methods
        public DocumentQuery<Sermon> GetSermons(string campus = "")
        {
            //Get Path
            string path = NodeAliasPath;

            //Get Child Sections
            var sermons = SermonProvider.GetSermons()
                .OnCurrentSite()
                .Path(path, PathTypeEnum.Children)
                .NestingLevel(1)
                .OrderBy(CCC.Helpers.DatabaseHelpers.OrderByCmsTree)
                .Published();

            return sermons;
        }
        #endregion
    }
}