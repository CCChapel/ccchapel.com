using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using CCC.Helpers;
using CCC.Models.App.Objects;
using CCC.Models.App.Items;

using CMS.DocumentEngine.Types;

namespace CCC.Models.App
{
    public static partial class Message
    {
        /// <summary>
        /// Returns the message as a ListItem, usable in the app
        /// </summary>
        /// <param name="input">The Message</param>
        /// <param name="subtitle">Set a custom Subtitle. By default, the Speaker's Name is used.</param>
        /// <param name="position">The position value</param>
        /// <param name="useImages">Set to true to use Sermon Series art</param>
        /// <param name="useDate">Set to true to display date.</param>
        /// <returns>Message as ListItem</returns>
        public static ListItem ListItem(this Sermon input, string subtitle = null, string position = null, bool useImages = false, bool useDate = false)
        {
            var listItem = new ListItem()
            {
                Title = input.Fields.MessageTitle,
                Subtitle = input.MessageSpeaker.FullName,
                Actions = input.Actions()
            };

            if (string.IsNullOrWhiteSpace(subtitle))
            {
                listItem.Subtitle = string.Format("{0}", input.MessageSpeaker.FullName);
            }

            if (!string.IsNullOrWhiteSpace(position))
            {
                listItem.Position = position;
            }
            else if (useImages == true)
            {
                listItem.Images = input.MessageSeries.ImageSet();
            }
            else if (useDate == true)
            {
                listItem.Date = input.MessageDate;
            }

            return listItem;
        }

        public static IEnumerable<Media> MediaItems(this Sermon input)
        {
            List<Media> media = new List<Media>();

            if (input.HasVideo)
            {
                media.Add(input.MediaItem(Media.Formats.MP4));
            }

            if (!string.IsNullOrWhiteSpace(input.DownloadUrlAudio))
            {
                media.Add(input.MediaItem(Media.Formats.MP3));
            }

            return media;
        }

        public static Media MediaItem(this Sermon input, Media.Formats format = Media.Formats.MP3)
        {
            string url = null;

            switch (format)
            {
                case Media.Formats.MP4:
                    url = string.Format("{0}{1}",
                            UrlHelpers.CurrentDomainName,
                            input.DownloadUrlVideo);
                    break;
                default:
                    url = input.DownloadUrlAudio;
                    break;
            }
             
            return new Media()
            {
                SapID = input.DocumentGUID,
                Url = url,
                Format = format,
                Downloadable = true,
                Images = input.MessageSeries.ImageSet()
            };
        }

        /// <summary>
        /// Returns a set of ListActions for the message
        /// </summary>
        /// <param name="input">The Message</param>
        /// <param name="url">The URL of the Action</param>
        /// <returns>Set of ListActions taking user from Message List to the Message Details</returns>
        public static IEnumerable<Actions.Action> Actions(this Sermon input)
        {
            List<Actions.Action> actions = new List<Actions.Action>();
            actions.Add(new Actions.Action()
            {
                Handler = App.Actions.Action.Handlers.MediaDetial,
                Type = App.Actions.Action.ActionTypes.Navigation,
                Url = string.Format("{0}{1}",
                        UrlHelpers.CurrentDomainName,
                        input.AppRouteUrl())
            });

            return actions;
        }

        /// <summary>
        /// Route Values for the Sermon
        /// </summary>
        private static object AppRouteValues(this Sermon input)
        {
            return new
            {
                controller = "Messages",
                action = "Message",
                messageAlias = input.NodeAlias
            };
        }

        /// <summary>
        /// Returns the Route URL, usable for loading information in the app
        /// </summary>
        /// <param name="input"></param>
        /// <returns>Route URL</returns>
        public static string AppRouteUrl(this Sermon input)
        {
            return Helpers.UrlHelpers.UrlHelper.HttpRouteUrl("AppApi", input.AppRouteValues());
        }
    }
}