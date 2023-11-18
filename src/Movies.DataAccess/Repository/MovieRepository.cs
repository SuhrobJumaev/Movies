
using Dapper;
using Npgsql;

namespace Movies.DataAccess;

public class MovieRepository : IMovieRepository
{
    private readonly IDbConnectionFactory _db;
    public MovieRepository(IDbConnectionFactory db)
    {
        _db = db;
    }

    public async ValueTask<int> CreateAsync(Movie movie, CancellationToken token = default)
    {
        using var conn = await _db.CreateConnectionAsync(token);

        using var transaction = conn.BeginTransaction();

        int movieId = await conn.QueryFirstOrDefaultAsync<int>(new CommandDefinition("""
                INSERT INTO movie (slug, title, year_of_release)
                VALUES (@Slug, @Title, @YearOfRelease) RETURNING id;
            """, movie, cancellationToken: token));

        if(movieId > 0)
        {   
            List<MovieGenre> dataToInsert = new();
            
            NpgsqlConnection npgsqlConnection = (NpgsqlConnection)conn;

            foreach (int genreId in movie.GenresIds)
            {
                dataToInsert.Add(new MovieGenre { movie_id = movieId, genre_id = genreId });
            }
            
            using (var importer = npgsqlConnection.BeginBinaryImport("COPY movie_genre (movie_id, genre_id) FROM STDIN (FORMAT BINARY)"))
            {
                foreach (var item in dataToInsert)
                {
                    importer.StartRow();
                    importer.Write(item.movie_id, NpgsqlTypes.NpgsqlDbType.Integer);
                    importer.Write(item.genre_id, NpgsqlTypes.NpgsqlDbType.Integer);
                }

                importer.Complete();
            } 
        }

        transaction.Commit();

        return movieId;
    }

    public async ValueTask<bool> DeleteAsync(int id, CancellationToken token = default)
    {
        using var conn = await _db.CreateConnectionAsync(token);

        string query = @"DELETE FROM movie WHERE id = @Id";

        int deletedRows = await conn.ExecuteAsync(new CommandDefinition(query, new { id }, cancellationToken: token));

        if (deletedRows > 0)
            return true;

        return false;
    }

    public async ValueTask<bool> EditAsync(Movie movie, IEnumerable<int> genreIdsForDelete, CancellationToken token = default)
    {
        using var conn = await _db.CreateConnectionAsync(token);

        using var transaction = conn.BeginTransaction();

        if(genreIdsForDelete.Any())
        {
            int deleteRow = await conn.ExecuteAsync(new CommandDefinition(
                $@"DELETE FROM movie_genre WHERE movie_id = @MovieId AND genre_id IN({string.Join(',', genreIdsForDelete)})"
                ,new { MovieId = movie.Id},cancellationToken: token));

            if(deleteRow <= 0)           
                return false;
        }

        int updatedRow = await conn.ExecuteAsync(new CommandDefinition("""
                UPDATE movie SET title = @Title, slug = @Slug, year_of_release = @YearOfRelease WHERE id = @Id;
            """, movie, cancellationToken: token));

        if (updatedRow <= 0)
        {
            transaction.Rollback();
            return false;
        }
            
        List<MovieGenre> dataToInsert = new();

        NpgsqlConnection npgsqlConnection = (NpgsqlConnection)conn;

        foreach (int genreId in movie.GenresIds)
        {
            dataToInsert.Add(new MovieGenre { movie_id = movie.Id, genre_id = genreId });
        }

        using (var importer = npgsqlConnection.BeginBinaryImport("COPY movie_genre (movie_id, genre_id) FROM STDIN (FORMAT BINARY)"))
        {
            foreach (var item in dataToInsert)
            {
                importer.StartRow();
                importer.Write(item.movie_id, NpgsqlTypes.NpgsqlDbType.Integer);
                importer.Write(item.genre_id, NpgsqlTypes.NpgsqlDbType.Integer);
            }

            importer.Complete();
        }

        transaction.Commit();

        return updatedRow > 0;
    }

