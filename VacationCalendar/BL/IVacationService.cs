using System;
using System.Collections.Generic;
using VacationCalendar.Domain;

namespace VacationCalendar.BL
{
    public interface IVacationService
    {
        List<Vacation> GetAllUsersVacations();
        List<Vacation> GetAllVacationsFor(User user);
        ImportResult AddNewVacation(User user, DateTime from, DateTime to);
        ImportResult AddTotalMonthVacations();
    }
}