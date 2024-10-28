using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Data;
using backend.DTOs.Appointment;
using backend.Enums;
using backend.Helpers;
using backend.Interfaces;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Repository
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly ApplicationDBContext _context;

        public AppointmentRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Appointment?> CancelAsync(int id, CancelAppointmentDto appointmentDto)
        {
            var appointment = await _context.Appointments.Include(a => a.Doctor).ThenInclude(d => d.User).Include(a => a.Patient).ThenInclude(p => p.User).FirstOrDefaultAsync(a => a.Id == id);
            if (appointment == null)
                return null;

            appointment.ReasonForCancellation = appointmentDto.ReasonForCancellation;
            appointment.Status = AppointmentStatus.Cancelled;
            await _context.SaveChangesAsync();
            return appointment;
        }

        public async Task<Appointment> CreateAsync(Appointment appointment)
        {
            await _context.Appointments.AddAsync(appointment);
            await _context.SaveChangesAsync();

            await _context.Entry(appointment).Reference(a => a.Doctor).LoadAsync();
            await _context.Entry(appointment).Reference(a => a.Patient).LoadAsync();
            await _context.Entry(appointment.Doctor).Reference(d => d.User).LoadAsync();
            await _context.Entry(appointment.Patient).Reference(p => p.User).LoadAsync();

            return appointment;
        }

        public async Task<(List<Appointment> Appointments, int PendingCount, int ScheduledCount, int CanceledCount)> GetAllAsync(AppointmentQueryObject query)
        {
            var appointments = _context.Appointments
            .Include(a => a.Patient)
            .Include(a => a.Doctor)
            .AsQueryable();

            if (query.Status.HasValue)
            {
                appointments = appointments.Where(a => a.Status == query.Status.Value);
            }

            if (!string.IsNullOrWhiteSpace(query.SortBy))
            {
                appointments = query.SortBy.Equals("CreatedOn", StringComparison.OrdinalIgnoreCase)
                    ? (query.IsDescending ? appointments.OrderByDescending(a => a.CreatedOn) : appointments.OrderBy(a => a.CreatedOn))
                    : (query.IsDescending ? appointments.OrderByDescending(a => a.UpdatedAt) : appointments.OrderBy(a => a.UpdatedAt));
            }

            var skipNumber = (query.PageNumber - 1) * query.PageSize;
            var pagedAppointments = await appointments.Skip(skipNumber).Take(query.PageSize).ToListAsync();

            int pendingCount = await appointments.CountAsync(a => a.Status == AppointmentStatus.Pending);
            int scheduledCount = await appointments.CountAsync(a => a.Status == AppointmentStatus.Scheduled);
            int canceledCount = await appointments.CountAsync(a => a.Status == AppointmentStatus.Cancelled);

            return (pagedAppointments, pendingCount, scheduledCount, canceledCount);
        }

        public async Task<List<Appointment>> GetAllForUserAsync(string userId)
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Where(a => a.DoctorId == userId || a.PatientId == userId)
                .ToListAsync();
        }

        public async Task<(List<Appointment> Appointments, int PendingCount, int ScheduledCount, int CanceledCount)> GetAllForUserAsync(string userId, AppointmentQueryObject query)
        {
            var appointments = _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Where(a => a.DoctorId == userId || a.PatientId == userId)
                .AsQueryable();

            if (query.Status.HasValue)
            {
                appointments = appointments.Where(a => a.Status == query.Status.Value);
            }

            if (!string.IsNullOrWhiteSpace(query.SortBy))
            {
                appointments = query.SortBy.Equals("CreatedOn", StringComparison.OrdinalIgnoreCase)
                    ? (query.IsDescending ? appointments.OrderByDescending(a => a.CreatedOn) : appointments.OrderBy(a => a.CreatedOn))
                    : (query.IsDescending ? appointments.OrderByDescending(a => a.UpdatedAt) : appointments.OrderBy(a => a.UpdatedAt));
            }

            var skipNumber = (query.PageNumber - 1) * query.PageSize;
            var pagedAppointments = await appointments.Skip(skipNumber).Take(query.PageSize).ToListAsync();


            int pendingCount = await appointments.CountAsync(a => (a.DoctorId == userId || a.PatientId == userId) && a.Status == AppointmentStatus.Pending);
            int scheduledCount = await appointments.CountAsync(a => (a.DoctorId == userId || a.PatientId == userId) && a.Status == AppointmentStatus.Scheduled);
            int canceledCount = await appointments.CountAsync(a => (a.DoctorId == userId || a.PatientId == userId) && a.Status == AppointmentStatus.Cancelled);

            return (pagedAppointments, pendingCount, scheduledCount, canceledCount);
        }

        public async Task<Appointment?> GetByIdAsync(int id)
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Appointment?> ScheduleAsync(int id, ScheduleAppointmentDto appointmentDto)
        {
            var appointment = await _context.Appointments.Include(a => a.Doctor).ThenInclude(d => d.User).Include(a => a.Patient).ThenInclude(p => p.User).FirstOrDefaultAsync(a => a.Id == id);
            if (appointment == null)
                return null;

            appointment.DoctorId = appointmentDto.DoctorId;
            appointment.ExpectedAppointmentDate = appointmentDto.ExpectedAppointmentDate;
            appointment.Status = AppointmentStatus.Scheduled;
            await _context.SaveChangesAsync();
            return appointment;
        }
    }
}