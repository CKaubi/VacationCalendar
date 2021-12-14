using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Xml.Linq;
using VacationCalendar.Common;
using VacationCalendar.DL;
using VacationCalendar.Domain;

namespace VacationCalendar.BL
{
    public class VacationService : IVacationService
    {
        private readonly VacationRepository _repository;

        public VacationService(VacationRepository repository)
        {
            _repository = repository;
        }

        public List<Vacation> GetAllUsersVacations()
        {
            return _repository.GetAllVacations();
        }

        public List<Vacation> GetAllVacationsFor(User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            return _repository.GetUserVacations(user);
        }

        public ImportResult AddNewVacation(User user, DateTime from, DateTime to)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            var allVacations = _repository.GetUserVacations(user);
            var isValid = IsIntersectionDateValid(allVacations, from, to);

            if (!isValid)
            {
                return new ImportResult(false, "Vacation periods intersect");
            }

            Vacation vacation = new Vacation(user, from, to);

            List<Vacation> vacations = VacationMapper.DivideVacationIntoMonth(vacation);

            try
            {
                _repository.AddCollection(vacations);

                return new ImportResult(true, "SUCCES");
            }
            catch (Exception exeption)
            {
                return new ImportResult(false, exeption.Message);
            }
        }

        public ImportResult AddTotalMonthVacations()
        {
            List<Vacation> vacations = _repository.GetVacationsForLastMonth();

            if (vacations.Count == 0)
            {
                return new ImportResult(false, "There are no records for the last month");
            }

            List<TotalMonthVacations> totals = VacationMapper.MapToListTotalMonthVacationsFrom(vacations);

            try
            {
                _repository.AddTotalMonthVacations(totals);

                return new ImportResult(true, "SUCCES");
            }
            catch (Exception exeption)
            {
                return new ImportResult(false, exeption.Message);
            }
        }        

        private bool IsIntersectionDateValid(
            List<Vacation> allVacations, DateTime from, DateTime to)
        {
            if (allVacations == null) throw new ArgumentNullException(nameof(allVacations));

            if (allVacations.Any(vacation => vacation.To == from)) return false;

            if (allVacations.Any(vacation => vacation.From == to)) return false;

            if (allVacations.Any(vacation => vacation.From < to && vacation.To>from)) return false;

            return true;
        }
    }
}