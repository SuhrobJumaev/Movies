
using Dapper;

namespace Movies.DataAccess;

public class RoleRepository : IRoleRepository
{
    private readonly IDbConnectionFactory _db;

    public RoleRepository(IDbConnectionFactory db)
    {
        _db = db;
    }

    public async ValueTask<int> CreateRoleAsync(Role role, CancellationToken token = default)
    {
        using var conn = await _db.CreateConnectionAsync(token);

        string query = @"INSERT INTO role (name) VALUES (@Name) RETURNING id;";

        int genreid = await conn.QueryFirstOrDefaultAsync<int>(new CommandDefinition(query, role, cancellationToken: token));

        return genreid;
    }

    public async ValueTask<bool> DeleteRoleAsync(int id, CancellationToken token = default)
    {
        using var conn = await _db.CreateConnectionAsync(token);

        string query = @"DELETE FROM role WHERE id = @Id";

        int deletedRows = await conn.ExecuteAsync(new CommandDefinition(query, new { id }, cancellationToken: token));

        if (deletedRows > 0)
            return true;

        return false;
    }

    public async ValueTask<bool> EditRoleAsync(Role role, CancellationToken token = default)
    {
        using var conn = await _db.CreateConnectionAsync(token);

        string query = @"UPDATE ""role"" SET name = @Name 
                        WHERE id = @Id";

        int updatedRows = await conn.ExecuteAsync(new CommandDefinition(query, role, cancellationToken: token));

        if (updatedRows > 0)
            return true;

        return false;
    }

    public async Task<IEnumerable<Role>> GetAllRolesAsync(CancellationToken token = default)
    {
        using var conn = await _db.CreateConnectionAsync(token);

        string query = @"SELECT id as Id, name as Name
                         FROM role ";

        IEnumerable<Role> roles = conn.QueryAsync<Role>(new CommandDefinition(query, token)).Result.ToList();

        return roles;
    }

    public async Task<Role?> GetRoleByIdAsync(int id, CancellationToken token = default)
    {
        using var conn = await _db.CreateConnectionAsync(token);

        string query = @"SELECT id as Id, name as Name
                         FROM role 
                         WHERE id = @Id";

        Role role = await conn.QueryFirstOrDefaultAsync<Role>(new CommandDefinition(query, new { id }, cancellationToken: token));

        return role;
    }
}
