using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

using CCC.Helpers;
using Kentico.Web.Mvc;

namespace MVC
{
    public static partial class StringExtenstions
    {
        /// <summary>
        /// Removes all whitespace between HTML elements.
        /// </summary>
        /// <param name="input">HTML to remove excess whitespace from</param>
        /// <returns>Minified HTML</returns>
        public static string RemoveWhitespace(this string input)
        {
            return DocumentHelpers.RemoveExcessWhiteSpace(input);
        }

        /// <summary>
        /// Processes Macros in a given string.
        /// </summary>
        /// <param name="input">HTML to process macros in</param>
        /// <returns>HTML with rendered macro expressions</returns>
        public static string ResolveMacros(this string input)
        {
            return CMS.MacroEngine.MacroResolver.Resolve(input);
        }

        /// <summary>
        /// Processes string so it's ready to be rendered using Html.Raw
        /// </summary>
        /// <param name="input">HTML to process</param>
        /// <returns>Processed HTML</returns>
        public static string ProcessForOutput(this string input)
        {
            return input.ResolveMacros().RemoveWhitespace();
        }

        /// <summary>
        /// Removes HTML tags from the string.
        /// </summary>
        /// <param name="input">String to remove HTML from</param>
        /// <returns>Plain text</returns>
        public static string RemoveHtml(this string input)
        {
            input = Regex.Replace(input, @"<[^>]*(>|$)", " ");
            input = Regex.Replace(input, @"[\r\n]+", string.Empty);

            return input.Trim();
        }

        /// <summary>
        /// Removes an HTML special characters and replaces them with their plain text equivilent.
        /// </summary>
        /// <param name="input"></param>
        /// <returns>Plain text string</returns>
        public static string ReplaceHtmlSpecialCharacters(this string input)
        {
            return input.Replace("&rsquo;", "'")
                        .Replace("&lsquo;", "'")
                        .Replace("&rdquo;", "\"")
                        .Replace("&ldquo;", "\"")
                        .Replace("&ndash;", "-")
                        .Replace("&hellip;", "...");
        }
    }
}