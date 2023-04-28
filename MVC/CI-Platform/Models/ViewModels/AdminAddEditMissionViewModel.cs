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
        [Required]
        public long MissionId { get; set; }

        [Required]
        public string? ThemeId { get; set; }
        [Required]
        public string? CityId { get; set; }
        [Required]
        public string? CountryId { get; set; }
        [Required]
        [MaxLength(128)]
        public string Title { get; set; } = null!;
        [Required]
        [MinLength(1)]
        public string? ShortDescription { get; set; }
        [Required]
        [MinLength(1)]
        public string? Description { get; set; }
        [Required]
        
        public DateTime? StartDate { get; set; }
        [Required]
        
        public DateTime? EndDate { get; set; }

        public DateTime? RegistrationDeadline { get; set; }
        [Required]
        public string MissionType { get; set; } = null!;
        [Required]
        public string Status { get; set; } = null!;
        [Required]
        [MaxLength(255)]
        public string? OrganizationName { get; set; }
        [Required]
        public string? OrganizationDetail { get; set; }
        [Required]
        public string? Availability { get; set; }
        [Required]
        public string[]? Skills { get; set; }
        [Required]
        public IFormFile? DefaultImage { get; set; }

        public int? TotalSeats { get; set; }
        
        public int GoalValue { get; set; }

        public string? GoalObjectiveText { get; set; }

        public string? VideoURLs { get; set; }
        
        public IFormFileCollection? Images { get; set; }

        public IFormFileCollection? Documents { get; set; }
    }
}
