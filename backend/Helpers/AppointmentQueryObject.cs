using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Enums;

namespace backend.Helpers
{
    public class AppointmentQueryObject
    {
        public AppointmentStatus? Status { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string SortBy { get; set; } = "CreatedOn";
        public bool IsDescending { get; set; } = true;
    }

}