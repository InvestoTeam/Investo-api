using Investo.DataAccess.Entities;
using Investo.DataAccess.EF;
using Investo.DataAccess.Interfaces;

namespace Investo.DataAccess.Repositories;

public class UserRepository : AbstractRepository<User, Guid>, IUserRepository
{
    public UserRepository(ApplicationDbContext context) : base(context)
    {
    }
}