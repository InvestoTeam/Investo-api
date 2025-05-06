namespace Investo.BusinessLogic.Services;

using AutoMapper;
using Investo.BusinessLogic.Interfaces;
using Investo.BusinessLogic.Models;
using Investo.DataAccess.EF;
using Investo.DataAccess.Entities;
using Investo.DataAccess.Interfaces;
using Investo.DataAccess.Repositories;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

public class UserService : IUserService
{
    private readonly IJwtService jwtService;
    private readonly IUserRepository userRepository;
    private readonly IMapper mapper;
    public UserService(ApplicationDbContext context, IJwtService jwtService, IMapper mapper)
    {
        this.userRepository = new UserRepository(context);
        this.jwtService = jwtService;
        this.mapper = mapper;
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

    private string GenerateHash(byte[] password, byte[] salt)
    {
        byte[] hash = SHA256.HashData(password.Concat(salt).ToArray());
        return Convert.ToBase64String(hash);
    }
}
