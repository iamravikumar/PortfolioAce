using System;

namespace PortfolioAce.Models
{
    public static class DateExtentions
    {
        public static DateTime InitialDate()
        {
            // sets the default Date to the next business day (if a weekend) or current business day
            DateTime currentDay = DateTime.Today;
            if (currentDay.DayOfWeek == DayOfWeek.Saturday)
            {
                currentDay = currentDay.AddDays(2);
            }
            else if (currentDay.DayOfWeek == DayOfWeek.Sunday)
            {
                currentDay = currentDay.AddDays(1);
            }
            return currentDay;
        }

    }
}
