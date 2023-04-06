using CI_Platform.Models.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Models.ViewModels
{
    public class UserProfileViewModel
    {
        public IFormFile ProfileImage { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        
        public string EmployeeId { get; set; }

        public string Manager { get; set; }
        
        public string Title { get; set; }

        public string Department { get; set; }
        [Required]
        public string MyProfileText { get; set; }

        public string WhyIVol { get; set; }

        public string City { get; set; }
        [Required]
        public string Country { get; set; }

        public string Availability { get; set; }

        public string LinkedinURL { get; set; }

        public List<string> UserSkills { get; set; }
        
        public string MySkills { get; set; }
    }
}
