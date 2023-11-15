using System.Data;
using Npgsql;

namespace Movies.DataAccess;

public class NpgsqlConnectionFactory : IDbConnectionFactory
{
    private readonly string _sqlConnString;

    public NpgsqlConnectionFactory(string sqlConnString)
    {
        _sqlConnString = sqlConnString;
    }

    public async Task<IDbConnection> CreateConnectionAsync(CancellationToken token = default)
    {
        var conn = new NpgsqlConnection(_sqlConnString);
        await conn.OpenAsync();
        return conn;
    }
}