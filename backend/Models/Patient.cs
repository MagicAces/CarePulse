using System.ComponentModel.DataAnnotations.Schema;
using backend.Enums;

namespace backend.Models
{
    [Table("Patients")]
    public class Patient
    {
        public string UserId { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public string Address { get; set; }
        public string Occupation { get; set; }
        public string EmergencyContactName { get; set; }
        public string EmergencyPhoneNumber { get; set; }
        public string PrimaryCarePhysicianId { get; set; }
        public string InsuranceProvider { get; set; }
        public string InsurancePolicyNumber { get; set; }
        public string Allergies { get; set; }
        public string CurrentMedications { get; set; }
        public string FamilyMedicalHistory { get; set; }
        public string PastMedicalHistory { get; set; }
        public string IdentificationType { get; set; }
        public string IdentificationNumber { get; set; }
        public string IdentificationDocumentURL { get; set; }
        public bool DisclosureOfHealthInfo { get; set; }
        public User User { get; set; }
        public Doctor PrimaryCarePhysician { get; set; }
    }
}
