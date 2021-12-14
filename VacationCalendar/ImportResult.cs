using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VacationCalendar
{
    public class ImportResult
    {
        public bool IsSuccess { get; }
        public string Message { get; }

        public ImportResult(bool isSuccess, string message)
        {
            IsSuccess = isSuccess;
            Message = message;
        }
    }
}