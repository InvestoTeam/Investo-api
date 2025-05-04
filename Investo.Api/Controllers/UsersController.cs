using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Investo.Api.Models.DTOs;
using Investo.DataAccess.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Investo.BusinessLogic.Interfaces;
using Investo.BusinessLogic.DTOs;
using Investo.DataAccess.Entities;

namespace Investo.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UsersController> _logger;
        private readonly IUserService _userService;

        public UsersController(
            IUserRepository userRepository,
            ILogger<UsersController> logger,
            IUserService userService)
        {
            _userRepository = userRepository;
            _logger = logger;
            _userService = userService;
        }

        // GET: api/users/profile
        [HttpGet("profile")]
        public async Task<IActionResult> GetUserProfile()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return Unauthorized();
            }

            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found");
            }

            return Ok(new
            {
                user.Id,
                user.Email,
                user.FullName,
                user.UserType,
                user.AvatarUrl,
                user.WalletAddress
            });
        }

        [HttpPut("profile")]
        public async Task<IActionResult> UpdateUserProfile([FromBody] UserProfileUpdateDto profileUpdate)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return Unauthorized();
            }

            var updatedUser = await _userService.UpdateUserProfileAsync(userId, profileUpdate);
            if (updatedUser == null)
            {
                return NotFound("User not found");
            }

            return Ok(new
            {
                updatedUser.Id,
                updatedUser.Email,
                updatedUser.FullName,
                updatedUser.UserType,
                updatedUser.AvatarUrl,
                updatedUser.WalletAddress
            });
        }

        // GET: api/users/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<UserProfileDto>> GetUserProfile(int id)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(id);
                
                if (user == null)
                {
                    return NotFound();
                }

                var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                bool isCurrentUser = !string.IsNullOrEmpty(currentUserId) && 
                                     int.TryParse(currentUserId, out int currentId) && 
                                     currentId == id;

                // If viewing someone else's profile, return limited info
                return Ok(MapToProfileDto(user, !isCurrentUser));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting user profile for ID {id}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving user profile data");
            }
        }

        private UserProfileDto MapToProfileDto(DataAccess.Entities.User user, bool isPublicProfile)
        {
            var profileDto = new UserProfileDto
            {
                Id = user.Id,
                Name = user.FullName,
                Email = isPublicProfile ? null : user.Email, // Don't expose email on public profile
                AvatarUrl = user.AvatarUrl ?? "assets/images/avatar-placeholder.png",
                WalletAddress = user.WalletAddress ?? "0x0000000000000000000000000000000000000000",
                KycVerified = user.KycVerified,
                TwoFactorEnabled = isPublicProfile ? false : user.TwoFactorEnabled, // Don't expose 2FA status on public profile
                JoinDate = user.JoinDate,
                IsPublicProfile = isPublicProfile
            };

            return profileDto;
        }
    }
}