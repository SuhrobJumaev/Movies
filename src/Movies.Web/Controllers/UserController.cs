using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using FluentValidation.Results;
using Movies.BusinessLogic;

namespace Movies.Web;

[Authorize()]
[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [Authorize(Utils.AdminPolicyName)]
    [HttpGet]
    public async Task<IEnumerable<UserDtoResponse>> GetAllUsersAsync(CancellationToken token)
        => await _userService.GetAllUsersAsync(token);

    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(int id, CancellationToken token)
    {
        int userId = HttpContext.GetUserId();

        string userRole = HttpContext.GetUserRole();

        if (userRole == Utils.UserClaimValue && userId != id)
            return Forbid();

        UserDtoResponse? user =  await _userService.GetUserByIdAsync(id, token);

        if(user is null)
        {
            return NotFound();
        }
        return Ok(user);
    }

    [Authorize(Utils.AdminPolicyName)]
    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] UserDto user, CancellationToken token)
    {
        UserDtoResponse createdUser = await _userService.CreateUserAsync(user, token);

        return CreatedAtAction(nameof(GetUser), new {id = createdUser.Id}, createdUser);
    }

    [Authorize(Utils.AdminPolicyName)]
    [HttpPut]
    public async Task<IActionResult> EditUser([FromBody] UserDto user, CancellationToken token)
    {
        UserDtoResponse? updatedUser = await _userService.EditUserAsync(user, token);

        if (updatedUser is null)
            return NotFound();

        return Ok(updatedUser);
        
    }

    [Authorize(Utils.AdminPolicyName)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id, CancellationToken token)
    {
        bool isDeleted = await _userService.DeleteUserAsync(id, token);

        if (!isDeleted)
            return NotFound();

        return Ok();
    }


}