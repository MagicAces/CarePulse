using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Data;
using backend.DTOs.Patient;
using backend.Interfaces;
using backend.Mappers;
using backend.Models;

namespace backend.Repository
{
    public class PatientRepository : IPatientRepository
    {
        private readonly ApplicationDBContext _context;
        private readonly IDoctorRepository _doctorRepo;
        public PatientRepository(ApplicationDBContext context, IDoctorRepository doctorRepo)
        {
            _context = context;
            _doctorRepo = doctorRepo;
        }

        public async Task<Patient?> GetByIdAsync(string userId)
        {
            return await _context.Patients.FindAsync(userId);
        }

        public async Task<Patient?> UpdateAsync(string userId, PatientProfileDto patientDto)
        {
            if (!string.IsNullOrEmpty(patientDto.PrimaryCarePhysicianId))
            {
                var primaryCarePhysician = await _doctorRepo.GetByIdAsync(patientDto.PrimaryCarePhysicianId);
                if (primaryCarePhysician == null)
                    return null;
            }

            var existingPatient = await _context.Patients.FindAsync(userId);

            if (existingPatient == null)
            {
                var newPatient = patientDto.ToPatientFromPatientProfileDto(userId);
                await _context.Patients.AddAsync(newPatient);
                await _context.SaveChangesAsync();
                return newPatient;
            }

            existingPatient.DateOfBirth = patientDto.DateOfBirth;
            existingPatient.Gender = patientDto.Gender;
            existingPatient.Address = patientDto.Address;
            existingPatient.Occupation = patientDto.Occupation;
            existingPatient.EmergencyContactName = patientDto.EmergencyContactName;
            existingPatient.EmergencyPhoneNumber = patientDto.EmergencyPhoneNumber;
            existingPatient.PrimaryCarePhysicianId = patientDto.PrimaryCarePhysicianId;
            existingPatient.InsuranceProvider = patientDto.InsuranceProvider;
            existingPatient.InsurancePolicyNumber = patientDto.InsurancePolicyNumber;
            existingPatient.Allergies = patientDto.Allergies;
            existingPatient.CurrentMedications = patientDto.CurrentMedications;
            existingPatient.FamilyMedicalHistory = patientDto.FamilyMedicalHistory;
            existingPatient.PastMedicalHistory = patientDto.PastMedicalHistory;
            existingPatient.IdentificationType = patientDto.IdentificationType;
            existingPatient.IdentificationNumber = patientDto.IdentificationNumber;
            existingPatient.IdentificationDocumentURL = patientDto.IdentificationDocumentURL;
            existingPatient.DisclosureOfHealthInfo = patientDto.DisclosureOfHealthInfo;

            await _context.SaveChangesAsync();
            return existingPatient;
        }

    }
}