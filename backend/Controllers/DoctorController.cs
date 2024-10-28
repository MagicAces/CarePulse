using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    [Route("api/doctor")]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorRepository _doctorRepo;

        public DoctorController(IDoctorRepository doctorRepo)
        {
            _doctorRepo = doctorRepo;
        }

        [HttpGet]
        [Authorize(Roles = "Patient, Admin")]
        public async Task<IActionResult> GetDoctors([FromQuery] DoctorQueryObject query)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var doctors = await _doctorRepo.GetAllAsync(query);
            var doctorsDto = doctors.Select(d => d.ToDoctorDto()).ToList();

            return Ok(doctorsDto);
        }
    }
}