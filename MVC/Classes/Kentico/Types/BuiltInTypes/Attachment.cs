using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using CMS.DocumentEngine;

using Kentico.Web.Mvc;

namespace MVC
{
    public static partial class AttachmentExtensions
    {
        /// <summary>
        /// Returns a class name (img-size--vertical or img-size--horizontal) depending on
        /// whether the image is tall or wide
        /// </summary>
        /// <param name="input">Kentico attachment</param>
        /// <returns>Class name based on attachment's dimensions</returns>
        public static string ImageSizingClass(this Attachment input)
        {
            if (input.ImageHeight > input.ImageWidth)
            {
                return "img-size--vertical";
            }
            else
            {
                return "img-size--horizontal";
            }
        }

        public static string GetRoute(this Attachment input)
        {
            System.Web.Mvc.UrlHelper urlHelper = new System.Web.Mvc.UrlHelper(HttpContext.Current.Request.RequestContext);
            
            return urlHelper.Kentico().Attachment(input);
        }
    }
}