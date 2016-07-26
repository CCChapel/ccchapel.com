using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using CCC.Models.App.Objects;
using CCC.Models.App.Items;
using CCC.Models.App.Actions;

using CMS.DocumentEngine.Types;

namespace CCC.Models.App
{
    public static partial class Message
    {
        /// <summary>
        /// Returns the message as a ListItem, usable in the app
        /// </summary>
        /// <param name="input">The Message</param>
        /// <returns>Message as ListItem</returns>
        public static ListItem ListItem(this Sermon input)
        {
            return new ListItem()
            {
                Title = input.Fields.MessageTitle,
                Subtitle = input.MessageSpeaker.FullName,
                Date = input.MessageDate,
                Images = input.MessageSeries.ImageSet(),
                //Position = i.ToString(),
                Actions = input.Actions()
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
                Url = string.Format("{0}{1}/message?messageAlias={2}",
                        Helpers.UrlHelpers.CurrentDomainName,
                        Helpers.UrlHelpers.CurrentRoute.Replace("/series", string.Empty),
                        input.NodeAlias)
            });

            return actions;
        }
    }
}