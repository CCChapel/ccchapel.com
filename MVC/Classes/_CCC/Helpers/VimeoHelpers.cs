using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

using Vimeo;
using Vimeo.Types;

namespace CCC.Helpers
{
    public static partial class VimeoHelpers
    {
        public static partial class Api
        {
            private static string Key
            {
                get
                {
                    return ConfigurationHelper.GetConnectionString("VimeoAPIKey");
                }
            }

            private static string Secret
            {
                get
                {
                    return ConfigurationHelper.GetConnectionString("VimeoAPISecret");
                }
            }

            private static string AccessToken
            {
                get
                {
                    return ConfigurationHelper.GetConnectionString("VimeoAccessToken");
                }
            }

            private static VimeoClient Client
            {
                get
                {
                    return VimeoClient.ReAuthorize(
                            AccessToken,
                            Key,
                            Secret);
                }
            }

            /// <summary>
            /// Get a Vimeo Video object
            /// </summary>
            /// <param name="vimeoID">The ID of the video to retrieve</param>
            /// <returns>A Vimeo Video object</returns>
            public static Video GetVideo(long vimeoID)
            {
                return Client.GetVideo(vimeoID);
            }

            public static string GetVideoDownloadUrl(long vimeoID)
            {
                return Client.GetVideoDownloadUrl(vimeoID);
            }
        }

        public static string CustomEmbedHtml(string embedHtml)
        {
            //<iframe src="https://player.vimeo.com/video/157907090?badge=0&autopause=0&player_id=0" width="1280" height="720" frameborder="0" title="The Promise (Part 4) - God Cuts a Covenant" webkitallowfullscreen mozallowfullscreen allowfullscreen></iframe>
            //<iframe src="https://player.vimeo.com/video/153663975?autoplay=1&color=28708a&title=0&byline=0&portrait=0" width="100%" height="100%" frameborder="0" webkitallowfullscreen mozallowfullscreen allowfullscreen></iframe>

            string widthPattern = @"(?:width)\s*=\s*""[^ ""]*"" * ";
            string heightPattern = @"(?:height)\s*=\s*""[^ ""]*"" * ";

            Regex rgxWidth = new Regex(widthPattern);
            Regex rgxHeight = new Regex(heightPattern);

            //Adjust Size
            //embedHtml = embedHtml.Replace("width=\"1280\"", "width=\"100%\"");
            //embedHtml = embedHtml.Replace("height=\"720\"", "height=\"100%\"");
            embedHtml = rgxWidth.Replace(embedHtml, "width=\"100%\"");
            embedHtml = rgxHeight.Replace(embedHtml, "height=\"100%\"");

            //Set Autoplay
            embedHtml = embedHtml.Replace("player_id=0", "player_id=0&autoplay=1");

            return embedHtml;
        }
    }
}