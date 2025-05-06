namespace Investo.BusinessLogic.Services;

using AutoMapper;
using Investo.BusinessLogic.Interfaces;
using Investo.BusinessLogic.Models;
using Investo.DataAccess.EF;
using Investo.DataAccess.Entities;
using Investo.DataAccess.Interfaces;
using Investo.DataAccess.Repositories;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

public class UserService : IUserService
{
    private readonly IJwtService jwtService;
    private readonly IUserRepository userRepository;
    private readonly IMapper mapper;
    private readonly IConfiguration configuration;

    public UserService(ApplicationDbContext context, IJwtService jwtService, IMapper mapper, IConfiguration configuration)
    {
        this.userRepository = new UserRepository(context);
        this.jwtService = jwtService;
        this.mapper = mapper;
        this.configuration = configuration;
    }

    public async Task<UserTokenDataModel?> LoginAsync(UserLoginModel userLoginModel)
    {
        var passwordSalt = await this.userRepository.GetUserSalt(userLoginModel.Email);
        if (passwordSalt is null)
        {
            return null;
        }

        byte[] password = Encoding.UTF8.GetBytes(userLoginModel.Password);
        string hashedPassword = this.GenerateHash(password, Convert.FromBase64String(passwordSalt));

        var userEntity = await this.userRepository.LoginAsync(userLoginModel.Email, hashedPassword);
        if (userEntity is null)
        {
            return null;
        }

        string token = this.jwtService.GenerateToken(this.mapper.Map<UserModel>(userEntity));

        string? refreshToken = await this.SetNewRefreshToken(userEntity.Id);
        if (refreshToken is null)
        {
            return null;
        }

        return new UserTokenDataModel
        {
            Token = token,
            RefreshToken = refreshToken,
        };
    }

    public async Task<Guid?> RegisterUserAsync(UserCreateModel userCreateModel)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(32);
        byte[] password = Encoding.UTF8.GetBytes(userCreateModel.Password);

        string hashPassword = this.GenerateHash(password, salt);
        string saltString = Convert.ToBase64String(salt);

        User? userEntity = this.mapper.Map<User>(userCreateModel);

        if (userEntity is null)
        {
            return null;
        }

        userEntity.PasswordHash = hashPassword;
        userEntity.PasswordSalt = saltString;
        userEntity.UserTypeId = userCreateModel.UserTypeId;
        userEntity.RegistrationDate = DateTime.UtcNow;

        return await this.userRepository.CreateAsync(userEntity);
    }

    public async Task<UserTokenDataModel?> RefreshTokenAsync(UserTokenDataModel userTokenDataModel)
    {
        ClaimsPrincipal? principals = this.jwtService.GetPrincipalFromExpiredToken(userTokenDataModel.Token);
        if (principals is null)
        {
            return null;
        }

        var id = principals.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (id is null)
        {
            return null;
        }

        Guid userId = Guid.Parse(id);
        string? oldRefreshToken = await this.userRepository.GetRefreshToken(userId);
        if (oldRefreshToken is null || !oldRefreshToken.Equals(userTokenDataModel.RefreshToken))
        {
            return null;
        }

        string? newRefreshToken = await this.SetNewRefreshToken(userId);
        if (newRefreshToken is null)
        {
            return null;
        }

        var userEnity = await this.userRepository.GetByIdAsync(userId);
        string? newToken = this.jwtService.GenerateToken(this.mapper.Map<UserModel>(userEnity));

        return new UserTokenDataModel
        {
            Token = newToken,
            RefreshToken = newRefreshToken,
        };
    }

    private async Task<string?> SetNewRefreshToken(Guid userId)
    {
        string refreshToken = this.jwtService.GenerateRefreshToken();
        DateTime expireDate = DateTime.UtcNow.AddDays(int.Parse(this.configuration["JWT:RefreshTokenExpirationDays"] ?? "7"));

        return (await this.userRepository.SetRefreshToken(userId, refreshToken, expireDate) is not null) ? refreshToken : null;
    }

    private string GenerateHash(byte[] password, byte[] salt)
    {
        byte[] hash = SHA256.HashData(password.Concat(salt).ToArray());
        return Convert.ToBase64String(hash);
    }
}
