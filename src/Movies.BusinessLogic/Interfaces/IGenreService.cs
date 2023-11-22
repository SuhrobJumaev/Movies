
namespace Movies.BusinessLogic;

public interface IGenreService
{
    Task<GenreDto> CreateGenreAsync(GenreDto genreDto, CancellationToken token = default);
    Task<GenreDto?> GetGenreByIdAsync(int id, CancellationToken token = default);
    Task<IEnumerable<GenreDto>> GetAllGenresAsync(CancellationToken token = default);
    Task<GenreDto?> EditGenreAsync(GenreDto genreDto, CancellationToken token = default);
    ValueTask<bool> DeleteGenreAsync(int id, CancellationToken token = default);

}
