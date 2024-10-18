using Microsoft.AspNetCore.Identity;

namespace backend.Models
{
    public class User: IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}