using System;
using VacationCalendar.Common;

namespace VacationCalendar.Domain
{
    public class Vacation
    {
        private DateTime _from;
        private DateTime _to;
        private User _user;

        public DateTime From 
        { 
            get
            {
                return _from;
            }
        }
        public DateTime To 
        { 
            get
            {
                return _to;
            }
        }
        public User User
        {
            get
            {
                return _user;
            }
            set 
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                _user = value;
            }
        }
        public int DurationBusinessDays
        {
            get
            {
                return GetCountBusinessDays();
            }
        }

        public Vacation(User user, DateTime vacationFrom, DateTime vacationTo)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (vacationFrom > vacationTo) throw new ArgumentException("from > to");

            _user = user;
            _from = vacationFrom;
            _to = vacationTo;
        }

        private int GetCountBusinessDays()
        {
            return DateTimeUtils.GetBusinessDaysBetween(From, To);
        }
    }
}