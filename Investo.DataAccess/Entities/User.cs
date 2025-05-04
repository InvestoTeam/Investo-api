using System;

namespace Investo.DataAccess.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string? AvatarUrl { get; set; }
        public string? WalletAddress { get; set; }
        public bool KycVerified { get; set; } = false;
        public bool TwoFactorEnabled { get; set; } = false;
        public UserType UserType { get; set; }
        public DateTime JoinDate { get; set; } = DateTime.UtcNow;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastLoginAt { get; set; }
    }
}