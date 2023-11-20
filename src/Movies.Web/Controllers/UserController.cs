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
    public async Task<IActionResult> GetAllUsersAsync([FromQuery] UserOptionsDto optionsDto, CancellationToken token = default)
    {
        UsersViewResponseDto users = await _userService.GetAllUsersAsync(optionsDto,token);
        
        return Ok(users);
    }
         

    [Authorize(Roles = Utils.AdminRole)]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserAsync(int id, CancellationToken token = default)
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
    public async Task<IActionResult> GetAsync(CancellationToken token = default)
    {
        int userId = HttpContext.GetUserId();

        UserDtoResponse? user = await _userService.GetUserByIdAsync(userId, token);

        if (user is null)
        {
            return NotFound();
        }
        return Ok(user);
    }

    [Authorize]
    [HttpPut("profile")]
    public async Task<IActionResult> EditProfileAsync([FromBody]UserDto user,CancellationToken token = default)
    {
        int userId = HttpContext.GetUserId();

        if (userId != user.Id)
            return Forbid();

        UserDtoResponse? updatedUser = await _userService.EditProfileAsync(user, token);

        if (updatedUser is null)
            return NotFound();

        return Ok(updatedUser);
    }

    [Authorize]
    [HttpPut("change-password")]
    public async Task<IActionResult> ChangePasswordAsync([FromBody]ChangePasswordDto changePasswordDto, CancellationToken token = default)
    {
        changePasswordDto.UserId = HttpContext.GetUserId();
        changePasswordDto.Email = HttpContext.GetUserEmail();

        bool isChangedPassword = await _userService.ChangePasswordAsync(changePasswordDto, token = default);

        if (!isChangedPassword)
            return NotFound();

        return Ok();
    }

    [Authorize(Roles = Utils.AdminRole)]
    [HttpPost]
    public async Task<IActionResult> CreateUserAsync([FromBody] UserDto user, CancellationToken token = default)
    {
        UserDtoResponse createdUser = await _userService.CreateUserAsync(user, token);

        return CreatedAtAction("GetUser", new {id = createdUser.Id}, createdUser);
    }

    [Authorize(Roles = Utils.AdminRole)]
    [HttpPut]
    public async Task<IActionResult> EditUserAsync([FromBody] UserDto user, CancellationToken token = default)
    {
        UserDtoResponse? updatedUser = await _userService.EditUserAsync(user, token);

        if (updatedUser is null)
            return NotFound();

        return Ok(updatedUser);
        
    }

    [Authorize(Roles = Utils.AdminRole)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUserAsync(int id, CancellationToken token = default)
    {
        bool isDeleted = await _userService.DeleteUserAsync(id, token);

        if (!isDeleted)
            return NotFound();

        return Ok();
    }


}