
using FluentValidation;
using FluentValidation.Results;
using Movies.DataAccess;

namespace Movies.BusinessLogic;

public class GenreService : IGenreService
{
    private readonly IGenreRepository _genreRepository;
    private readonly IValidator<GenreDto> _validator;

    public GenreService(IGenreRepository genreRepository, IValidator<GenreDto> validator)
    {
        _genreRepository = genreRepository;
        _validator = validator;

    }

    public async Task<GenreDto> CreateGenreAsync(GenreDto genreDto, CancellationToken token = default)
    {
        _validator.Validate(genreDto, opt =>
        {
            opt.ThrowOnFailures();
            opt.IncludeRuleSets("Create");
        });

        Genre genre = genreDto.DtoToGenre();

        int genreId = await _genreRepository.CreateGenreAsync(genre, token);

        return genreDto with { Id = genreId };
    }

    public async ValueTask<bool> DeleteGenreAsync(int id, CancellationToken token = default)
    {
        if (id <= 0)
            return false;

        bool isDeleted = await _genreRepository.DeleteGenreAsync(id, token);

        return isDeleted;
    }

    public async Task<GenreDto?> EditGenreAsync(GenreDto genreDto, CancellationToken token = default)
    {
        _validator.Validate(genreDto, opt =>
        {
            opt.ThrowOnFailures();
            opt.IncludeRuleSets("Edit");
        });

        Genre genre = genreDto.DtoToGenre();

        bool isUpdate = await _genreRepository.EditGenreAsync(genre, token);

        if(!isUpdate)
            return null;

        return genreDto;
    }

    public async Task<IEnumerable<GenreDto>> GetAllGenresAsync(CancellationToken token = default)
    {
        IEnumerable<Genre> genres = await _genreRepository.GetAllGenresAsync(token);

        if(genres is null)
            return Enumerable.Empty<GenreDto>();

        return genres.GenresToDto();
    }

    public async Task<GenreDto?> GetGenreByIdAsync(int id, CancellationToken token = default)
    {
        Genre genre = await _genreRepository.GetGenreByIdAsync(id, token);

        if(genre is null)
            return null;

        return genre.GenreToDto();
    }
}
