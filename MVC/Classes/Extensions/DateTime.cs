using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC
{
    public static partial class DateTimeExtenstions
    {
        /// <summary>
        /// Returns true if the DateTime matches a null DateTime value
        /// </summary>
        /// <param name="input">DateTime to test</param>
        /// <returns>True if the DateTime matches a null DateTime value</returns>
        public static bool IsNull(this DateTime input)
        {
            if (input == CMS.Helpers.DateTimeHelper.ZERO_TIME)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns a nullable string representation of the DateTime
        /// </summary>
        /// <param name="input">The initial DateTime</param>
        /// <param name="format">The DateTime format to return</param>
        /// <returns>A nullable string representation of the DateTime</returns>
        public static string ToDateStringOrNull(this DateTime input, string format = "M/d/yyyy")
        {
            if (input.IsNull())
            {
                return null;
            }
            else
            {
                return input.ToString(format);
            }
        }

        /// <summary>
        /// Returns the first non-null DateTime value of the specified set
        /// </summary>
        /// <param name="input">Initial value to test</param>
        /// <param name="args">Values to compare against</param>
        /// <returns>The first non-null DateTime value from the specified set</returns>
        public static DateTime Coalesce(this DateTime input, params DateTime[] args)
        {
            //Create string for result and populate with initial value
            string resultString = input.ToDateStringOrNull();

            //Interate through each argument
            foreach (DateTime date in args)
            {
                //Coalesce strings
                resultString = resultString ?? date.ToDateStringOrNull();
            }

            //Return DateTime representation of resultString
            return CMS.Helpers.ValidationHelper.GetDate(resultString, CMS.Helpers.DateTimeHelper.ZERO_TIME);
        }

        /// <summary>
        /// Creates a string representation of the timespan between two times
        /// </summary>
        /// <param name="startTime">The start time</param>
        /// <param name="endTime">The end time</param>
        /// <param name="format">The time format string to use</param>
        /// <returns>A string representation of the timespan between both times</returns>
        public static string ToTimeRangeString(this DateTime startTime, DateTime endTime, string format = "h:mm tt")
        {
            if (startTime.TimeOfDay > endTime.TimeOfDay)
            {
                throw new InvalidOperationException("endTime must be after startTime.");
            }

            //startTime is endTime
            if (startTime.TimeOfDay == endTime.TimeOfDay)
            {
                return RemoveZeroMinutes(startTime.ToShortTimeString());
            }

            //All Day
            if ((startTime.Hour == 0 && startTime.Minute == 0) &&
                (endTime.Hour == 23 && endTime.Minute == 59))
            {
                return "";
            }

            //Spanning a.m. to p.m.
            if (startTime.TimeOfDay.TotalHours < 12 && endTime.TimeOfDay.TotalHours >= 12)
            {
                //##:## a.m. &ndash; ##:## p.m.
                return string.Format("{0} &ndash; {1}",
                    RemoveZeroMinutes(startTime.ToString(format)),
                    RemoveZeroMinutes(endTime.ToString(format)));
            }
            else
            {
                //##:##-##:## a.m.
                return string.Format("{0}-{1}",
                    RemoveZeroMinutes(startTime.ToString(format.Replace("t", string.Empty).TrimEnd())),    //Remove a.m.
                    RemoveZeroMinutes(endTime.ToString(format)));
            }
        }

        private static string RemoveZeroMinutes(string input)
        {
            return input.Replace(":00", string.Empty);
        }

        /// <summary>
        /// Creates a string representation of the date span between two dates
        /// </summary>
        /// <param name="startDate">The start date</param>
        /// <param name="endDate">The end date</param>
        /// <param name="format">The date format string to use</param>
        /// <returns></returns>
        public static string ToDateRangeString(this DateTime startDate, DateTime endDate, string format = "MMM d, yyyy")
        {
            if (startDate.Date > endDate.Date)
            {
                throw new InvalidOperationException("endDate must be after startDate.");
            }

            if (startDate.Date == endDate.Date)
            {
                return startDate.ToString(format);
            }
            
            if (startDate.Year < endDate.Year)
            {
                //MMM d, yyyy &ndash; MMM d, yyyy
                return string.Format("{0} &ndash; {1}",
                    startDate.ToString(format),
                    endDate.ToString(format));
            }
            else
            {
                //MMM d-MMM d, yyyy
                return string.Format("{0}-{1}",
                    startDate.ToString(format.Replace("y", string.Empty).TrimEnd()),    //Remove year
                    endDate.ToString(format));
            }
        }
    }
}