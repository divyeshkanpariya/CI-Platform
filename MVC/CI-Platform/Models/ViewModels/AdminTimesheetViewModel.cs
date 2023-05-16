using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Models.ViewModels
{
    public class AdminTimesheetViewModel
    {
        public long TimesheetId { get; set; }

        public string? UserName { get; set; }

        public string? Mission { get; set; }

        public TimeSpan? Time { get; set; }

        public int? Action { get; set; }

        public DateTime DateVolunteered { get; set; }

        public int TotalTimesheets { get; set; }

    }
}
