using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CCC.Models
{
    public partial class EventsPage
    {
        public EventList List { get; set; }

        #region Constructors
        public EventsPage() { }

        public EventsPage(DateTime listStartDate)
        {
            List = new EventList(listStartDate);
        }

        public EventsPage(DateTime listStartDate, DateTime listEndDate)
        {
            List = new EventList(listStartDate, listEndDate);
        }

        public EventsPage(DateTime listStartDate, TimeSpan listDateSpan)
        {
            List = new EventList(listStartDate, listDateSpan);
        }
        #endregion

        #region Sub-Classes
        public partial class Featured
        {

        }

        public partial class EventList
        {
            public static TimeSpan defaultSpan = new TimeSpan(60, 0, 0, 0, 0); // DateTime.Now.AddMonths(2).Subtract(DateTime.Now);      //Two months
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }

            #region Constructors
            public EventList()
            {
                StartDate = DateTime.Now;
                EndDate = StartDate.Add(defaultSpan);
            }

            public EventList(DateTime startDate)
            {
                StartDate = startDate;
                EndDate = startDate.Add(defaultSpan);
            }

            public EventList(DateTime startDate, DateTime endDate)
            {
                StartDate = startDate;
                EndDate = endDate;
            }

            public EventList(DateTime startDate, TimeSpan dateSpan)
            {
                StartDate = startDate;
                EndDate = startDate.Add(dateSpan);
            }
            #endregion
        }
        #endregion
    }
}