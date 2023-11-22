using Asp.Versioning;
using FluentValidation;
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
}
