using System.ComponentModel.DataAnnotations;

namespace Investo.Api.ViewModels;

public class UserUpdateViewModel
{
    [Required]
    [RegularExpression(@"^\w{2,100}$")]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [RegularExpression(@"^\w{2,100}$")]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
}
