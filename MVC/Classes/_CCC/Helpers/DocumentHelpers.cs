using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;


using CMS.DocumentEngine.Types;
using CMS.MacroEngine;
using CMS.Base;
using System.Text;

namespace CCC.Helpers
{
    public static partial class DocumentHelpers
    {
        //public static Type GetTypeFromClassName(string className)
        //{
        //    switch (className)
        //    {
        //        case "pages.crossSellSection":
        //            return typeof(CrossSellSection);
        //            break;
        //        case "pages.imageSection":
        //            return typeof(ImageSection);
        //            break;
        //        case "pages.contentSection":
        //            return typeof(ContentSection);
        //            break;
        //    }

        //    throw new NotImplementedException("Class name not recognized");
        //}

        public static bool ResolveMacroCondition(string condition)
        {
            if (String.IsNullOrWhiteSpace(condition))
            {
                return true;
            }
            else
            {
                //Resolve Macro
                string output = MacroResolver.Resolve(condition);

                //Cast as bool
                bool resolution = CMS.Helpers.ValidationHelper.GetBoolean(output, false);

                return resolution;
            }
        }

        public static string RemoveMacroExpressions(string html)
        {
            return Regex.Replace(html, @"{%.*%}", string.Empty);
        }

        public static string RemoveHtmlTags(string html)
        {
            return Regex.Replace(html, @"<[^>]*>", string.Empty);
        }

        public static string RemoveExcessWhiteSpace(string html)
        {
            Regex REGEX_BETWEEN_TAGS = new Regex(@">\s+<", RegexOptions.Compiled);
            Regex REGEX_LINE_BREAKS = new Regex(@"\n\s+", RegexOptions.Compiled);

            html = REGEX_BETWEEN_TAGS.Replace(html, "><");
            html = REGEX_LINE_BREAKS.Replace(html, string.Empty);

            return html;
        }   

        public static string LineBreaksToParagraphs(string html)
        {
            string breaks = string.Format("<br />\n<br />\n");
            string breaks2 = string.Format("\n");
            string paragraphs = "</p><p>";

            return string.Format("<p>{0}</p>", 
                html
                    .Replace(breaks, paragraphs)
                    .Replace(breaks2, paragraphs)
                    .Replace("<p></p>", string.Empty));
        }
    }
}