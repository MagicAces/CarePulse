using System;
using System.ComponentModel.DataAnnotations;
using backend.Enums;

namespace backend.DTOs.Patient
{
    public class PatientProfileDto
    {
        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public Gender Gender { get; set; }

        [Required]
        [MinLength(3, ErrorMessage = "Address must be at least 3 characters.")]
        public required string Address { get; set; }

        [Required]
        [MinLength(3, ErrorMessage = "Occupation must be at least 3 characters.")]
        public required string Occupation { get; set; }

        [Required]
        [MinLength(3, ErrorMessage = "Emergency contact name must be at least 3 characters.")]
        public required string EmergencyContactName { get; set; }

        [Required]
        [Phone]
        [RegularExpression(@"^\+\d{1,14}$", ErrorMessage = "Phone number must start with + and contain up to 14 digits.")]
        public required string EmergencyPhoneNumber { get; set; }

        [Required]
        [MinLength(3, ErrorMessage = "Primary care physician ID must be at least 3 characters.")]
        public required string PrimaryCarePhysicianId { get; set; }

        [Required]
        [MinLength(3, ErrorMessage = "Insurance provider must be at least 3 characters.")]
        public required string InsuranceProvider { get; set; }

        [Required]
        [MinLength(3, ErrorMessage = "Insurance policy number must be at least 3 characters.")]
        public required string InsurancePolicyNumber { get; set; }

        [Required]
        [MinLength(3, ErrorMessage = "Allergies must be at least 3 characters.")]
        public required string Allergies { get; set; }

        [Required]
        [MinLength(3, ErrorMessage = "Current medications must be at least 3 characters.")]
        public required string CurrentMedications { get; set; }

        [Required]
        [MinLength(3, ErrorMessage = "Family medical history must be at least 3 characters.")]
        public required string FamilyMedicalHistory { get; set; }

        [Required]
        [MinLength(3, ErrorMessage = "Past medical history must be at least 3 characters.")]
        public required string PastMedicalHistory { get; set; }

        [Required]
        [MinLength(3, ErrorMessage = "Identification type must be at least 3 characters.")]
        public required string IdentificationType { get; set; }

        [Required]
        [MinLength(3, ErrorMessage = "Identification number must be at least 3 characters.")]
        public required string IdentificationNumber { get; set; }

        [Required]
        [Url(ErrorMessage = "Invalid URL format for identification document.")]
        public required string IdentificationDocumentURL { get; set; }

        [Required]
        public bool DisclosureOfHealthInfo { get; set; } = true;
    }
}
