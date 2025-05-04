using Investo.BusinessLogic.DTOs;
using Investo.DataAccess.Entities;

namespace Investo.BusinessLogic.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResult> RegisterAsync(UserRegistrationDto registrationDto);
        Task<AuthResult> LoginAsync(string email, string password);
    }

    public class AuthResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public UserType? UserType { get; set; }
        public string Token { get; set; }
    }
} 