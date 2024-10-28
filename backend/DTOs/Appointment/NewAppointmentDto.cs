using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.ComponentModel.DataAnnotations;
using backend.Validation.Attributes;

namespace backend.DTOs.Appointment
{

    public class NewAppointmentDto
    {
        [Required(ErrorMessage = "Doctor ID is required.")]
        public string DoctorId { get; set; }

        [Required(ErrorMessage = "Reason for the appointment is required.")]
        [StringLength(250, ErrorMessage = "The reason cannot exceed 250 characters.")]
        public string ReasonForAppointment { get; set; }

        [StringLength(500, ErrorMessage = "Additional comments cannot exceed 500 characters.")]
        public string AdditionalComments { get; set; } = string.Empty;

        [Required(ErrorMessage = "Expected appointment date is required.")]
        [DataType(DataType.DateTime, ErrorMessage = "Invalid date format.")]
        [FutureDate(ErrorMessage = "Expected appointment date must be in the future.")]
        public DateTime ExpectedAppointmentDate { get; set; }
    }

}