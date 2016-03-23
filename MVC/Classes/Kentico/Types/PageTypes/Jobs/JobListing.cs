using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using CMS.CustomTables;
using CMS.CustomTables.Types;
using CCC.Helpers;

namespace CMS.DocumentEngine.Types
{
    /// <summary>
    /// Represents a content item of type JobListing.
    /// </summary>
    public partial class JobListing : TreeNode
    {
        /// <summary>
        /// The Campus information for this Job Listing
        /// </summary>
        public CampusesItem Campus
        {
            get
            {
                return CustomTableItemProvider.GetItem<CampusesItem>(this.JobCampusID);
            }
        }

        /// <summary>
        /// Route Values for the Job Listing
        /// </summary>
        public object RouteValues
        {
            get
            {
                return new
                {
                    controller = "Jobs",
                    action = "Index",
                    jobTitle = NodeAlias
                };
            }
        }

        /// <summary>
        /// The Route URL to the Job Listing
        /// </summary>
        public string RouteUrl
        {
            get
            {
                return UrlHelpers.UrlHelper.RouteUrl(RouteValues);
            }
        }
    }
}