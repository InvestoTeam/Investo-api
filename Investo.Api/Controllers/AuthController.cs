using Microsoft.AspNetCore.Mvc;
using Investo.DataAccess.Interfaces;
using Investo.DataAccess.Entities;
using Investo.BusinessLogic.DTOs;
using Investo.BusinessLogic.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace Investo.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;
        
        public AuthController(
            IConfiguration configuration, 
            IUserRepository userRepository, 
            IAuthService authService,
            ILogger<AuthController> logger)
        {
            _configuration = configuration;
            _userRepository = userRepository;
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationDto registrationDto)
        {
            _logger.LogInformation("Received registration request for email: {Email}", registrationDto.Email);
            _logger.LogDebug("Registration data: {@RegistrationData}", registrationDto);

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                
                _logger.LogWarning("Validation failed: {@Errors}", errors);
                
                return BadRequest(new { 
                    success = false,
                    message = "Validation failed",
                    errors = errors
                });
            }

            try
            {
                _logger.LogInformation("Calling AuthService.RegisterAsync");
                var result = await _authService.RegisterAsync(registrationDto);
                _logger.LogInformation("AuthService.RegisterAsync result: {@Result}", result);
                
                if (!result.Success)
                {
                    _logger.LogWarning("Registration failed: {Message}", result.Message);
                    return BadRequest(new { 
                        success = false,
                        message = result.Message 
                    });
                }

                _logger.LogInformation("Registration successful for user: {Email}", registrationDto.Email);
                return Ok(new { 
                    success = true, 
                    message = "Registration successful",
                    userType = result.UserType,
                    token = result.Token
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during registration: {Message}", ex.Message);
                return StatusCode(500, new { 
                    success = false,
                    message = "An error occurred during registration",
                    error = ex.Message
                });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                
                return BadRequest(new { 
                    success = false,
                    message = "Validation failed",
                    errors = errors
                });
            }

            try
            {
                var result = await _authService.LoginAsync(request.Email, request.Password);
                
                if (!result.Success)
                {
                    return Unauthorized(new { 
                        success = false,
                        message = result.Message 
                    });
                }

                return Ok(new { 
                    success = true, 
                    message = "Login successful",
                    token = result.Token,
                    userType = result.UserType
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { 
                    success = false,
                    message = "An error occurred during login",
                    error = ex.Message
                });
            }
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out int id))
            {
                return Unauthorized(new { 
                    success = false,
                    message = "Unauthorized" 
                });
            }

            var user = await _userRepository.GetByIdAsync(id);

            if (user == null)
            {
                return NotFound(new { 
                    success = false,
                    message = "User not found" 
                });
            }

            return Ok(new {
                success = true,
                id = user.Id,
                email = user.Email,
                fullName = user.FullName,
                userType = user.UserType
            });
        }
    }

    public class LoginRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}