using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using VacationCalendar.Domain;

namespace VacationCalendar.Common
{
    public class XmlMapper
    {
        public static XDocument MapToXDocumentFrom(List<TotalMonthVacations> totals)
        {
            if (totals == null) throw new ArgumentNullException(nameof(totals));

            XDocument xTotalsDoc = new XDocument();

            XElement xTotals = new XElement("totals");

            foreach (TotalMonthVacations total in totals)
            {
                XElement xTotal = MapToXElementFrom(total);

                xTotals.Add(xTotal);
            }

            xTotalsDoc.Add(xTotals);

            return xTotalsDoc;
        }

        private static XElement MapToXElementFrom(TotalMonthVacations total)
        {
            if (total == null) throw new ArgumentNullException(nameof(total));

            XElement xTotal = new XElement("total");

            XElement totalMonth = new XElement("month", total.Month);
            XElement totalUserId = new XElement("userId", total.UserId);
            XElement totalDays = new XElement("days", total.QuantityBusinessDays);

            xTotal.Add(totalMonth);
            xTotal.Add(totalUserId);
            xTotal.Add(totalDays);

            return xTotal;
        }
    }
}