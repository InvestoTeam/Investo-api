using System;
using System.Threading.Tasks;
using Investo.BusinessLogic.DTOs;
using Investo.BusinessLogic.Interfaces;
using Investo.DataAccess.Entities;
using Investo.DataAccess.Interfaces;

namespace Investo.BusinessLogic.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetByIdAsync(id);
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _userRepository.GetByEmailAsync(email);
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            var existingUser = await _userRepository.GetByIdAsync(user.Id);
            if (existingUser == null)
            {
                return false;
            }

            await _userRepository.UpdateAsync(user);
            return true;
        }

        public async Task<User?> UpdateUserProfileAsync(int userId, UserProfileUpdateDto profileUpdate)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            
            if (user == null)
            {
                return null;
            }

            // Update user properties
            if (!string.IsNullOrEmpty(profileUpdate.FullName))
            {
                user.FullName = profileUpdate.FullName;
            }

            if (!string.IsNullOrEmpty(profileUpdate.AvatarUrl))
            {
                user.AvatarUrl = profileUpdate.AvatarUrl;
            }

            if (!string.IsNullOrEmpty(profileUpdate.WalletAddress))
            {
                user.WalletAddress = profileUpdate.WalletAddress;
            }

            // Save changes
            return await _userRepository.UpdateAsync(user);
        }
    }
}