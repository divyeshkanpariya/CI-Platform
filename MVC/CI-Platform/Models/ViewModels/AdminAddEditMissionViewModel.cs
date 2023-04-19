using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Models.ViewModels
{
    public class AdminAddEditMissionViewModel
    {
        public long MissionId { get; set; }

        [Required]
        public string? ThemeId { get; set; }

        public string? CityId { get; set; }

        public string? CountryId { get; set; }

        public string Title { get; set; } = null!;

        public string? ShortDescription { get; set; }

        public string? Description { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public DateTime? RegistrationDeadline { get; set; }

        public string MissionType { get; set; } = null!;

        public string Status { get; set; } = null!;

        public string? OrganizationName { get; set; }

        public string? OrganizationDetail { get; set; }

        public string? Availability { get; set; }

        public string? Skills { get; set; }

        public IFormFile? DefaultImage { get; set; }

        public int TotalSeats { get; set; }

        public int GoalValue { get; set; }

        public string GoalObjectiveText { get; set; }

        public string? VideoURLs { get; set; }
        
        public IFormFileCollection? Images { get; set; }

        public IFormFileCollection? Documents { get; set; }
    }
}
