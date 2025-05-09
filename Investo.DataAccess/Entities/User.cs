namespace Investo.DataAccess.Entities;

using System.ComponentModel.DataAnnotations.Schema;

[Table("users")]
public class User : AbstractEntity<Guid>
{
    [Column("email")]
    public string Email { get; set; } = null!;

    [Column("first_name")]
    public string FirstName { get; set; } = null!;

    [Column("last_name")]
    public string LastName { get; set; } = null!;

    [Column("password_hash")]
    public string PasswordHash { get; set; } = null!;

    [Column("password_salt")]
    public string PasswordSalt { get; set; } = null!;

    [Column("refresh_token")]
    public string? RefreshToken { get; set; }

    [Column("refresh_token_expire_date")]
    public DateTime? RefreshTokenExpireDate { get; set; }

    [Column("registration_date")]
    public DateTime RegistrationDate { get; set; }

    [Column("user_type_id")]
    [ForeignKey(nameof(UserType))]
    public int UserTypeId { get; set; }

    public UserType? UserType { get; set; }
}
