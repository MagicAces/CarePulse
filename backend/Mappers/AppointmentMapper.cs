using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.DTOs.Appointment;
using backend.Models;

namespace backend.Mappers
{
    public static class AppointmentMapper
    {
        public static Appointment ToAppointmentFromNewAppointmentDTO(this NewAppointmentDto appointmentDto, string patientId)
        {
            return new Appointment
            {
                ReasonForAppointment = appointmentDto.ReasonForAppointment,
                AdditionalComments = appointmentDto.AdditionalComments,
                DoctorId = appointmentDto.DoctorId,
                ExpectedAppointmentDate = appointmentDto.ExpectedAppointmentDate,
                Status = Enums.AppointmentStatus.Pending,
                PatientId = patientId,
                ReasonForCancellation = null
            };
        }
        public static AppointmentDto ToAppointmentDto(this Appointment appointment)
        {
            return new AppointmentDto
            {
                Id = appointment.Id,
                ReasonForAppointment = appointment.ReasonForAppointment,
                ReasonForCancellation = appointment.ReasonForCancellation,
                Status = appointment.Status.ToString(),
                ExpectedAppointmentDate = appointment.ExpectedAppointmentDate,
                CreatedOn = appointment.CreatedOn,
                UpdatedAt = appointment.UpdatedAt,
                PatientId = appointment.PatientId,
                DoctorId = appointment.DoctorId,
                Patient = appointment.Patient.ToPatientDto()
            };
        }
    }
}