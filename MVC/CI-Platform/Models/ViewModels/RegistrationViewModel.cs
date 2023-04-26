using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Models.ViewModels
{
    public class RegistrationViewModel
    {
        [Required]
        [MaxLength(16)]
        public string? FirstName { get; set; }

        [Required]
        [MaxLength(16)]
        public string? LastName { get; set; }

        [Required]
        public long PhoneNumber { get; set; }

        [Required]
        [MaxLength(128)]
        public string Email { get; set; } = null!;

        [Required]
        [MinLength(8, ErrorMessage = "Minimum length of Password should be 8")]
        [MaxLength(255)]
        public string Password { get; set; } = null!;

        [Required]
        [MinLength(8, ErrorMessage = "Minimum length of Password should be 8")]
        [MaxLength(255)]
        [Compare("Password", ErrorMessage ="Password and Confirm Password are not Same")]
        public string ConfirmPassword { get; set; } = null!;

    }
}
