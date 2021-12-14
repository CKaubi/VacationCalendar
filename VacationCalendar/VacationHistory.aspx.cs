using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VacationCalendar.BL;
using VacationCalendar.Common;
using VacationCalendar.DL;
using VacationCalendar.Domain;
using VacationCalendar.Views;

namespace VacationCalendar
{
    public partial class VacationHistory : System.Web.UI.Page
    {
        readonly User _currentUser = new User(2, "John");
        readonly VacationService _vacationService = new VacationService(new VacationRepository());

        protected void Page_Load(object sender, EventArgs e)
        {
            List<Vacation> vacations = _vacationService.GetAllVacationsFor(_currentUser);

            FillVacationHistoryGridView(vacations);
        }

        private void UpdateVacationHistoryGridView()
        {
            List<Vacation> vacations = _vacationService.GetAllVacationsFor(_currentUser);

            FillVacationHistoryGridView(vacations);

            if (vacations.Count() != 0 && !vacationHistory.Visible)
            {
                ShowGridView();
            }
        }

        private void FillVacationHistoryGridView(List<Vacation> vacations)
        {
            if (vacations == null) throw new ArgumentNullException(nameof(vacations));

            if (vacations.Count == 0)
            {
                ShowNoVacationsMessage();
                return;
            }

            List<VacationReportRecord> reports = vacations.Select(VacationMapper.MapToVacationReportRecord).ToList();

            vacationHistory.DataSource = reports;
            vacationHistory.DataBind();
        }

        protected void FormSubmit(object sender, EventArgs e)
        {
            DateTime from;
            DateTime to;

            bool isValidFrom = ValidateTextIsDate(dateStartInput.Text, out from);
            bool isValidTo = ValidateTextIsDate(dateEndInput.Text, out to);

            if (!isValidFrom || !isValidTo)
            {
                ShowErrorMessage("Invalid date");
                return;
            }

            if (!IsDateRangeValid(from, to))
            {
                ShowErrorMessage("Date to less then date from");
                return;
            }

            ImportResult result = _vacationService.AddNewVacation(_currentUser, from, to);

            if (result.IsSuccess)
            {
                UpdateVacationHistoryGridView();
                ShowSuccessMessage();
                ClearTextBoxes();
            }
            else
            {
                ShowErrorMessage(result.Message);
            }
        }

        private bool ValidateTextIsDate(string stringDate, out DateTime parsedDate)
        {
            if (string.IsNullOrEmpty(stringDate)) throw new ArgumentNullException(nameof(stringDate));

            var format = "yyyyMdd";

            var isValid = DateTime.TryParseExact(stringDate, format,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out parsedDate);

            return isValid;
        }

        bool IsDateRangeValid(DateTime @from, DateTime to)
        {
            if (from == null) throw new ArgumentNullException(nameof(from));
            if (to == null) throw new ArgumentNullException(nameof(to));

            return from <= to;
        }

        private void ShowNoVacationsMessage()
        {
            vacationHistory.Visible = false;
            noVacationsMessage.Visible = true;
        }

        private void ShowGridView()
        {
            vacationHistory.Visible = true;
            noVacationsMessage.Visible = false;
        }

        private void ShowErrorMessage(string textMessage)
        {
            if (string.IsNullOrEmpty(textMessage)) throw new ArgumentNullException(nameof(textMessage));

            successMessageLabel.Visible = false;

            submitResultMessage.Visible = true;
            errorMessageLabel.Visible = true;
            errorMessageLabel.Text = textMessage;
        }

        private void ShowSuccessMessage()
        {
            errorMessageLabel.Visible = false;

            submitResultMessage.Visible = true;
            successMessageLabel.Visible = true;
        }

        private void ClearTextBoxes()
        {
            dateEndInput.Text = "";
            dateStartInput.Text = "";
        }
    }
}