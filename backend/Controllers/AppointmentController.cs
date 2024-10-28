using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.DTOs.Appointment;
using backend.Extensions;
using backend.Helpers;
using backend.Interfaces;
using backend.Mappers;
using backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/appointment")]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentRepository _appointmentRepo;
        private readonly IDoctorRepository _doctorRepo;
        private readonly IEmailService _emailService;

        public AppointmentController(IAppointmentRepository appointmentRepo, IDoctorRepository doctorRepo, IEmailService emailService)
        {
            _appointmentRepo = appointmentRepo;
            _doctorRepo = doctorRepo;
            _emailService = emailService;
        }

        private async Task SendAppointmentEmail(string email, string subject, string message)
        {
            await _emailService.SendEmailAsync(new SendEmailObject
            {
                ToEmail = email,
                Subject = subject,
                Body = message
            });
        }

        [HttpGet("my")]
        [Authorize(Roles = "Doctor, Patient")]
        public async Task<IActionResult> GetMyAppointments([FromQuery] AppointmentQueryObject query)
        {
            string userId = User.GetUserId();
            var (appointments, pendingCount, scheduledCount, canceledCount) = await _appointmentRepo.GetAllForUserAsync(userId, query);

            var response = new
            {
                Appointments = appointments.Select(a => a.ToAppointmentDto()).ToList(),
                Counts = new
                {
                    Pending = pendingCount,
                    Scheduled = scheduledCount,
                    Canceled = canceledCount
                }
            };

            return Ok(response);
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = "Doctor, Patient")]
        public async Task<IActionResult> GetAppointmentById([FromRoute] int id)
        {
            var appointment = await _appointmentRepo.GetByIdAsync(id);
            if (appointment == null)
                return NotFound("Appointment not found");

            return Ok(appointment);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllAppointments([FromQuery] AppointmentQueryObject query)
        {
            var (appointments, pendingCount, scheduledCount, canceledCount) = await _appointmentRepo.GetAllAsync(query);

            var response = new
            {
                Appointments = appointments.Select(a => a.ToAppointmentDto()).ToList(),
                Counts = new
                {
                    Pending = pendingCount,
                    Scheduled = scheduledCount,
                    Canceled = canceledCount
                }
            };

            return Ok(response);
        }

        [HttpPost]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> RequestAppointment([FromBody] NewAppointmentDto appointmentDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var doctor = await _doctorRepo.GetByIdAsync(appointmentDto.DoctorId);
            if (doctor == null)
                return NotFound("Doctor not found");

            if (doctor.User == null || string.IsNullOrEmpty(doctor.User.FullName))
                return BadRequest("Doctor's profile is incomplete.");

            string userId = User.GetUserId();
            string email = User.GetUserEmail();
            string fullname = User.GetFullName();

            var newAppointment = appointmentDto.ToAppointmentFromNewAppointmentDTO(userId);
            await _appointmentRepo.CreateAsync(newAppointment);

            string subject = "Appointment Requested: Confirmation";
            string message = $@"
                <p>Dear {fullname},</p>
                <p>Your appointment request has been sent to an admin for review. Here are the details of your appointment:</p>
                <ul>
                    <li><strong>Appointment ID:</strong> #{newAppointment.Id}</li>
                    <li><strong>Doctor:</strong> {doctor.User.FullName}</li>
                    <li><strong>Reason for Appointment:</strong> {newAppointment.ReasonForAppointment}</li>
                    <li><strong>Additional Comments:</strong> {newAppointment.AdditionalComments}</li>
                    <li><strong>Expected Appointment Date:</strong> {newAppointment.ExpectedAppointmentDate:MMMM dd, yyyy}</li>
                </ul>
                <p>Expect to receive an update on the status of your appointment soon.</p>
                <p>Best regards,</p>
                <p>CarePulse</p>";

            await SendAppointmentEmail(email, subject, message);

            return CreatedAtAction(nameof(GetAppointmentById), new { id = newAppointment.Id }, newAppointment.ToAppointmentDto());
        }


        [HttpPut("{id:int}/schedule")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ScheduleAppointment([FromRoute] int id, [FromBody] ScheduleAppointmentDto appointmentDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var doctor = await _doctorRepo.GetByIdAsync(appointmentDto.DoctorId);
            if (doctor == null)
                return NotFound("Doctor not found");

            var existingAppointment = await _appointmentRepo.GetByIdAsync(id);
            if(existingAppointment == null) 
                return NotFound("Appointment not found");
            if (existingAppointment.Status != Enums.AppointmentStatus.Pending)
                return BadRequest("Appointment has already been updated");
             
            var appointment = await _appointmentRepo.ScheduleAsync(id, appointmentDto);

            string patientEmail = appointment.Patient.User.Email;
            string patientFullname = appointment.Patient.User.FullName;

            string subject = "Your Appointment Has Been Scheduled!";
            string message = $@"
                <p>Dear {patientFullname},</p>
                <p>We are pleased to inform you that your appointment has been scheduled. Here are the updated details:</p>
                <ul>
                    <li><strong>Appointment ID:</strong> #{appointment.Id}</li>
                    <li><strong>Doctor:</strong> {doctor.User.FullName}</li>
                    <li><strong>Reason for Appointment:</strong> {appointment.ReasonForAppointment}</li>
                    <li><strong>Additional Comments:</strong> {appointment.AdditionalComments}</li>
                    <li><strong>Scheduled Appointment Date:</strong> {appointment.ExpectedAppointmentDate:MMMM dd, yyyy}</li>
                </ul>
                <p>We look forward to seeing you on the scheduled date. Please feel free to reach out if you have any questions.</p>
                <p>Best regards,</p>
                <p>CarePulse</p>";

            await SendAppointmentEmail(patientEmail, subject, message);

            string doctorEmail = appointment.Doctor.User.Email;
            string doctorFullname = appointment.Doctor.User.FullName;

            string doctorSubject = "New Appointment Scheduled with Patient";
            string doctorMessage = $@"
                <p>Dear Dr. {doctorFullname},</p>
                <p>A new appointment has been scheduled. Here are the appointment details:</p>
                <ul>
                    <li><strong>Appointment ID:</strong> #{appointment.Id}</li>
                    <li><strong>Patient Name:</strong> {patientFullname}</li>
                    <li><strong>Patient Email:</strong> {patientEmail}</li>
                    <li><strong>Reason for Appointment:</strong> {appointment.ReasonForAppointment}</li>
                    <li><strong>Additional Comments:</strong> {appointment.AdditionalComments}</li>
                    <li><strong>Scheduled Appointment Date:</strong> {appointment.ExpectedAppointmentDate:MMMM dd, yyyy}</li>
                </ul>
                <p>Please prepare accordingly for this appointment. Feel free to reach out if you have any questions.</p>
                <p>Best regards,</p>
                <p>CarePulse Team</p>";

            await SendAppointmentEmail(doctorEmail, doctorSubject, doctorMessage);

            return Ok(appointment.ToAppointmentDto());
        }

        [HttpPut("{id:int}/cancel")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CancelAppointment([FromRoute] int id, [FromBody] CancelAppointmentDto appointmentDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingAppointment = await _appointmentRepo.GetByIdAsync(id);
            if (existingAppointment == null)
                return NotFound("Appointment not found");
            if (existingAppointment.Status != Enums.AppointmentStatus.Pending)
                return BadRequest("Appointment has already been updated");

            var appointment = await _appointmentRepo.CancelAsync(id, appointmentDto);

            string patientEmail = appointment.Patient.User.Email;
            string patientFullname = appointment.Patient.User.FullName;

            string subject = "Important: Your Appointment Has Been Cancelled";
            string message = $@"
                <p>Dear {patientFullname},</p>
                <p>We regret to inform you that your scheduled appointment has been cancelled. Here are the details:</p>
                <ul>
                    <li><strong>Appointment ID:</strong> #{appointment.Id}</li>
                    <li><strong>Doctor:</strong> {appointment.Doctor.User.FullName}</li>
                    <li><strong>Reason for Cancellation:</strong> {appointment.ReasonForCancellation}</li>
                </ul>
                <p>You can schedule a new appointment anytime on our platform if you would like to reschedule.</p>
                <p>We apologize for any inconvenience and look forward to assisting you in the future.</p>
                <p>Best regards,</p>
                <p>CarePulse Team</p>";

            await SendAppointmentEmail(patientEmail, subject, message);

            return Ok(appointment.ToAppointmentDto());
        }
    }
}