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
    // TODO: Replace GUID with token
    public async Task<ActionResult<Guid>> CreateUser([FromBody] UserCreateViewModel user)
    {
        Guid? userId = await this.userService.RegisterUserAsync(this.mapper.Map<UserCreateModel>(user));

        if (userId is null)
        {
            return this.BadRequest("User already exists");
        }

        return this.Ok(userId);
    }
}
