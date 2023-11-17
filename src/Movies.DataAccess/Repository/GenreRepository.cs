using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.DataAccess;

public class GenreRepository : IGenreRepository
{
    private readonly IDbConnectionFactory _db;

    public GenreRepository(IDbConnectionFactory db)
    {
        _db = db;
    }

    public async Task<List<int>> GetAllGenreIdsAsync(CancellationToken token = default)
    {
        using var conn = await _db.CreateConnectionAsync();

        string query = @"SELECT id FROM genre";

        List<int> genreIds = conn.QueryAsync<int>(new CommandDefinition(query, token)).Result.ToList();

        return genreIds;
    }
}
