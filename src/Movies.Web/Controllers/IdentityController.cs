using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Movies.BusinessLogic;

namespace Movies.Web;

[ApiController]
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
    public async Task<IActionResult> GetTokenAsync([FromBody] LoginDto loginDto, CancellationToken token = default)
    {
        UserDtoResponse? user = await _identityService.GetUserByEmailAndPasswordAsync(loginDto, token);

        if (user is null)
            return NotFound();

        string jwtToken =  _identityService.GenerateToken(user);

        return Ok(jwtToken);
    }
}
