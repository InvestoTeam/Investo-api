using Investo.BusinessLogic.Models;
using System.Security.Claims;

namespace Investo.BusinessLogic.Interfaces;

public interface IJwtService
{
    string GenerateToken(UserModel userModel);
    string GenerateRefreshToken();
    ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
}
