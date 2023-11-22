
namespace Movies.BusinessLogic;

public interface IMovieService
{
    Task<MovieDtoResponse> CreateMovieAsync(MovieDto movieDto, CancellationToken token = default);
    Task<MovieDtoResponse?> GetMovieByIdAsync(int id, CancellationToken token = default);
    Task<MoviesViewResponseDto> GetAllMoviesAsync(MovieOptionsDto optionDto, CancellationToken token = default);
    Task<MovieDtoResponse?> EditMovieAsync(MovieDto movieDto, CancellationToken token = default);
    ValueTask<bool> DeleteMovieAsync(int id, CancellationToken token = default);
    IAsyncEnumerable<byte[]> StreamVideoAsync(string videoName, CancellationToken token = default);
}
