using AutoMapper;
using Investo.Api.ViewModels;
using Investo.BusinessLogic.Interfaces;
using Investo.BusinessLogic.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

    [HttpGet("profile/{id:guid}")]
    public async Task<ActionResult<UserModel>> GetProfile([FromRoute] Guid id)
    {
        var user = await this.userService.GetUserByIdAsync(id);
        if (user is null)
        {
            return this.NotFound("User not found");
        }

        return Ok(user);
    }

    [Authorize]
    [HttpPut("profile")]
    public async Task<ActionResult> UpdateProfile([FromBody] UserUpdateViewModel userUpdateModel)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);

        bool result = await this.userService.UpdateProfileAsync(userId, this.mapper.Map<UserUpdateModel>(userUpdateModel));
        if (!result)
        {
            return this.BadRequest("Failed to update user profile");
        }

        return Ok();
    }
}
