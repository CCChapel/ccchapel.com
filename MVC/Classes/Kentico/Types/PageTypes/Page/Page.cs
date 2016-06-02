using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using CMS.DocumentEngine;
using CCC.Helpers;

namespace CMS.DocumentEngine.Types
{
    /// <summary>
    /// Represents a content item of type Page.
    /// </summary>
    public partial class Page : TreeNode
    {
        /// <summary>
        /// All the child sections of the Page
        /// </summary>
        public MultiDocumentQuery Sections
        {
            get
            {
                return DocumentHelper.GetDocuments()
                    .OnCurrentSite()
                    .Path(NodeAliasPath, PathTypeEnum.Children)
                    .NestingLevel(1)
                    .WhereContains("ClassName", "Section")
                    .OrderBy(DatabaseHelpers.OrderByCmsTree)
                    .Published();
            }
        }

        public Attachment SocialImageFile
        {
            get
            {
                var links = (from l in DocumentHelper.GetDocuments().Published()
                             where l.NodeGUID == new Guid(SocialImage)
                             select l);

                if (links.Any())
                {
                    var link = links.First();

                    if (link.ClassName == BasicImage.CLASS_NAME)
                    {
                        BasicImage image = BasicImageProvider.GetBasicImage(link.NodeGUID, SiteHelpers.SiteCulture, SiteHelpers.SiteName);
                        return image.Fields.ImageFile;
                    }
                    else if (link.ClassName == Background.CLASS_NAME)
                    {
                        Background bg = BackgroundProvider.GetBackground(link.NodeGUID, SiteHelpers.SiteCulture, SiteHelpers.SiteName);
                        return bg.Fields.ImageFile;
                    }
                }

                throw new NullReferenceException("Image File Not Found");
            }
        }

        /// <summary>
        /// The prefix value representing the Kentico folder containing pages
        /// </summary>
        public static string PathPrefix
        {
            get
            {
                return "/Pages/";
            }
        }

        /// <summary>
        /// Route Values for the Page
        /// </summary>
        public object RouteValues
        {
            get
            {
                return new
                {
                    controller = "Page",
                    action = "Index",
                    path = NodeAliasPath.Replace(PathPrefix, String.Empty)
                };
            }
        }

        /// <summary>
        /// Get a URL to the Page
        /// </summary>
        public string PageUrl
        {
            get
            {
                //var helper = new UrlHelper(HttpContext.Current.Request.RequestContext);


                return UrlHelpers.UrlHelper.RouteUrl(RouteValues);

                //return NodeAliasPath.Replace(PathPrefix, string.Empty).ToLower();
            }
        }
    }
}