using Investo.DataAccess.Entities;
using Investo.DataAccess.EF;
using Investo.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Investo.DataAccess.Repositories;

public class UserRepository : AbstractRepository<User, Guid>, IUserRepository
{
    public UserRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<string?> GetUserSalt(string email)
    {
        var user = await this.dbSet.FirstOrDefaultAsync(u => u.Email == email);
        return user?.PasswordSalt;
    }

    public async Task<User?> LoginAsync(string email, string passwordHash)
    {
        var user = await this.dbSet.FirstOrDefaultAsync(u => u.Email == email && u.PasswordHash == passwordHash);
        return user;
    }

    public async Task<string?> SetRefreshToken(Guid userId, string refreshToken, DateTime expireDate)
    {
        var user = await this.dbSet.FirstOrDefaultAsync(u => u.Id == userId);
        if (user is null)
        {
            return null;
        }
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpireDate = expireDate;
        await this.context.SaveChangesAsync();
        return refreshToken;
    }
}