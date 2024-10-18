using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Helpers
{
    public class ConfirmObject
    {
        [Required(ErrorMessage = "User Id is needed")]
        public required string UserId { get; set; }

        [Required(ErrorMessage = "Token is required")]
        public required string Token { get; set; }
    }
}