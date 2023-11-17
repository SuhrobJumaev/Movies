using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using FluentValidation.Results;
using Movies.BusinessLogic;

namespace Movies.Web;


[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [Authorize(Roles = Utils.AdminRole)]
    [HttpGet]
    public async Task<IActionResult> GetAllUsersAsync(CancellationToken token)
    {
        IEnumerable<UserDtoResponse> users = await _userService.GetAllUsersAsync(token);
        
        return Ok(users);
    }
         

    [Authorize(Roles = Utils.AdminRole)]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserAsync(int id, CancellationToken token)
    {
        UserDtoResponse? user =  await _userService.GetUserByIdAsync(id, token);

        if(user is null)
        {
            return NotFound();
        }
        return Ok(user);
    }

    [Authorize]
    [HttpGet("profile")]
    public async Task<IActionResult> GetAsync(CancellationToken token)
    {
        int userId = HttpContext.GetUserId();

        UserDtoResponse? user = await _userService.GetUserByIdAsync(userId, token);

        if (user is null)
        {
            return NotFound();
        }
        return Ok(user);
    }

    [Authorize(Roles = Utils.AdminRole)]
    [HttpPost]
    public async Task<IActionResult> CreateUserAsync([FromBody] UserDto user, CancellationToken token)
    {
        UserDtoResponse createdUser = await _userService.CreateUserAsync(user, token);

        return CreatedAtAction("GetUser", new {id = createdUser.Id}, createdUser);
    }

    [Authorize(Roles = Utils.AdminRole)]
    [HttpPut]
    public async Task<IActionResult> EditUserAsync([FromBody] UserDto user, CancellationToken token)
    {
        UserDtoResponse? updatedUser = await _userService.EditUserAsync(user, token);

        if (updatedUser is null)
            return NotFound();

        return Ok(updatedUser);
        
    }

    [Authorize(Roles = Utils.AdminRole)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUserAsync(int id, CancellationToken token)
    {
        bool isDeleted = await _userService.DeleteUserAsync(id, token);

        if (!isDeleted)
            return NotFound();

        return Ok();
    }


}