using Investo.DataAccess.Entities;
using System.Threading.Tasks;

namespace Investo.DataAccess.Interfaces
{
    public interface IUserRepository
    {
        // Authentication methods
        Task<User> GetByEmailAsync(string email);
        Task<User> CreateAsync(User user);
        
        // Profile methods
        Task<User> GetByIdAsync(int id);
        Task<User> UpdateAsync(User user);
        Task<bool> EmailExistsAsync(string email);
    }
}