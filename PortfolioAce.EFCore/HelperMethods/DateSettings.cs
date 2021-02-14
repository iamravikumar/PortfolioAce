using System;
using System.Collections.Generic;

namespace PortfolioAce.EFCore.HelperMethods
{
    public static class DateSettings
    {
        public static List<DateTime> AnnualWorkingDays(DateTime startDate)
        {
            //gets a years worth of working days from the startdate.
            DateTime endDate = startDate.AddYears(1);
            DateTime newDate = startDate;
            List<DateTime> allDates = new List<DateTime>();
            while (newDate < endDate)
            {
                if (newDate.DayOfWeek != DayOfWeek.Saturday && newDate.DayOfWeek != DayOfWeek.Sunday)
                {
                    allDates.Add(newDate);
                }
                newDate = newDate.AddDays(1);
            }
            return allDates;
        }

        public static List<DateTime> AnnualMonthEnds(DateTime startDate)
        {
            // get the next 12month end.
            DateTime endDate = startDate.AddYears(1);
            DateTime newDate = startDate;
            List<DateTime> allDates = new List<DateTime> { startDate };
            while (newDate < endDate)
            {
                newDate = MonthEndWorkingDate(newDate);
                allDates.Add(newDate);
                newDate = newDate.AddMonths(1);
            }
            return allDates;
        }

        private static DateTime MonthEndWorkingDate(DateTime myDate)
        {
            DateTime lastBusinessDay = new DateTime(myDate.Year, myDate.Month, DateTime.DaysInMonth(myDate.Year, myDate.Month));
            if (lastBusinessDay.DayOfWeek == DayOfWeek.Sunday)
            {
                lastBusinessDay = lastBusinessDay.AddDays(-2);
            }

            else if (lastBusinessDay.DayOfWeek == DayOfWeek.Saturday)
            {
                lastBusinessDay = lastBusinessDay.AddDays(-1);
            }
            return lastBusinessDay;
        }

    }
}
