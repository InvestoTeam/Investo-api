using Investo.DataAccess.Entities;

namespace Investo.BusinessLogic.Models;

public class UserModel
{
    public Guid Id { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public DateTime RegistrationDate { get; set; }

    public int UserTypeId { get; set; }

    public UserType? UserType { get; set; }
}
