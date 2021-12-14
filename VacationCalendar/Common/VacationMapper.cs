using System;
using System.Collections.Generic;
using System.Linq;
using VacationCalendar.Domain;
using VacationCalendar.Views;

namespace VacationCalendar.Common
{
    public class VacationMapper
    {
        public static VacationReportRecord MapToVacationReportRecord(Vacation vacation)
        {
            if (vacation == null) throw new ArgumentNullException(nameof(vacation));

            VacationReportRecord report = new VacationReportRecord();

            report.Id = vacation.User.Id;
            report.Name = vacation.User.Name;
            report.From = vacation.From;
            report.To = vacation.To;
            report.DurationBusinessDays = vacation.DurationBusinessDays;

            return report;
        }

        public static List<Vacation> DivideVacationIntoMonth(Vacation vacation)
        {
            if (vacation == null) throw new ArgumentNullException(nameof(vacation));

            DateTime from = vacation.From;
            DateTime to = vacation.To;

            List<Vacation> vacations = new List<Vacation>();

            while (from.Month != to.Month || from.Year != to.Year)
            {
                int daysBeforeEndOfMonth = DateTime.DaysInMonth(from.Year, from.Month) - from.Day;

                DateTime dateEndCurrentMonth = from.AddDays(daysBeforeEndOfMonth);

                vacations.Add(new Vacation(vacation.User,from, dateEndCurrentMonth));

                from = dateEndCurrentMonth.AddDays(1);
            }

            vacations.Add(new Vacation(vacation.User, from, to));
            return vacations;
        }

        public static List<TotalMonthVacations> MapToListTotalMonthVacationsFrom(List<Vacation> vacations)
        {
            if (vacations == null) throw new ArgumentNullException(nameof(vacations));

            DateTime lastMonth = DateTimeUtils.GetPreviousMonthDate();

            List<int> usersId = vacations.Select(vacation => vacation.User.Id).Distinct().ToList();

            List<TotalMonthVacations> totals = new List<TotalMonthVacations>();

            foreach (int userId in usersId)
            {
                int quantityBusinessDays = vacations.Where(vacation => vacation.User.Id == userId).Sum(vacation => vacation.DurationBusinessDays);

                TotalMonthVacations total = new TotalMonthVacations(lastMonth, userId, quantityBusinessDays);

                totals.Add(total);
            }

            return totals;
        }
    }
}