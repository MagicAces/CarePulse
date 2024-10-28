using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Enums;

namespace backend.DTOs.Patient
{
    public class PatientDto
    {
        public string UserId { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
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
    }
}