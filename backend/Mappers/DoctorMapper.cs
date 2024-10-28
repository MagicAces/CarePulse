using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.DTOs.Doctor;
using backend.Models;

namespace backend.Mappers
{
    public static class DoctorMapper
    {
        public static DoctorDto ToDoctorDto(this Doctor doctor)
        {
            return new DoctorDto
            {
                UserId = doctor.UserId,
                Appointments = doctor.Appointments.Select(a => a.ToAppointmentDto()).ToList(),
            };
        }
    }
}