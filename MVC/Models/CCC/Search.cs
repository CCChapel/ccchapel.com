using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Kentico.Search;

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
    }
}