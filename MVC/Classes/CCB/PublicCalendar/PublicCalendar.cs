using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using CCC.Helpers;
using CMS.CustomTables;
using CMS.CustomTables.Types;

using ChurchCommunityBuilder.Api.Events.Entity;
using ChurchCommunityBuilder.Api.Events.QueryObject;

namespace CCB
{
    public partial class PublicCalendar
    {
        /// <summary>
        /// Gets a collection of Events from the Public Calendar during the specified dates
        /// </summary>
        /// <param name="startDate">The first date to include in the collection</param>
        /// <param name="endDate">The last date to include in the collection</param>
        /// <returns>A collection of Events from the Public Calendar</returns>
        public static CalendarCollection GetEvents(DateTime startDate, DateTime endDate)
        {
            string id = string.Format("{0}_{1}", startDate.ToShortDateString(), endDate.ToShortDateString());
            string cacheID = CachingHelpers.CachingID("CcbPublicCalendar", id);

            if (CachingHelpers.Cache.Contains(cacheID))
            {
                //Get Data From Cache
                CalendarCollection cc = (CalendarCollection)CachingHelpers.Cache.Get(cacheID);

                return cc;
            }
            else
            {
                //Create Query Object
                var qo = new CalendarQO { DateStart = startDate, DateEnd = endDate };

                //Get Events
                CalendarCollection cc = Api.Client.Events.Calendar.List(qo);

                //Add to Cache
                CachingHelpers.Cache.Add(cacheID, cc, CachingHelpers.Policy);

                //Return Events
                return cc;
            }
        }

        /// <summary>
        /// Gets a collection of Events from the Public Calendar for the next month
        /// </summary>
        /// <returns>A collection of Events from the Public Calendar</returns>
        public static CalendarCollection GetEvents()
        {
            return GetEvents(DateTime.Now, DateTime.Now.AddMonths(1));
        }
    }

    public static partial class Extensions
    {
        public static CampusesItem CampusInfo(this Calendar input)
        {
            //Start with Group
            int groupID = input.GroupName.CCBID ?? default(int);

            //Get Group
            var group = Groups.GetGroup(groupID);

            return CustomTableItemProvider.GetItems<CampusesItem>()
                .Where(c => c.CampusCcbID == group.CampusCcbID)
                .FirstOrDefault();
        }

        public static ChurchCommunityBuilder.Api.Events.Entity.Event EventInfo(this Calendar input)
        {
            int eventID = input.EventName.CCBID.Value;

            return Event.GetEvent(eventID);
        }

        public static object RouteValues(this Calendar input)
        {
            return new
            {
                controller = "Event",
                action = "Index",
                year = input.Date.Value.Year.ToString("0000"),
                month = input.Date.Value.Month.ToString("00"),
                day = input.Date.Value.Day.ToString("00"),
                eventName = input.EventName
            };
        }
    }
}