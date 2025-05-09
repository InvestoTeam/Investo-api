namespace Investo.DataAccess.Repositories;

using Investo.DataAccess.EF;
using Investo.DataAccess.Entities;
using Investo.DataAccess.Interfaces;

public class UserTypeRepository : AbstractRepository<UserType, int>, IUserTypeRepository
{
    public UserTypeRepository(ApplicationDbContext context) : base(context)
    {
    }
}
