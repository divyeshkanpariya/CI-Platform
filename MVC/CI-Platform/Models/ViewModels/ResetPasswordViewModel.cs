﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Models.ViewModels
{
    public class ResetPasswordViewModel
    {
        [Required]
        public string Email { get; set; } = null!;
        [Required]
        public string Token { get; set; } = null!;
        [Required]
        [MinLength(8,ErrorMessage = "Minimum length of Password should be 8")]
        public string Password { get; set; } = null!;
        [Required]
        [MinLength(8, ErrorMessage = "Minimum length of Password should be 8")]
        [Compare("Password", ErrorMessage = "Password and Confirm-Password are mot same !!")]
        public string ConfirmPassword { get; set; } = null!;
    }
}
