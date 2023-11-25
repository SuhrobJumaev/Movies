using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Movies.BusinessLogic;
using Asp.Versioning;

namespace Movies.Web;


[ApiController]
[Route("api/users")]
[ApiVersion(Utils.API_VERSION_1)]
public class UserController : ControllerBase
{
    private IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [Authorize(Roles = Utils.AdminRole)]
    [HttpGet]
    [ProducesResponseType(typeof(UsersViewResponseDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllUsersAsync([FromQuery] UserOptionsDto optionsDto, CancellationToken token = default)
    {
        UsersViewResponseDto users = await _userService.GetAllUsersAsync(optionsDto,token);
        
        return Ok(users);
    }
         
    [Authorize(Roles = Utils.AdminRole)]
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(UserDtoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUserAsync(int id, CancellationToken token = default)
    {
        UserDtoResponse? user =  await _userService.GetUserByIdAsync(id, token);

        if(user is null)
        {
            return NotFound();
        }
        return Ok(user);
    }

    
    [Authorize(Roles = Utils.AdminRole)]
    [HttpPost]
    [ProducesResponseType(typeof(UserDtoResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationFailureResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateUserAsync([FromBody] UserDto user, CancellationToken token = default)
    {
        UserDtoResponse createdUser = await _userService.CreateUserAsync(user, token);

        return CreatedAtAction("GetUser", new {id = createdUser.Id}, createdUser);
    }

    [Authorize(Roles = Utils.AdminRole)]
    [HttpPut]
    [ProducesResponseType(typeof(UserDtoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationFailureResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> EditUserAsync([FromBody] UserDto user, CancellationToken token = default)
    {
        UserDtoResponse? updatedUser = await _userService.EditUserAsync(user, token);

        if (updatedUser is null)
            return NotFound();

        return Ok(updatedUser);
        
    }

    [Authorize(Roles = Utils.AdminRole)]
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteUserAsync(int id, CancellationToken token = default)
    {
        bool isDeleted = await _userService.DeleteUserAsync(id, token);

        if (!isDeleted)
            return NotFound();

        return Ok();
    }

}