using System;

namespace Investo.Api.Models.DTOs
{
    public class UserProfileDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string AvatarUrl { get; set; }
        public string WalletAddress { get; set; }
        public bool KycVerified { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public DateTime JoinDate { get; set; }
        
        // Public profile information
        public bool IsPublicProfile { get; set; }
    }
}