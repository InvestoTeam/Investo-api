using AutoMapper;
using Investo.Api.ViewModels;
using Investo.BusinessLogic.Interfaces;
using Investo.BusinessLogic.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Investo.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService userService;
    private readonly IMapper mapper;

    public UserController(IUserService userService, IMapper mapper)
    {
        this.userService = userService;
        this.mapper = mapper;
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserTokenDataModel>> CreateUser([FromBody] UserCreateViewModel user)
    {
        await this.userService.RegisterUserAsync(this.mapper.Map<UserCreateModel>(user));
        UserTokenDataModel? userToken = await this.userService.LoginAsync(this.mapper.Map<UserLoginModel>(user));
        if (userToken is null)
        {
            return this.BadRequest("Invalid email or password");
        }

        return this.Ok(userToken);
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserTokenDataModel>> LoginUser([FromBody] UserLoginViewModel user)
    {
        UserTokenDataModel? userToken = await this.userService.LoginAsync(this.mapper.Map<UserLoginModel>(user));
        if (userToken is null)
        {
            return this.BadRequest("Invalid email or password");
        }

        return this.Ok(userToken);
    }

    [HttpPost("refresh")]
    public async Task<ActionResult<UserTokenDataModel>> RefreshToken([FromBody] UserRefreshTokenViewModel oldToken)
    {
        UserTokenDataModel? userToken = await this.userService.RefreshTokenAsync(new UserTokenDataModel
        {
            Token = oldToken.OldToken,
            RefreshToken = oldToken.RefreshToken,
        });
        if (userToken is null)
        {
            return this.BadRequest("Invalid refresh token");
        }

        return this.Ok(userToken);
    }
}
