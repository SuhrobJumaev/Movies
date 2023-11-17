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

    public async ValueTask<int> CreateGenreAsync(Genre genre, CancellationToken token = default)
    {
        using var conn = await _db.CreateConnectionAsync(token);

        string query = @"INSERT INTO genre (name) VALUES (@Name) RETURNING id;";

        int genreid = await conn.QueryFirstOrDefaultAsync<int>(new CommandDefinition(query, genre, cancellationToken: token));

        return genreid;
    }

    public async ValueTask<bool> DeleteGenreAsync(int id, CancellationToken token = default)
    {
        using var conn = await _db.CreateConnectionAsync(token);

        string query = @"DELETE FROM genre WHERE id = @Id";

        int deletedRows = await conn.ExecuteAsync(new CommandDefinition(query, new { id }, cancellationToken: token));

        if (deletedRows > 0)
            return true;

        return false;
    }

    public async ValueTask<bool> EditGenreAsync(Genre genre, CancellationToken token = default)
    {
        using var conn = await _db.CreateConnectionAsync(token);

        string query = @"UPDATE ""genre"" SET name = @Name 
                        WHERE id = @Id";

        int updatedRows = await conn.ExecuteAsync(new CommandDefinition(query, genre, cancellationToken: token));

        if (updatedRows > 0)
            return true;

        return false;
    }

    public async Task<IEnumerable<Genre>> GetAllGenresAsync(CancellationToken token = default)
    {
        using var conn = await _db.CreateConnectionAsync(token);

        string query = @"SELECT id as Id, name as Name
                         FROM genre ";

        IEnumerable<Genre> genres = conn.QueryAsync<Genre>(new CommandDefinition(query, token)).Result.ToList();

        return genres;
    }

    public async Task<Genre?> GetGenreByIdAsync(int id, CancellationToken token = default)
    {
        using var conn = await _db.CreateConnectionAsync(token);

        string query = @"SELECT id as Id, name as Name
                         FROM genre 
                         WHERE id = @Id";

        Genre genre = await conn.QueryFirstOrDefaultAsync<Genre>(new CommandDefinition(query,new { id}, cancellationToken: token));

        return genre;
    }
}
