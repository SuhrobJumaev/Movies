
namespace Movies.DataAccess;

public interface IMovieRepository
{
    ValueTask<int> CreateAsync(Movie movie, CancellationToken token = default);
    ValueTask<bool> EditAsync(Movie movie, IEnumerable<int> genreIdsForDelete,CancellationToken token = default);
    Task<Movie> GetAsync(int id, CancellationToken token = default);
    Task<IEnumerable<Movie>> GetAllAsync(MovieOptions options, CancellationToken token = default);
    ValueTask<int> GetCountMovies(MovieOptions options, CancellationToken token = default);
    ValueTask<bool> DeleteAsync(int id, CancellationToken token = default);
    Task<IEnumerable<int>> GetGenreIdsByMovieId(int movieId, CancellationToken token = default);
    Task<IEnumerable<Genre>> GetGenresByIdsAsync(IEnumerable<int> genreIds, CancellationToken token = default);
    Task<bool> DeleteGenreFormMovieAsync(int movieId, List<int> genreIds, CancellationToken token = default);

}
