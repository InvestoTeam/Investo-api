using Investo.DataAccess.Entities;

namespace Investo.DataAccess.Interfaces;

public interface IUserRepository : ICrudRepository<User, Guid>
{
}
