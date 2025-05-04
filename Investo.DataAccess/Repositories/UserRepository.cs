using Microsoft.EntityFrameworkCore;
using Investo.DataAccess.Entities;
using Investo.DataAccess.Interfaces;
using Investo.DataAccess.EF;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Investo.DataAccess.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(ApplicationDbContext context, ILogger<UserRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<User> GetByIdAsync(int id)
        {
            _logger.LogInformation("Getting user by ID: {Id}", id);
            return await _context.Users.FindAsync(id);
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            _logger.LogInformation("Getting user by email: {Email}", email);
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            _logger.LogInformation("Checking if email exists: {Email}", email);
            return await _context.Users
                .AnyAsync(u => u.Email.ToLower() == email.ToLower());
        }

        public async Task<User> CreateAsync(User user)
        {
            _logger.LogInformation("Creating new user: {@User}", user);
            try
            {
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
                _logger.LogInformation("User created successfully with ID: {Id}", user.Id);
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user: {Message}", ex.Message);
                throw;
            }
        }

        public async Task<User> UpdateAsync(User user)
        {
            _logger.LogInformation("Updating user with ID: {Id}", user.Id);
            try
            {
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                _logger.LogInformation("User updated successfully");
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user: {Message}", ex.Message);
                throw;
            }
        }
    }
}