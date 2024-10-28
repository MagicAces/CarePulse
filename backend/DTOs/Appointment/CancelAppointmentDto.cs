using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace backend.DTOs.Appointment
{
    public class CancelAppointmentDto
    {
        [Required(ErrorMessage = "Reason for cancellation is required.")]
        [StringLength(500, ErrorMessage = "The cancellation reason cannot exceed 500 characters.")]
        public string ReasonForCancellation { get; set; }
    }
}