using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace backend.DTOs.Account
{
    public class ResendEmailDto
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }
    }
}