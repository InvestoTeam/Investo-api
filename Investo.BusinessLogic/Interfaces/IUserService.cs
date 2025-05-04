using System.Threading.Tasks;
using Investo.BusinessLogic.DTOs;
using Investo.DataAccess.Entities;

namespace Investo.BusinessLogic.Interfaces
{
    public interface IUserService
    {
        Task<User?> GetUserByIdAsync(int id);
        Task<User?> GetUserByEmailAsync(string email);
        Task<bool> UpdateUserAsync(User user);
        Task<User?> UpdateUserProfileAsync(int userId, UserProfileUpdateDto profileUpdate);
    }
}