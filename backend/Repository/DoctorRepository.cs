using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Data;
using backend.Helpers;
using backend.Interfaces;
using backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace backend.Repository
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly ApplicationDBContext _context;
        private readonly UserManager<User> _userManager;

        public DoctorRepository(ApplicationDBContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<Doctor> CreateAsync(Doctor doctorModel)
        {
            await _context.Doctors.AddAsync(doctorModel);
            await _context.SaveChangesAsync();
            return doctorModel;
        }

        public async Task<List<Doctor>> GetAllAsync(DoctorQueryObject query)
        {
            var doctors = _context.Doctors.Include(d => d.Appointments).ThenInclude(a => a.Patient).AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.SortBy))
            {
                if (query.SortBy.Equals("Appointments", StringComparison.OrdinalIgnoreCase))
                {
                    doctors = query.IsDescending ? doctors.OrderByDescending(d => d.Appointments.Count) : doctors.OrderBy(d => d.Appointments.Count);
                }
            }

            var skipNumber = (query.PageNumber - 1) * query.PageSize;
            return await doctors.Skip(skipNumber).Take(query.PageSize).ToListAsync();
        }

        public async Task<Doctor?> GetByIdAsync(string userId)
        {
            var doctor = await _context.Doctors.Include(d => d.User).Include(d => d.Appointments).FirstOrDefaultAsync(d => d.UserId == userId);
            if (doctor == null)
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user != null && (await _userManager.GetRolesAsync(user)).FirstOrDefault() == "Doctor")
                {
                    var newDoctor = new Doctor
                    {
                        UserId = user.Id,
                        Appointments = []
                    };
                    await _context.Doctors.AddAsync(newDoctor);
                    await _context.SaveChangesAsync();
                    return newDoctor;
                }
                return null;
            }
            return doctor;
        }


    }
}