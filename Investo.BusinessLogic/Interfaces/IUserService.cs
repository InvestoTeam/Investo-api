using Investo.BusinessLogic.Models;

namespace Investo.BusinessLogic.Interfaces;

public interface IUserService
{
    Task<Guid?> RegisterUserAsync(UserCreateModel userCreateModel);
}
