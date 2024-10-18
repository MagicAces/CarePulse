using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.DTOs.Error
{
    public class ErrorResponse
    {
        public string Message { get; set; }
        public List<string> Errors { get; set; } = [];
    }
}