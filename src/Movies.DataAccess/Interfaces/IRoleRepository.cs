
namespace Movies.DataAccess;

public interface IRoleRepository
{
    ValueTask<int> CreateRoleAsync(Role role, CancellationToken token = default);
    Task<Role?> GetRoleByIdAsync(int id, CancellationToken token = default);
    Task<IEnumerable<Role>> GetAllRolesAsync(CancellationToken token = default);
    ValueTask<bool> EditRoleAsync(Role role, CancellationToken token = default);
    ValueTask<bool> DeleteRoleAsync(int id, CancellationToken token = default);
}
