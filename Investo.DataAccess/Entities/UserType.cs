namespace Investo.DataAccess.Entities;

using System.ComponentModel.DataAnnotations.Schema;

[Table("user_types")]
public class UserType : AbstractEntity<int>
{
    [Column("title")]
    public string Title { get; set; } = null!;
}

public enum UserTypes
{
    Admin = 1,
    Investor = 2,
    PropertyOwner = 3,
}