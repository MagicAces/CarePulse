using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Models
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public DateTime Expires { get; set; }
        public bool IsRevoked { get; set; } = false;
        public bool IsUsed { get; set; } = false;
        public string UserId { get; set; }
        public User User { get; set; }
    }

}