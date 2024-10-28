using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.DTOs.Patient;
using backend.Models;

namespace backend.Interfaces
{
    public interface IPatientRepository
    {
        Task<Patient?> GetByIdAsync(string userId);
        Task<Patient?> UpdateAsync(string userId, PatientProfileDto patientDto);
    }
}