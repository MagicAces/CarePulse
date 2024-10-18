using System.ComponentModel.DataAnnotations.Schema;
using backend.Enums;

namespace backend.Models
{
    [Table("Appointments")]
    public class Appointment
    {
        public int Id { get; set; }
        public string ReasonForAppointment { get; set; } = string.Empty;
        public string AdditionalComments { get; set; } = string.Empty;
        public string ReasonForCancellation { get; set; } = string.Empty;
        public AppointmentStatus Status { get; set; }
        public DateTime ExpectedAppointmentDate { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public string PatientId { get; set; }
        public string DoctorId { get; set; }
        
        public Patient Patient { get; set; }
        public Doctor Doctor { get; set; }
    }
}