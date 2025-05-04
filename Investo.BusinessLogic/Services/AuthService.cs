using Investo.BusinessLogic.DTOs;
using Investo.BusinessLogic.Interfaces;
using Investo.DataAccess.Entities;
using Investo.DataAccess.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using BCrypt.Net;
using Microsoft.Extensions.Logging;

namespace Investo.BusinessLogic.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthService> _logger;

        public AuthService(IUserRepository userRepository, IConfiguration configuration, ILogger<AuthService> logger)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<AuthResult> RegisterAsync(UserRegistrationDto registrationDto)
        {
            _logger.LogInformation("Starting user registration for email: {Email}", registrationDto.Email);

            if (await _userRepository.EmailExistsAsync(registrationDto.Email))
            {
                _logger.LogWarning("Registration failed: Email {Email} already exists", registrationDto.Email);
                return new AuthResult
                {
                    Success = false,
                    Message = "Email is already registered"
                };
            }

            var user = new User
            {
                Email = registrationDto.Email,
                FullName = registrationDto.FullName,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(registrationDto.Password),
                UserType = registrationDto.UserType,
                CreatedAt = DateTime.UtcNow
            };

            try
            {
                _logger.LogInformation("Creating user in database");
                await _userRepository.CreateAsync(user);
                _logger.LogInformation("User created successfully with ID: {UserId}", user.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user: {Message}", ex.Message);
                return new AuthResult
                {
                    Success = false,
                    Message = $"An error occurred while creating the user: {ex.Message}"
                };
            }

            return new AuthResult
            {
                Success = true,
                Message = "Registration successful",
                UserType = user.UserType,
                Token = GenerateJwtToken(user) 
            };
        }

        public async Task<AuthResult> LoginAsync(string email, string password)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                return new AuthResult
                {
                    Success = false,
                    Message = "Invalid email or password"
                };
            }

            return new AuthResult
            {
                Success = true,
                Message = "Login successful",
                UserType = user.UserType,
                Token = GenerateJwtToken(user)
            };
        }

        private string GenerateJwtToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key not configured")));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim("UserType", user.UserType.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}