using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Models.ViewModels
{
    public class AdminAddCmsViewModel
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public string? Title { get; set; }
        [Required]
        public string? Description { get; set; }
        [Required]
        public string Slug { get; set; } = null!;
        [Required]
        public string Status { get; set; } = null!;
    }
}
