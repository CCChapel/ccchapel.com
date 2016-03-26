using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Kentico.Search;
using CMS.DocumentEngine.Types;
using CCC.Helpers;

namespace CCC.Models
{
    public partial class SearchResults
    {
        public string Query
        {
            get;
            set;
        }

        public IEnumerable<SearchResultItem> Items
        {
            get;
            set;
        }

        public int ItemCount
        {
            get;
            set;
        }

        public AjaxSearchResults AjaxResults
        {
            get
            {
                return new AjaxSearchResults()
                {
                    Query = Query,
                    TotalItemCount = ItemCount,
                    Items = Items.AjaxItems()
                };
            }
        }

        public partial class AjaxSearchResults
        {
            public int TotalItemCount { get; set; }
            public string Query { get; set; }
            public IEnumerable<AjaxSearchResultItem> Items { get; set; }

            public partial class AjaxSearchResultItem
            {
                public string Title { get; set; }
                public string Url { get; set; }
                public string Description { get; set; }
            }
        }
    }

    public static partial class Extensions
    {
        public static string ItemUrl(this SearchResultItem input)
        {
            switch (input.PageTypeCodeName)
            {
                case Page.CLASS_NAME:
                    return PageProvider.GetPage(input.NodeId, SiteHelpers.SiteCulture, SiteHelpers.SiteName)
                        .FirstOrDefault().PageUrl;
                case ContentSection.CLASS_NAME:
                    return ContentSectionProvider.GetContentSection(input.NodeId, SiteHelpers.SiteCulture, SiteHelpers.SiteName)
                        .FirstOrDefault().RouteUrl;
                case ImageSection.CLASS_NAME:
                    return ImageSectionProvider.GetImageSection(input.NodeId, SiteHelpers.SiteCulture, SiteHelpers.SiteName)
                        .FirstOrDefault().RouteUrl;
                case CrossSellSection.CLASS_NAME:
                    return CrossSellSectionProvider.GetCrossSellSection(input.NodeId, SiteHelpers.SiteCulture, SiteHelpers.SiteName)
                        .FirstOrDefault().RouteUrl;
                case CcbEvent.CLASS_NAME:
                    return CcbEventProvider.GetCcbEvent(input.NodeId, SiteHelpers.SiteCulture, SiteHelpers.SiteName)
                        .FirstOrDefault().RouteUrl;
                case JobListing.CLASS_NAME:
                    return JobListingProvider.GetJobListing(input.NodeId, SiteHelpers.SiteCulture, SiteHelpers.SiteName)
                        .FirstOrDefault().RouteUrl;
                case Sermon.CLASS_NAME:
                    return SermonProvider.GetSermon(input.NodeId, SiteHelpers.SiteCulture, SiteHelpers.SiteName)
                        .FirstOrDefault().RouteUrl;
                case Series.CLASS_NAME:
                    return SeriesProvider.GetSeries(input.NodeId, SiteHelpers.SiteCulture, SiteHelpers.SiteName)
                        .FirstOrDefault().RouteUrl;
                default:
                    throw new NotImplementedException("Unknown PageType: " + input.PageTypeCodeName);
            }
        }

        public static IEnumerable<SearchResults.AjaxSearchResults.AjaxSearchResultItem> AjaxItems(this IEnumerable<SearchResultItem> input)
        {
            List<SearchResults.AjaxSearchResults.AjaxSearchResultItem> list = new List<SearchResults.AjaxSearchResults.AjaxSearchResultItem>();
            foreach (var item in input)
            {
                SearchResults.AjaxSearchResults.AjaxSearchResultItem ajax = new SearchResults.AjaxSearchResults.AjaxSearchResultItem()
                {
                    Title = item.Title,
                    Url = item.ItemUrl(),
                    Description = item.Content
                };

                list.Add(ajax);
            }

            return list;
        }
    }
}