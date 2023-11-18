using Microsoft.AspNetCore.Mvc;
using Movies.BusinessLogic;

namespace Movies.Web;

[ApiController]
[Route("api/roles")]
public class RoleController : ControllerBase
{
    private IRoleService _roleService;

    public RoleController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateRoleAsync([FromBody] RoleDto roleDto, CancellationToken token)
    {
        RoleDto createdRole = await _roleService.CreateRoleAsync(roleDto);

        return CreatedAtAction("GetRole", new { id = createdRole.Id }, createdRole);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllRolesAsync(CancellationToken token)
    {
        IEnumerable<RoleDto> roles = await _roleService.GetAllRolesAsync(token);

        return Ok(roles);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetRoleAsync(int id, CancellationToken token)
    {
        RoleDto? role = await _roleService.GetRoleByIdAsync(id, token);

        if (role is null)
            return NotFound();

        return Ok(role);
    }

    [HttpPut]
    public async Task<IActionResult> EditRoleAsync([FromBody] RoleDto roleDto, CancellationToken token)
    {
        RoleDto? updatedRole = await _roleService.EditRoleAsync(roleDto, token);

        if (updatedRole is null)
            return NotFound();

        return Ok(updatedRole);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRoleAsync(int id, CancellationToken token)
    {
        bool isDeleted = await _roleService.DeleteRoleAsync(id, token);

        if (!isDeleted)
            return NotFound();

        return Ok();
    }
}
