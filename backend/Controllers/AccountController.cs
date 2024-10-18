using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.DTOs;
using backend.DTOs.Account;
using backend.DTOs.Error;
using backend.Helpers;
using backend.Interfaces;
using backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/account")]
    [Produces("application/json")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IEmailService _emailService;
        private readonly ISMSService _smsService;
        private readonly ITokenService _tokenService;
        private readonly ISecretService _secretService;
        public AccountController(UserManager<User> userManager, IEmailService emailService, ISMSService smsService, ITokenService tokenService, ISecretService secretService)
        {
            _userManager = userManager;
            _emailService = emailService;
            _smsService = smsService;
            _tokenService = tokenService;
            _secretService = secretService;
        }

        private async Task SendConfirmationEmail(string email, User user)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var otp = token.Split(':')[0];

            await _emailService.SendEmailAsync(new SendEmailObject
            {
                ToEmail = email,
                Subject = "Confirm Your Email",
                Body = $"<p>Dear {user.FullName},</p><br/><p>Your OTP Code for verifying your account is {otp}.</p>"
            });
        }

        private async Task<bool> SendPhoneVerificationCode(string phone, User user)
        {
            var token = await _userManager.GenerateChangePhoneNumberTokenAsync(user, phone);
            var otp = token.Split(':')[0];

            var result = await _smsService.SendSmsAsync(phone, $"Your OTP code is {otp}");
            return result;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto, [FromQuery] bool patient = true)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (!patient)
                {
                    string secretKey = HttpContext.Request.Headers["Secret-Key"];
                    if (!_secretService.ValidateSecret(secretKey))
                        return Unauthorized("Invalid Secret Key");
                }

                var user = new User
                {
                    FullName = registerDto.FullName,
                    Email = registerDto.Email,
                    PhoneNumber = registerDto.PhoneNumber,
                    UserName = $"{registerDto.FullName.Replace(" ", "").ToLower()}_{Guid.NewGuid()}"
                };

                var createdUser = await _userManager.CreateAsync(user);
                if (!createdUser.Succeeded)
                    return StatusCode(500, new ErrorResponse
                    {
                        Message = "User Creation Failed",
                        Errors = createdUser.Errors.Select(e => e.Description).ToList()
                    });

                string role = patient ? "Patient" : registerDto.Role.ToString();
                var roleResult = await _userManager.AddToRoleAsync(user, role);
                if (!roleResult.Succeeded)
                    return StatusCode(500, new ErrorResponse
                    {
                        Message = "Role Creation Failed",
                        Errors = roleResult.Errors.Select(e => e.Description).ToList()
                    });

                await SendConfirmationEmail(registerDto.Email, user);

                return Ok(
                    new NewUserDto
                    {
                        Id = user.Id,
                        UserName = user.UserName,
                        Message = "Code has been sent to your inbox"
                    }
                );
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e);
            }
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] ConfirmObject emailObject)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByIdAsync(emailObject.UserId);
            if (user == null)
                return BadRequest("User not found");
            var result = await _userManager.ConfirmEmailAsync(user, emailObject.Token);
            if (!result.Succeeded)
                return BadRequest("Invalid or Expired OTP Token");

            var role = (await _userManager.GetRolesAsync(user)).FirstOrDefault();

            if (role == null)
                return BadRequest("User does not have a role");

            string SMSMessage = "";
            bool sent = false;
            if (!user.PhoneNumberConfirmed)
            {
                sent = await SendPhoneVerificationCode(user.PhoneNumber, user);
                SMSMessage = sent ? $"OTP Code sent to {user.PhoneNumber}" : $"SMS Code could not be sent. Try again";
            }

            return Ok(
                new ConfirmEmailDto
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    UserName = user.UserName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                    Role = role,
                    SMSSent = sent,
                    SMSMessage = SMSMessage,
                    Token = _tokenService.CreateToken(user, role),
                    Message = "Email Confirmed Successfully"
                }
            );
        }

        [HttpGet("confirm-phone")]
        public async Task<IActionResult> ConfirmPhone([FromQuery] ConfirmObject phoneObject)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByIdAsync(phoneObject.UserId);
            if (user == null)
                return BadRequest("User does not exist");

            var result = await _userManager.VerifyChangePhoneNumberTokenAsync(user, phoneObject.Token, user.PhoneNumber);
            if (!result)
                return BadRequest("Invalid or Expired OTP Code");

            user.PhoneNumberConfirmed = true;
            await _userManager.UpdateAsync(user);

            var role = (await _userManager.GetRolesAsync(user)).FirstOrDefault();
            if (role == null)
                return BadRequest("User does not have a role");

            return Ok(
                new ConfirmPhoneDto
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    UserName = user.UserName,
                    Email = user.Email,
                    EmailConfirmed = user.EmailConfirmed,
                    PhoneNumber = user.PhoneNumber,
                    PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                    Role = role,
                    Token = _tokenService.CreateToken(user, role),
                    Message = "Phone Number Confirmed Successfully"
                }
            );
        }

        [HttpPost("resend-email")]
        public async Task<IActionResult> ResendEmail([FromBody] ResendEmailDto emailDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(emailDto.Email);
            if (user == null || await _userManager.IsEmailConfirmedAsync(user))
                return Ok(new
                {
                    Message = "Email already Confirmed"
                });

            await SendConfirmationEmail(emailDto.Email, user);
            return Ok(new NewUserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Message = "Confirmation Email Sent"
            });
        }

        [HttpPost("resend-phone")]
        public async Task<IActionResult> ResendPhone([FromBody] ResendPhoneDto phoneDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(phoneDto.Email);
            if (user == null || await _userManager.IsPhoneNumberConfirmedAsync(user))
                return Ok(new
                {
                    Message = "Phone number already Confirmed"
                });

            var sent = await SendPhoneVerificationCode(phoneDto.PhoneNumber, user);
            return Ok(new NewUserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Message = sent ? "Phone Number Verification Code Sent" : "Code not sent. Try again later"
            });
        }
    }
}