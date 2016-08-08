using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using CCC.Models.App.Items;
using CCC.Models.App.Actions;

namespace CCC.Models.App
{
    public static partial class Calendar
    {
        public static ListItem ListItem(this ChurchCommunityBuilder.Api.Events.Entity.Calendar input)
        {
            List<ListAction> actions = new List<ListAction>();
            actions.Add(new ListAction()
            {
                Handler = Actions.Action.Handlers.List,
                Style = ListAction.Styles.Plain,
                Type = Actions.Action.ActionTypes.Navigation,
                Url = "#"
            });
            return new ListItem()
            {
                Title = input.EventName.Value,
                Subtitle = input.EventType,
                Date = input.Date.Value,
                Actions = actions 
            };
        }
    }
}