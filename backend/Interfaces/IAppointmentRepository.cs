using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.DTOs.Appointment;
using backend.Helpers;
using backend.Models;

namespace backend.Interfaces
{
    public interface IAppointmentRepository
    {
        Task<(List<Appointment> Appointments, int PendingCount, int ScheduledCount, int CanceledCount)> GetAllForUserAsync(string userId, AppointmentQueryObject query);
        Task<Appointment?> GetByIdAsync(int id);
        Task<(List<Appointment> Appointments, int PendingCount, int ScheduledCount, int CanceledCount)> GetAllAsync(AppointmentQueryObject query);
        Task<Appointment> CreateAsync(Appointment appointment);
        Task<Appointment?> ScheduleAsync(int id, ScheduleAppointmentDto appointmentDto);
        Task<Appointment?> CancelAsync(int id, CancelAppointmentDto appointmentDto);
    }
}