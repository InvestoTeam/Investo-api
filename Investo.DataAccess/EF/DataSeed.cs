using Investo.DataAccess.Entities;

namespace Investo.DataAccess.EF;

public static class DataSeed
{
    public static IEnumerable<UserType> GetUserTypes()
    {
        return new List<UserType>
        {
            new UserType { Id = (int)UserTypes.Admin, Title = Enum.GetName(UserTypes.Admin)! }, 
            new UserType { Id = (int)UserTypes.Investor, Title = Enum.GetName(UserTypes.Investor)! },
            new UserType { Id = (int)UserTypes.PropertyOwner, Title = Enum.GetName(UserTypes.PropertyOwner)! },
        };
    }
}
