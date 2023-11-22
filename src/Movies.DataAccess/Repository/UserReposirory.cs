
using System.Data.Common;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Threading;
using Dapper;

namespace Movies.DataAccess;

public sealed class UserRepository : IUserRepository
{
     private readonly IDbConnectionFactory _db;
    
    public UserRepository(IDbConnectionFactory db)
    {
        _db = db;
    }

    public async ValueTask<int> CreateAsync(User user, CancellationToken token = default)
    {
        using var conn  = await  _db.CreateConnectionAsync(token);

        string query = @"INSERT INTO ""user"" (name, last_name, age, gender, phone, email, password, role_id)
                                       VALUES (@Name, @LastName, @Age, @Gender, @Phone, @Email, @Password, @RoleId) RETURNING id;";

        int userid = await conn.QueryFirstOrDefaultAsync<int>(new CommandDefinition(query, user, cancellationToken: token));

        return userid;
    }

    public async ValueTask<bool> DeleteAsync(int id, CancellationToken token = default)
    {
        using var conn = await _db.CreateConnectionAsync(token);

        string query = @"DELETE FROM ""user"" WHERE id = @Id";

        int deletedRows = await conn.ExecuteAsync(new CommandDefinition(query, new { id }, cancellationToken: token));

        if(deletedRows > 0)
            return true;

        return false;
    }

    public async ValueTask<bool> EditAsync(User user, CancellationToken token = default)
    {
        using var conn = await _db.CreateConnectionAsync(token);

        string query = @"UPDATE ""user"" SET name = @Name, last_name = @LastName, age = @Age, gender = @Gender, phone = @Phone, role_id = @RoleId 
                        WHERE id = @Id";

        int updatedRows = await conn.ExecuteAsync(new CommandDefinition( query, user, cancellationToken: token) );

        if (updatedRows > 0)
            return true;

        return false;
    }

    public async Task<IEnumerable<User>> GetAllAsync(MyUserOptions options, CancellationToken token = default)
    {
        using var conn = await _db.CreateConnectionAsync(token);

        string orderClause = $"ORDER BY U.{options.SortField} {(options.SortOrder == SortOrder.Ascending ? "ASC" : "DESC")}";
        
        string query = $@"SELECT U.id as Id, U.name as Name, U.last_name as LastName, U.age as Age, U.role_id as RoleId,
                         U.gender as Gender, U.phone as Phone, U.email as Email, R.name as RoleName
                         FROM ""user"" as U
                         JOIN role as R ON R.id = U.role_id
                         WHERE (@Search IS NULL OR (CONCAT(U.name,'',U.last_name) ILIKE ('%' || @Search || '%' ) OR CONCAT(U.last_name,'',U.name) ILIKE ('%' || @Search || '%' ) ))
                         {orderClause}
                         LIMIT @PageSize
                         OFFSET @Offset";

        IEnumerable<User> users = conn.QueryAsync<User>(new CommandDefinition(query,new 
        { 
            Search = options.Search?.Replace(" ", ""),
            PageSize = options.PageSize,
            Offset = (options.Page - 1) * options.PageSize
        }, cancellationToken: token)).Result.ToList();

        return users;
    }

    public async Task<User> GetAsync(int id, CancellationToken token = default)
    {
        using var conn = await _db.CreateConnectionAsync(token);

        string query = @"SELECT U.id as Id, U.name as Name, U.last_name as LastName, U.age as Age, U.role_id as RoleId,
                         U.gender as Gender, U.phone as Phone, U.email as Email, R.name as RoleName
                        FROM ""user"" as U
                        JOIN role as R ON R.id = U.role_id
                        WHERE U.id = @Id";

        User user = await conn.QueryFirstOrDefaultAsync<User>(new CommandDefinition(query,  new {Id = id}, cancellationToken: token));

        return user;
    }

    public async Task<User> GetUserByEmailAsync(string email, CancellationToken token = default)
    {
        using var conn = await _db.CreateConnectionAsync(token);

        string query = @"SELECT id as Id, email as Email, name as Name, last_name as LastName  FROM ""user"" WHERE email = @Email";

        User user = await conn.QueryFirstOrDefaultAsync<User>(new CommandDefinition(query, new { Email = email }, cancellationToken: token));

        return user;
    }

    public async Task<User> GetUserByEmailAndPasswordAsync(string email, string password, CancellationToken token = default)
    {
        using var conn = await _db.CreateConnectionAsync(token);

        string query = @"SELECT U.id as Id, U.name as Name, U.last_name as LastName, U.age as Age, U.role_id as RoleId,
                         U.gender as Gender, U.phone as Phone, U.email as Email, R.name as RoleName
                        FROM ""user"" as U
                        JOIN role as R ON R.id = U.role_id
                        WHERE U.email = @Email AND U.password = @Password";

        User user = await conn.QueryFirstOrDefaultAsync<User>(new CommandDefinition(query, new {Email = email, Password = password}, cancellationToken: token));

        return user;
    }

    public async ValueTask<bool> EditProfileAsync(User user, CancellationToken token = default)
    {
        using var conn = await _db.CreateConnectionAsync(token);

        string query = @"UPDATE ""user"" SET name = @Name, last_name = @LastName, age = @Age, gender = @Gender, phone = @Phone 
                        WHERE id = @Id";

        int updatedRows = await conn.ExecuteAsync(new CommandDefinition(query, user, cancellationToken: token));

        if (updatedRows > 0)
            return true;

        return false;
    }

    public async ValueTask<bool> ChangeUserPasswordAsync(int? userId, string newPassword, CancellationToken token = default)
    {
        using var conn = await _db.CreateConnectionAsync(token);

        string query = @"UPDATE ""user"" SET password = @NewPassword 
                        WHERE id = @Id";

        int updatedRows = await conn.ExecuteAsync(new CommandDefinition(query, new {Id = userId, NewPassword = newPassword}, cancellationToken: token));

        if (updatedRows > 0)
            return true;

        return false;
    }

    public async ValueTask<int> GetCountUsers(MyUserOptions options, CancellationToken token = default)
    {
        using var conn = await _db.CreateConnectionAsync(token);

        string query = $@"SELECT COUNT(U.id)
                         FROM ""user"" as U
                         JOIN role as R ON R.id = U.role_id
                         WHERE (@Search IS NULL OR (CONCAT(U.name,'',U.last_name) ILIKE ('%' || @Search || '%' ) OR CONCAT(U.last_name,'',U.name) ILIKE ('%' || @Search || '%' ) ))";

        int  countUsers = await conn.QueryFirstOrDefaultAsync<int>(new CommandDefinition(query, new
        {
            Search = options.Search?.Replace(" ", ""),
        }, cancellationToken: token));

        return countUsers;
    }
}