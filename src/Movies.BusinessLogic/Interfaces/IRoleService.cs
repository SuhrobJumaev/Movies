
namespace Movies.BusinessLogic;

public interface IRoleService
{
    Task<RoleDto> CreateRoleAsync(RoleDto roleDto, CancellationToken token = default);
    Task<RoleDto?> GetRoleByIdAsync(int id, CancellationToken token = default);
    Task<IEnumerable<RoleDto>> GetAllRolesAsync(CancellationToken token = default);
    Task<RoleDto?> EditRoleAsync(RoleDto roleDto, CancellationToken token = default);
    ValueTask<bool> DeleteRoleAsync(int id, CancellationToken token = default);

}
