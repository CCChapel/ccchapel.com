using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using CCC.Models.App.Objects;
using CCC.Models.App.Items;
using CCC.Models.App.Actions;

using CMS.CustomTables;

namespace CCC.Models.App
{
    public static partial class CampusesItem
    {
        /// <summary>
        /// Route Values for the Series
        /// </summary>
        private static object AppRouteValues(this CMS.CustomTables.Types.CampusesItem input)
        {
            return new
            {
                controller = "Events",
                action = "Index",
                campusName = input.CampusCodeName
            };
        }

        /// <summary>
        /// Returns the Route URL, usable for loading information in the app
        /// </summary>
        /// <param name="input"></param>
        /// <returns>Route URL</returns>
        public static string AppRouteUrl(this CMS.CustomTables.Types.CampusesItem input)
        {
            return Helpers.UrlHelpers.UrlHelper.HttpRouteUrl("AppApi", input.AppRouteValues());
        }

        public static IEnumerable<ListAction> ListActions(this CMS.CustomTables.Types.CampusesItem input)
        {
            ListAction action = new ListAction()
            {
                Type = Actions.Action.ActionTypes.Navigation,
                Handler = Actions.Action.Handlers.List,
                //Style = ListAction.Styles.Grid,
                Url = input.AppRouteUrl()
            };
            var actions = new List<ListAction>();
            actions.Add(action);

            return actions;
        }

        public static ListItem ListItem(this CMS.CustomTables.Types.CampusesItem input)
        {
            return new ListItem
            {
                Title = input.Fields.CampusName,
                //Images = input.ImageSet(),
                Actions = input.ListActions()
            };
        }

        public static IEnumerable<ListItem> ListItems(this CMS.CustomTables.Types.CampusesItem input)
        {
            //Create List
            List<ListItem> list = new List<ListItem>();

            //Get Campuses
            var campuses = CustomTableItemProvider.GetItems<CMS.CustomTables.Types.CampusesItem>();

            //Add campuses to List
            foreach (var campus in campuses)
            {
                list.Add(campus.ListItem());
            }

            return list;
        }
    }
}