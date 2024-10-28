using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.DTOs.Appointment;

namespace backend.DTOs.Doctor
{
    public class DoctorDto
    {
        public string UserId { get; set; }
        public List<AppointmentDto> Appointments { get; set; }
    }
}