using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Models.ViewModels
{
    public class AdminUserTableViewModel
    {
        public long UserId { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string Email { get; set; }

        public string? EmployeeId { get; set; }

        public string? Department { get; set; }

        public string Status { get; set; } = null!;

        public int TotalUsers { get; set; }

    }
}