    public async Task<IEnumerable<Movie>> GetAllAsync(MovieOptions options, CancellationToken token = default)
    {
        using var conn = await _db.CreateConnectionAsync(token);

        options.SortField = (options.SortField == "year") ? "year_of_release" : options.SortField;

        string orderClause = $"ORDER BY {options.SortField} {(options.SortOrder == SortOrder.Ascending? "ASC" : "DESC")}";

        string query = $@"SELECT id as Id, year_of_release as YearOfRelease, title as Title, slug as Slug
                        FROM movie
                        WHERE (@Title IS NULL OR title ILIKE ('%' || @Title || '%')) 
                        AND  (@Year IS NULL OR year_of_release = @Year)
                        {orderClause}
                        LIMIT @PageSize
                        OFFSET @Offset";

        IEnumerable<Movie> movies = conn.QueryAsync<Movie>(new CommandDefinition(query, new 
        { 
            Title = options.Title,
            Year = options.Year,
            PageSize = options.PageSize,
            Offset = (options.Page - 1) * options.PageSize
        }, 
        cancellationToken: token)).Result.ToList();
        
        return movies;
    }

    public async Task<IEnumerable<int>> GetGenreIdsByMovieId(int movieId, CancellationToken token = default)
    {
        using var conn = await _db.CreateConnectionAsync();

        string query = @"SELECT genre_id FROM movie_genre WHERE movie_id = @Id";

        List<int> genreIds = conn.QueryAsync<int>(new CommandDefinition(query, new { Id = movieId }, cancellationToken: token)).Result.ToList();

        return genreIds;
    }

    public async Task<IEnumerable<Genre>> GetGenresByIdsAsync(IEnumerable<int> genderIds,CancellationToken token = default)
    {
        using var conn = await _db.CreateConnectionAsync();

        IEnumerable<Genre> genres = Enumerable.Empty<Genre>();
       
        string query = $@"SELECT id as Id, name as Name 
                                FROM genre 
                                WHERE id IN ({string.Join(',', genderIds)})";

            genres = conn.QueryAsync<Genre>(new CommandDefinition(query, new { }, cancellationToken: token)).Result.ToList();
   
        return genres;
    }

    public async Task<Movie> GetAsync(int id, CancellationToken token = default)
    {
        using var conn = await _db.CreateConnectionAsync(token);

        string query = @"SELECT id as Id, year_of_release as YearOfRelease, title as Title, slug as Slug
                        FROM movie 
                        WHERE id = @Id";

        Movie movie =  await conn.QueryFirstOrDefaultAsync<Movie>(new CommandDefinition(query, new { Id  = id}, cancellationToken: token));

        return movie;
    }

    public async Task<bool> DeleteGenreFormMovieAsync(int movieId, List<int> genreIds, CancellationToken token = default)
    {
         using var conn = await _db.CreateConnectionAsync(token);

        string query = $@"DELETE FROM movie_genre WHERE movie_id = @MovieId AND genre_id IN({string.Join(',', genreIds)})";

        int deletedRows = await conn.ExecuteAsync(new CommandDefinition(query, new { MovieId = movieId }, cancellationToken: token));

        if (deletedRows > 0)
            return true;

        return false;
    }

    public async ValueTask<int> GetCountMovies(MovieOptions options, CancellationToken token = default)
    {
        using var conn = await _db.CreateConnectionAsync(token);

        string query = $@"SELECT COUNT(id)
                        FROM movie
                        WHERE (@Title IS NULL OR title ILIKE ('%' || @Title || '%')) 
                        AND  (@Year IS NULL OR year_of_release = @Year)";

        int countMovies = await conn.QueryFirstOrDefaultAsync<int>(new CommandDefinition(query, new {
            Title = options.Title,
            Year = options.Year,
        },
        cancellationToken: token));

        return countMovies;
    }
}
