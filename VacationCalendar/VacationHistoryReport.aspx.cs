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
    public partial class VacationHistoryReport : System.Web.UI.Page
    {
        readonly VacationService vacationService = new VacationService(new VacationRepository());
        protected void Page_Load(object sender, EventArgs e)
        {            
            if (IsSuperUser() == false)
            {
                Response.Redirect("Default.aspx");
                return;
            }

            SetTextToImportVacationsButton();

            List<Vacation> vacations = vacationService.GetAllUsersVacations();

            FillVacationsGridView(vacations);
        }

        private bool IsSuperUser()
        {
            return true;
        }

        private void FillVacationsGridView(List<Vacation> vacations)
        {
            if (vacations == null) throw new ArgumentNullException(nameof(vacations));

            if (vacations.Count == 0)
            {
                ShowNoVacationsMessage();
                return;
            }

            List<VacationReportRecord> reports = vacations.Select(VacationMapper.MapToVacationReportRecord).ToList();

            vacationHistoryReport.DataSource = reports;
            vacationHistoryReport.DataBind();
        }

        protected void ImportVacations(object sender, EventArgs e)
        {
            ImportResult result = vacationService.AddTotalMonthVacations();

            if (result.IsSuccess)
            {
                ShowImportResultMessage("Vacations imported successfully", true);
            }
            else
            {
                ShowImportResultMessage(result.Message, false);
            }
        }

        private void ShowImportResultMessage(string message, bool isSuccess)
        {
            importResultMessageLabel.Text = message;
            
            if (isSuccess)
            {
                importResultMessageLabel.CssClass = "result-success";
                importResultMessageLabel.Visible = true;

                return;
            }
            
            importResultMessageLabel.CssClass = "result-error";
            importResultMessageLabel.Visible = true;
        }

        private void SetTextToImportVacationsButton()
        {
            DateTime date = DateTimeUtils.GetPreviousMonthDate();

            string previousMonthName = date.ToString("MMMM", CultureInfo.CreateSpecificCulture("us"));

            importVacationsButton.Text = $"Finalize & Import vacations for {previousMonthName}";
        }

        private void ShowNoVacationsMessage()
        {
            vacationHistoryReport.Visible = false;
            noVacationsMessage.Visible = true;
        }
    }
}