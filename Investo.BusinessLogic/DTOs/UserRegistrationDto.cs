using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Investo.DataAccess.Entities;

namespace Investo.BusinessLogic.DTOs
{
    public class UserRegistrationDto
    {
        [Required]
        [MaxLength(100)]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        [Required]
        public UserType UserType { get; set; }
    }
} 