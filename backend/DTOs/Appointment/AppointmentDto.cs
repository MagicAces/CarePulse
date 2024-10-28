using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.DTOs.Patient;
using backend.Enums;

namespace backend.DTOs.Appointment
{
    public class AppointmentDto
    {
        public int Id { get; set; }
        public string ReasonForAppointment { get; set; } = string.Empty;
        public string AdditionalComments { get; set; } = string.Empty;
        public string ReasonForCancellation { get; set; } = string.Empty;
        public string Status { get; set; }
        public DateTime ExpectedAppointmentDate { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public string PatientId { get; set; }
        public string DoctorId { get; set; }

        public PatientDto Patient { get; set; }
    }
}