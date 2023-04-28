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
        public IFormFile? ProfileImage { get; set; }

        [Required]
        [MaxLength(16)]
        public string? Name { get; set; }
        [Required]
        [MaxLength(16)]
        public string? Surname { get; set; }
        [MaxLength(16)]
        public string? EmployeeId { get; set; }

        public string? Manager { get; set; }

        [MaxLength(255)]
        
        public string? Title { get; set; }

        [MaxLength(16)]
        public string? Department { get; set; }

        [Required]
        public string? MyProfileText { get; set; }

        public string? WhyIVol { get; set; }

        public string? City { get; set; }

        [Required]
        public string? Country { get; set; }

        public string? Availability { get; set; }

        [MaxLength(255)]
        [Url]
        public string? LinkedinURL { get; set; }

        public List<string>? UserSkillIdList { get; set; }

        public List<string>? UserSkills { get; set; }
        [Required]
        [MinLength(1)]
        public string? MySkills { get; set; }
    }
}
