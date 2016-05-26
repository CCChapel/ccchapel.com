using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using CCC.Helpers;

namespace CMS.DocumentEngine.Types
{
    /// <summary>
    /// Represents a content item of type CrossSellSection.
    /// </summary>
    public partial class CrossSellSection : TreeNode
    {
        /// <summary>
        /// The maximum number of CrossSellItems to list at a time
        /// </summary>
        private static int MaxItems = 3;

        public IQueryable<CrossSellItem> CrossSellItems
        {
            get
            {
                string path = NodeAliasPath;

                //Check for Path Override
                if (!string.IsNullOrWhiteSpace(SectionItemsPath))
                {
                    path = SectionItemsPath;
                }


                //Get Cross Sell Items
                var items = CrossSellItemProvider.GetCrossSellItems()
                    .OnCurrentSite()
                    .Path(path, PathTypeEnum.Children)
                    .OrderBy(DatabaseHelpers.OrderByCmsTree)
                    .Published()
                    .Where(c => c.ClassName == CrossSellItem.CLASS_NAME);


                //Check for Current Campus Override
                if (SectionCampusOverride == true)
                {
                    //Swap out campus name if overriding
                    string campusName = string.Format(@"""{0}""", SectionCampusSelection);

                    items = (from i in items
                             where DocumentHelpers.ResolveMacroCondition(
                                 i.MacroCondition.Replace("CurrentCampus()", campusName))
                             select i);
                }
                else
                {
                    items = (from i in items
                             where DocumentHelpers.ResolveMacroCondition(i.MacroCondition)
                             select i);
                }

                    //.Where(c => ((DocumentHelpers.ResolveMacroCondition(macroCondition))
                return items.Take(MaxItems);
            }
        }

        /// <summary>
        /// Get a URL to the CrossSellSection
        /// </summary>
        public string RouteUrl
        {
            get
            {
                //Get Parent Page
                Page page = PageProvider.GetPage(
                    this.Parent.NodeGUID,
                    SiteHelpers.SiteCulture,
                    SiteHelpers.SiteName);
                string url = page.PageUrl;

                //Add anchor
                url += string.Format("#{0}", NodeAlias.ToLower());

                return url;
            }
        }
    }
}