
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    [Table("Doctors")]
    public class Doctor
    {
        public string UserId { get; set; }
        public List<Appointment> Appointments { get; set; } = new List<Appointment>();

        public User User { get; set; }
    }
}