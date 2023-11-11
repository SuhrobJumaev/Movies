using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Movies.BusinessLogic;

namespace Movies.Web;

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private IUserService _userService;

    public UserController(
        IUserService userService
        )
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<IEnumerable<UserDto>> GetAllUsersAsync(CancellationToken token)
    {
        IEnumerable<UserDto> users = null;
        return users;
    }

    [HttpGet("{id}")]
    public async Task<UserDto> GetUserAsync([FromQuery] int id, CancellationToken token)
    {
        UserDto user = new();
        return user;
    }

    [HttpPost]
    public async Task<UserDto> CreateUserAsync(CancellationToken token)
    {
         UserDto user = new();
        return user;
    }


    [HttpPut]
    public async Task<UserDto> EditUserAsync([FromBody] UserDto user, CancellationToken token)
    {
       
        return user;
    }

    [HttpDelete]
    public async ValueTask<bool> DeleteUserAsync([FromQuery] int id, CancellationToken token)
    {
        return true;
    }


}