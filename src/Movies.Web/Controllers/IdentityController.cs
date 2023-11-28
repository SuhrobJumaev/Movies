using Asp.Versioning;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Movies.BusinessLogic;
using Movies.DataAccess;

namespace Movies.Web;

[ApiController]
[ApiVersion("1.0")]
public class IdentityController : ControllerBase
{

    private readonly IIdentityService _identityService;
    private readonly IUserService _userService;

    public IdentityController(IIdentityService identityService, IUserService userService)
    {
        _identityService = identityService;
        _userService = userService;
    }

    [HttpPost("token")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTokenAsync([FromBody] LoginDto loginDto, CancellationToken token = default)
    {
        UserDtoResponse? user = await _identityService.GetUserByEmailAndPasswordAsync(loginDto, token);

        if (user is null)
            return NotFound();

        string jwtToken =  _identityService.GenerateToken(user);

        return Ok(jwtToken);
    }

    [HttpPost("registration")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationFailureResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Registation([FromBody] RegistrationDto registrationDto, CancellationToken token = default)
    {
        UserDtoResponse createdUser = await _identityService.RegistrationUser(registrationDto, token);

        string jwtToken =  _identityService.GenerateToken(createdUser);

        return Ok(jwtToken);
    }


    [Authorize]
    [HttpPut("profile")]
    [ProducesResponseType(typeof(UserDtoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> EditProfileAsync([FromBody] UserDto user, CancellationToken token = default)
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
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ChangePasswordAsync([FromBody] ChangePasswordDto changePasswordDto, CancellationToken token = default)
    {
        changePasswordDto.UserId = HttpContext.GetUserId();
        changePasswordDto.Email = HttpContext.GetUserEmail();

        bool isChangedPassword = await _userService.ChangePasswordAsync(changePasswordDto, token = default);

        if (!isChangedPassword)
            return NotFound();

        return Ok();
    }

    [Authorize]
    [HttpGet("profile")]
    [ProducesResponseType(typeof(UserDtoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProfileAsync(CancellationToken token = default)
    {
        int userId = HttpContext.GetUserId();

        UserDtoResponse? user = await _userService.GetUserByIdAsync(userId, token);

        if (user is null)
        {
            return NotFound();
        }
        return Ok(user);
    }
}
