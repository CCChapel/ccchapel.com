﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using CCC.Helpers;
using CMS.MacroEngine;
using CMS.Helpers;

namespace CMS.DocumentEngine.Types
{
    /// <summary>
    /// Represents a content item of type ImageSection.
    /// </summary>
    public partial class ImageSection : TreeNode
    {
        public RandomBackground Background
        {
            get
            {
                return new RandomBackground(Fields.SectionImages);
            }
        }

        /// <summary>
        /// Selects a random background from the set
        /// </summary>
        public class RandomBackground
        {
            public Attachment Url { get; set; }
            public string Positioning { get; set; }

            public RandomBackground(IEnumerable<TreeNode> images)
            {
                //Won't work in Views/PageSection/_Image.cshtml!

                foreach (var image in images
                    .Where(i => (i.ClassName == Types.Background.CLASS_NAME) || 
                                (i.ClassName == MacroLink.CLASS_NAME))
                    .OrderBy(i => Guid.NewGuid()))
                {
                    bool imageOkay = false;
                    Background background = new Background();

                    if (image.ClassName == Types.Background.CLASS_NAME)
                    {
                        background = (Types.Background)image;
                        imageOkay = true;
                    }
                    else if (image.ClassName == MacroLink.CLASS_NAME)
                    {
                        var macroLink = (MacroLink)image;

                        //Check that Macro Link's Target Class Matches
                        if (macroLink.Fields.PageClassName == Types.Background.CLASS_NAME)
                        {
                            //Resolve Macro Expression
                            Guid guid = ValidationHelper.GetGuid(
                                MacroResolver.Resolve(macroLink.Fields.MacroExpression),
                                new Guid());

                            //Get Background
                            background = BackgroundProvider.GetBackground(guid, SiteHelpers.SiteCulture, SiteHelpers.SiteName);

                            imageOkay = true;
                        }
                    }

                    if (imageOkay == true)
                    { 
                        if ((DocumentHelpers.ResolveMacroCondition(background.ImageMacroCondition) == true))
                        {
                            Url = background.Fields.ImageFile;
                            Positioning = background.Fields.ImagePositioning;

                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Get a URL to the ImageSection
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