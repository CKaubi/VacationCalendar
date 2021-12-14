using System;

namespace VacationCalendar.Domain
{
    public class TotalMonthVacations
    {
        private DateTime _month;
        private int _userId;
        private int _quantityBusinessDays;

        public string Month
        {
            get
            {
                return _month.ToString("yyyyMM");
            }
        }
        public int UserId
        {
            get
            {
                return _userId;
            }
        }
        public int QuantityBusinessDays
        {
            get
            {
                return _quantityBusinessDays;
            }
        }

        public TotalMonthVacations(DateTime month, int userId, int quantityBusinessDays)
        {
            if (quantityBusinessDays < 0) throw new ArgumentOutOfRangeException(nameof(quantityBusinessDays));

            _userId = userId;
            _month = month;
            _quantityBusinessDays = quantityBusinessDays;
        }
    }
}