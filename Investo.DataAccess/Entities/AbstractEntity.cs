namespace Investo.DataAccess.Entities;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public abstract class AbstractEntity<T>
{
    [Key]
    [Column("id")]
    public T Id { get; set; }
}