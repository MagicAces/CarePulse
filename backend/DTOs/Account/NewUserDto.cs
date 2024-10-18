using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.DTOs.Account
{
    public class NewUserDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Message { get; set; }
    }
}