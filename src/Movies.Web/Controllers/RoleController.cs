using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Movies.BusinessLogic;
namespace Movies.Web;

[ApiController]
[Route("api/roles")]
[ApiVersion(Utils.API_VERSION_1)]
[Authorize(Roles = Utils.AdminRole)]
public class RoleController : ControllerBase
{
    private IRoleService _roleService;

    public RoleController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(RoleDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationFailureResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateRoleAsync([FromBody] RoleDto roleDto, CancellationToken token = default)
    {
        RoleDto createdRole = await _roleService.CreateRoleAsync(roleDto, token);

        return CreatedAtAction("GetRole", new { id = createdRole.Id }, createdRole);
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<RoleDto>),StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllRolesAsync(CancellationToken token = default)
    {
        IEnumerable<RoleDto> roles = await _roleService.GetAllRolesAsync(token);

        return Ok(roles);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(RoleDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetRoleAsync(int id, CancellationToken token = default)
    {
        RoleDto? role = await _roleService.GetRoleByIdAsync(id, token);

        if (role is null)
            return NotFound();

        return Ok(role);
    }

    [HttpPut]
    [ProducesResponseType(typeof(RoleDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationFailureResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> EditRoleAsync([FromBody] RoleDto roleDto, CancellationToken token = default)
    {
        RoleDto? updatedRole = await _roleService.EditRoleAsync(roleDto, token);

        if (updatedRole is null)
            return NotFound();

        return Ok(updatedRole);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteRoleAsync(int id, CancellationToken token = default)
    {
        bool isDeleted = await _roleService.DeleteRoleAsync(id, token);

        if (!isDeleted)
            return NotFound();

        return Ok();
    }
}
