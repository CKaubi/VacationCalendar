using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VacationCalendar.Domain;

namespace VacationCalendar.Views
{
    public class VacationReportRecord
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public int DurationBusinessDays { get; set; }
    }
}