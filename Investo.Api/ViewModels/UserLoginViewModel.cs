namespace Investo.Api.ViewModels;

using System.ComponentModel.DataAnnotations;

public class UserLoginViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    public string Password { get; set; }
}