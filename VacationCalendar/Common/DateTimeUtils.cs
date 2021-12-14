using System;

namespace VacationCalendar.Common
{
    public class DateTimeUtils
    {
        public static int GetBusinessDaysBetween(DateTime from, DateTime to)
        {
            if (from > to) throw new ArgumentException("from > @to");

            int countBusinessDays = 0;

            while (from <= to.Date)
            {
                if (from.DayOfWeek != DayOfWeek.Saturday && from.DayOfWeek != DayOfWeek.Sunday)
                {
                    countBusinessDays++;
                }

                from = from.AddDays(1);
            }

            return countBusinessDays;
        }      

        public static DateTime GetPreviousMonthDate()
        {
            return DateTime.Now.AddMonths(-1);
        }
    }
}