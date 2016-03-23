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
                //Get Cross Sell Items
                return CrossSellItemProvider.GetCrossSellItems()
                    .OnCurrentSite()
                    .Path(NodeAliasPath, PathTypeEnum.Children)
                    .NestingLevel(1)
                    .OrderBy(DatabaseHelpers.OrderByCmsTree)
                    .Published()
                    .Where(c => ((DocumentHelpers.ResolveMacroCondition(c.MacroCondition)) &&
                                 (c.ClassName == CrossSellItem.CLASS_NAME)))
                    .Take(MaxItems);
            }
        }
    }
}