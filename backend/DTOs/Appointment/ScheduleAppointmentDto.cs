
using System.ComponentModel.DataAnnotations;
using backend.Validation.Attributes;

namespace backend.DTOs.Appointment
{
    public class ScheduleAppointmentDto
    {
        [Required(ErrorMessage = "Doctor ID is required.")]
        public string DoctorId { get; set; }

        [Required(ErrorMessage = "Expected appointment date is required.")]
        [DataType(DataType.DateTime, ErrorMessage = "Invalid date format.")]
        [FutureDate(ErrorMessage = "Expected appointment date must be in the future.")]
        public DateTime ExpectedAppointmentDate { get; set; }
    }
}