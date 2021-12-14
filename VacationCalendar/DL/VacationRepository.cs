using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Xml.Linq;
using Dapper;
using VacationCalendar.Common;
using VacationCalendar.Domain;

namespace VacationCalendar.DL
{
    public class VacationRepository
    {
        private readonly string connectionString =
            ConfigurationManager.ConnectionStrings["TimeSheetSystem"].ConnectionString;

        private const int DUPLICATE_KEY_ERROR_CODE = 2627;

        public List<Vacation> GetAllVacations()
        {
            return GetVacationsByIdOrAll();
        }

        public List<Vacation> GetUserVacations(User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            return GetVacationsByIdOrAll(user.Id);
        }

        private List<Vacation> GetVacationsByIdOrAll(int? id = null)
        {
            List<Vacation> vacations = new List<Vacation>();

            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                vacations = connection.Query(
                        "GetUserVacationHistory",
                        param: new { id },
                        commandType: CommandType.StoredProcedure)
                    .Select(row => new Vacation(
                        new User(row.Id, row.Name),
                        row.VacationFrom,
                        row.VacationTo))
                    .ToList();
            }

            return vacations;
        }

        public List<Vacation> GetVacationsForLastMonth()
        {
            List<Vacation> vacations = new List<Vacation>();

            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                vacations = connection.Query(
                        "GetVacationsForLastMonth",
                        commandType: CommandType.StoredProcedure)
                    .Select(row => new Vacation(
                        new User(row.Id, row.Name),
                        row.VacationFrom,
                        row.VacationTo))
                    .ToList();
            }

            return vacations;
        }

        public void AddTotalMonthVacations(List<TotalMonthVacations> totals, bool useBulkImport = false)
        {
            if (totals == null) throw new ArgumentNullException(nameof(totals));

            if (useBulkImport)
            {
                AddCollectionFromXml(XmlMapper.MapToXDocumentFrom(totals));
            }
            else
            {
                AddCollection(totals);
            }
        }

        public void Add(Vacation vacation)
        {
            Insert((connection, transaction) => {

                var parameters = GetParametersFor(vacation);

                connection.Execute("AddVacation", parameters, commandType: CommandType.StoredProcedure, transaction: transaction);

            });
        }

        public void AddCollection(List<Vacation> vacations)
        {
            Insert((connection, transaction) => {

                foreach(Vacation vacation in vacations)
                {
                    var parameters = GetParametersFor(vacation);

                    connection.Execute("AddVacation", parameters, commandType: CommandType.StoredProcedure, transaction: transaction);
                }

            });
        }

        private void AddCollection(List<TotalMonthVacations> totals)
        {
            Insert((connection, transaction) => {

                foreach(TotalMonthVacations total in totals)
                {
                    var parameters = GetParametersFor(total);

                    connection.Execute("ImportVacationScalar", parameters, commandType: CommandType.StoredProcedure, transaction: transaction);
                }

            });
        }

        private void AddCollectionFromXml(XDocument xdocument)
        {
            Insert((connection, transaction) => {
                connection.Execute("ImportFromXML", new { xmlParameter = xdocument }, commandType: CommandType.StoredProcedure, transaction: transaction);
            });
        }

        private DynamicParameters GetParametersFor(Vacation vacation)
        {
            if (vacation == null) throw new ArgumentNullException(nameof(vacation));

            var parameters = new DynamicParameters();

            parameters.Add("@id", vacation.User.Id);
            parameters.Add("@date_from", vacation.From);
            parameters.Add("@date_to", vacation.To);

            return parameters;
        }

        private DynamicParameters GetParametersFor(TotalMonthVacations total)
        {
            if (total == null) throw new ArgumentNullException(nameof(total));

            var parameters = new DynamicParameters();

            parameters.Add("@month_id", total.Month);
            parameters.Add("@user_id", total.UserId);
            parameters.Add("@days", total.QuantityBusinessDays);

            return parameters;
        }

        private void Insert(Action<IDbConnection, IDbTransaction> action)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    var transaction = connection.BeginTransaction();

                    try
                    {
                        action(connection, transaction);

                        transaction.Commit();
                    }
                    catch (SqlException sqlException)
                    {
                        transaction.Rollback();

                        if (sqlException.Number == DUPLICATE_KEY_ERROR_CODE)
                        {
                            throw new Exception("Last month's data has already been imported", sqlException.InnerException);
                        }

                        throw;
                    }
                }
            }
            catch
            {
                throw;
            }
        }
    }
}