using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.DTOs.Patient;
using backend.Models;

namespace backend.Mappers
{
    public static class PatientMapper
    {
        public static Patient ToPatientFromPatientProfileDto(this PatientProfileDto patientDto, string userId)
        {
            return new Patient
            {
                UserId = userId,
                DateOfBirth = patientDto.DateOfBirth,
                Gender = patientDto.Gender,
                Address = patientDto.Address,
                Occupation = patientDto.Occupation,
                EmergencyContactName = patientDto.EmergencyContactName,
                EmergencyPhoneNumber = patientDto.EmergencyPhoneNumber,
                PrimaryCarePhysicianId = patientDto.PrimaryCarePhysicianId,
                InsuranceProvider = patientDto.InsuranceProvider,
                InsurancePolicyNumber = patientDto.InsurancePolicyNumber,
                Allergies = patientDto.Allergies,
                CurrentMedications = patientDto.CurrentMedications,
                FamilyMedicalHistory = patientDto.FamilyMedicalHistory,
                PastMedicalHistory = patientDto.PastMedicalHistory,
                IdentificationType = patientDto.IdentificationType,
                IdentificationNumber = patientDto.IdentificationNumber,
                IdentificationDocumentURL = patientDto.IdentificationDocumentURL,
                DisclosureOfHealthInfo = patientDto.DisclosureOfHealthInfo
            };
        }
        
        public static PatientDto ToPatientDto(this Patient patient)
        {
            return new PatientDto
            {
                UserId = patient.UserId,
                DateOfBirth = patient.DateOfBirth,
                Gender = patient.Gender.ToString(),
                Address = patient.Address,
                Occupation = patient.Occupation,
                EmergencyContactName = patient.EmergencyContactName,
                EmergencyPhoneNumber = patient.EmergencyPhoneNumber,
                PrimaryCarePhysicianId = patient.PrimaryCarePhysicianId,
                InsuranceProvider = patient.InsuranceProvider,
                InsurancePolicyNumber = patient.InsurancePolicyNumber,
                Allergies = patient.Allergies,
                CurrentMedications = patient.CurrentMedications,
                FamilyMedicalHistory = patient.FamilyMedicalHistory,
                PastMedicalHistory = patient.PastMedicalHistory,
                IdentificationType = patient.IdentificationType,
                IdentificationNumber = patient.IdentificationNumber,
                IdentificationDocumentURL = patient.IdentificationDocumentURL,
                DisclosureOfHealthInfo = patient.DisclosureOfHealthInfo
            };
        }
    }
}