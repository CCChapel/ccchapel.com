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
        private static string index = "CCChapel";
        public static readonly string[] searchIndexes = new string[] { index };
        private const int PAGE_SIZE = 50;

        private readonly SearchService mService =
            new SearchService(searchIndexes, SiteHelpers.SiteCulture, SiteHelpers.SiteName, false);

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

        #region Constructors
        public SearchResults(string query, int page = 0)
        {
            int numberOfResults;

            Items = mService.Search(
                        query,
                        page: page,
                        pageSize: PAGE_SIZE,
                        numberOfResults: out numberOfResults)
                    .ProcessResults();
            Query = query;
            ItemCount = Items.Count();
        }

        public SearchResults() { }
        #endregion

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
        /// <summary>
        /// Removes Page Section results, replaces them with their parent page and then removes duplicates.
        /// Also, checks cache for Event Data from CCB and populates the content, if the data is cached.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static IEnumerable<SearchResultItem> ProcessResults(this IEnumerable<SearchResultItem> input)
        {
            //Convert to List
            var list = input.ToList();

            for (int i = 0; i < list.Count; i++)
            {
                //Get Item
                var item = list[i];

                //Check Item Class
                if (
                    (item.PageTypeCodeName == ContentSection.CLASS_NAME) ||
                    (item.PageTypeCodeName == ImageSection.CLASS_NAME) ||
                    (item.PageTypeCodeName == CrossSellSection.CLASS_NAME)
                   )
                {
                    //Get Parent Page
                    int parentID = CMS.DocumentEngine.DocumentHelper.GetDocuments(item.PageTypeCodeName).WhereEquals("NodeId", item.NodeId).FirstOrDefault().NodeParentID;
                    Page parentPage = PageProvider.GetPage(parentID, SiteHelpers.SiteCulture, SiteHelpers.SiteName);

                    //Create new SearchResultItem for Parent
                    SearchResultItem newItem = new SearchResultItem()
                    {
                        Title = parentPage.Fields.Name,
                        Content = item.Content,
                        Date = item.Date,
                        NodeId = parentID,
                        PageTypeCodeName = parentPage.NodeClassName,
                        PageTypeDispayName = parentPage.ClassName,
                        ImageAttachment = item.ImageAttachment
                    };

                    //Remove original item and replace with new item
                    list.Remove(item);
                    list.Insert(i, newItem);
                }
                else if (item.PageTypeCodeName == CcbEvent.CLASS_NAME)
                {
                    //Get Event
                    CcbEvent ev = CcbEventProvider.GetCcbEvent(item.NodeId, SiteHelpers.SiteCulture, SiteHelpers.SiteName);

                    //Check if event is in Cache
                    if (ev.IsEventDataInCache == true)
                    {
                        //Create new SearchResultItem for Parent
                        SearchResultItem newItem = new SearchResultItem()
                        {
                            Title = item.Title,
                            Content = ev.CcbEventData.Description,
                            Date = item.Date,
                            NodeId = item.NodeId,
                            PageTypeCodeName = item.PageTypeCodeName,
                            PageTypeDispayName = item.PageTypeDispayName,
                            ImageAttachment = item.ImageAttachment
                        };

                        //Remove original item and replace with new item
                        list.Remove(item);
                        list.Insert(i, newItem);
                    }
                }
            }

            //Remove duplicates and return list
            input = list.GroupBy(g => g.NodeId)
                        .Select(g => g.First());

            return input;
        }

        /// <summary>
        /// Formats the content for search results
        /// </summary>
        /// <param name="input"></param>
        /// <param name="query">The query text</param>
        /// <returns></returns>
        public static string ModifiedContent(this SearchResultItem input, string query = null)
        {
            int finalSize = 240;
            char[] punctuation = { '.', '!', '?' };

            string content = input.StrippedContent();

            if (string.IsNullOrWhiteSpace(query))
            {
                if (content.Length < finalSize)
                {
                    return content;
                }
                else
                {
                    int index = content.IndexOfAny(punctuation, finalSize);
                    return string.Format("{0}&hellip;", content.Remove(index));
                }
            }
            else
            {

                //Get index of search query
                int index = content.IndexOf(query);

                if (index < 0)
                {
                    index = 0;
                }

                //Clip string +/- 30 characters from search query
                int start = index - (finalSize / 2);
                if (start < 0)
                {
                    start = 0;
                }
                else
                {
                    //Start at the first whitespace
                    start = content.IndexOfAny(punctuation, start) + 1;
                }

                content = content.Remove(0, start).TrimStart();
                //content = string.Format("&hellip;{0}", content.Remove(0, start).TrimStart());

                if (content.Length > finalSize)
                {
                    content = content.Remove(finalSize);

                    //Find last whitespace
                    int end = content.LastIndexOfAny(punctuation);

                    if (end > 0)
                    {
                        content = string.Format("{0}&hellip;", content.Remove(end).TrimEnd());
                    }
                }

                //Highlight Query
                return content.Replace(query, string.Format("<b>{0}</b>", query));
            }
        }

        /// <summary>
        /// Removes all appropriate excess markup and content from the search result
        /// </summary>
        /// <param name="input"></param>
        /// <returns>Search result minus all excess markup and content</returns>
        public static string StrippedContent(this SearchResultItem input)
        {
            return DocumentHelpers.RemoveHtmlTags(
                        DocumentHelpers.RemoveMacroExpressions(input.Content)
                    ).Replace("&nbsp;", " ").Replace("\r\n", string.Empty).Trim();
        }

        /// <summary>
        /// Returns the Route Url for the given search result
        /// </summary>
        /// <param name="input"></param>
        /// <returns>The Route Url for the given search result</returns>
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

        /// <summary>
        /// Creates an object appropriate for asycronous results
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static IEnumerable<SearchResults.AjaxSearchResults.AjaxSearchResultItem> AjaxItems(this IEnumerable<SearchResultItem> input)
        {
            List<SearchResults.AjaxSearchResults.AjaxSearchResultItem> list = new List<SearchResults.AjaxSearchResults.AjaxSearchResultItem>();

            foreach (var item in input)
            {
                SearchResults.AjaxSearchResults.AjaxSearchResultItem ajax = new SearchResults.AjaxSearchResults.AjaxSearchResultItem()
                {
                    Title = item.Title,
                    Url = item.ItemUrl(),
                    Description = item.ModifiedContent()
                };

                list.Add(ajax);
            }

            return list;
        }
    }
}