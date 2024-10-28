using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using backend.DTOs.Patient;
using backend.Extensions;
using backend.Interfaces;
using backend.Mappers;
using backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/patient")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IPatientRepository _patientRepo;

        public PatientController(UserManager<User> userManager, IPatientRepository patientRepo)
        {
            _userManager = userManager;
            _patientRepo = patientRepo;
        }

        [HttpPost("profile")]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> UpdateProfile([FromBody] PatientProfileDto patientDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string userId = User.GetUserId();

            var patientModel = await _patientRepo.UpdateAsync(userId, patientDto);
            if (patientModel == null)
                return BadRequest("Primary Physician not found");

            return Ok(patientModel.ToPatientDto());
        }
    }
}