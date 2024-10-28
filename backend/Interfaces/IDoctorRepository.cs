using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Helpers;
using backend.Models;

namespace backend.Interfaces
{
    public interface IDoctorRepository
    {
        Task<Doctor?> GetByIdAsync(string userId);
        Task<List<Doctor>> GetAllAsync(DoctorQueryObject query);
        Task<Doctor> CreateAsync(Doctor doctorModel);
    }
}