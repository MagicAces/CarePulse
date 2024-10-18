using System.ComponentModel.DataAnnotations;
using backend.Enums;

namespace backend.DTOs
{
    public class RegisterDto
    {
        [Required]
        [MinLength(2, ErrorMessage = "Full name must be 2 characters or more")]
        public string FullName { get; set; } = string.Empty;
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        [Phone]
        [RegularExpression(@"^\+\d{1,14}$", ErrorMessage = "Phone number must start with +.")]
        public string? PhoneNumber { get; set; }

        [EnumDataType(typeof(UserRole), ErrorMessage = "Role must be either Admin or Doctor.")]
        public UserRole Role { get; set; } = UserRole.Admin;
    }
}