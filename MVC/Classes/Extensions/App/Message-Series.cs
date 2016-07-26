using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using MVC;

using CMS.DocumentEngine.Types;

using CCC.Models.App.Objects;
using CCC.Models.App.Items;
using CCC.Models.App.Actions;

namespace CCC.Models.App
{
    public static partial class Message_Series
    {
        /// <summary>
        /// Returns the image set for the series, usable in the app
        /// </summary>
        /// <param name="input">The Series</param>
        /// <returns>Image Set</returns>
        public static IEnumerable<Image> ImageSet(this Series input)
        {
            var image = new Image()
            {
                Url = string.Format("{0}{1}",
                        Helpers.UrlHelpers.CurrentDomainName,
                        input.Background.Fields.ImageFile.GetRoute()),
                Width = input.Background.Fields.ImageFile.ImageWidth
            };

            var seriesImages = new List<Image>();
            seriesImages.Add(image);

            return seriesImages;
        }

        /// <summary>
        /// Returns the series as a ListItem, usable in the app
        /// </summary>
        /// <param name="input">The Series</param>
        /// <returns>Series as ListItem</returns>
        public static ListItem ListItem(this Series input)
        {
            return new ListItem()
            {
                Title = input.Fields.Title,
                Images = input.ImageSet(),
                Actions = input.ListActions()
            };
        }

        /// <summary>
        /// Returns a set of ListActions for the series
        /// </summary>
        /// <param name="input">The Series</param>
        /// <param name="url">The URL of the Action</param>
        /// <returns>Set of ListActions taking user from Series List to the Series Details</returns>
        public static IEnumerable<ListAction> ListActions(this Series input, string url = null)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                url = string.Format("{0}{1}/series?seriesAlias={2}",
                        Helpers.UrlHelpers.CurrentDomainName,
                        Helpers.UrlHelpers.CurrentRoute,
                        input.NodeAlias);
            }

            ListAction action = new ListAction()
            {
                Type = Actions.Action.ActionTypes.Navigation,
                Handler = Actions.Action.Handlers.List,
                //Style = ListAction.Styles.Grid,
                Url = url
            };
            var actions = new List<ListAction>();
            actions.Add(action);

            return actions;
        }
    }
}