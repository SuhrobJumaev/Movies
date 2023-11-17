
using Movies.DataAccess;

namespace Movies.BusinessLogic;

public static class MovieMapper
{
    public static Movie DtoToMovie(this MovieDto movieDto)
    {
        return new()
        {
            Id = movieDto.Id,
            Title = movieDto.Title,
            YearOfRelease = movieDto.YearOfRelease,
            GenresIds = movieDto.GenresIds,
        };
    }

    public static IEnumerable<MovieDtoResponse> MoviesToResponseDto(this IEnumerable<Movie> movies)
    {
        IEnumerable<MovieDtoResponse> moviesDto = Enumerable.Empty<MovieDtoResponse>();

        moviesDto = movies.Select(m => new MovieDtoResponse
        {
            Id = m.Id,
            Title = m.Title,
            Slug = m.Slug,
            YearOfRelease = m.YearOfRelease,

        }).ToList();

        return moviesDto;
    }

    public static MovieDtoResponse MovieToResponseDto(this Movie movie)
    {
        return new ()
        {
            Id = movie.Id,
            Title = movie.Title,
            Slug = movie.Slug,
            YearOfRelease = movie.YearOfRelease,
            Genres = movie.Genres,
        };
    }
}
