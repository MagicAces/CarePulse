using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace backend.DTOs.Account
{
    public class LoginDto
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        [Phone]
        [RegularExpression(@"^\+\d{1,14}$", ErrorMessage = "Phone number must start with +.")]
        public string? PhoneNumber { get; set; }
    }
}