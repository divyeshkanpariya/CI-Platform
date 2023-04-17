using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Models.ViewModels
{
    public class AdminAddUserViewModel
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public string Email { get; set; }

        
        public string EmployeeId { get; set; }

        public string Department { get; set; }

        public string ProfileText { get; set; }

        public string City { get; set; }
        [Required]
        public string Country { get; set; }

        public char Status { get; set; }

    }
}
