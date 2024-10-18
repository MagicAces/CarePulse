using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.DTOs.Account
{
    public class ConfirmEmailDto : NewUserDto
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public string Role { get; set; }
        public string SMSMessage { get; set; }
        public string Token { get; set; }
        public bool SMSSent { get; set; }
    }
}