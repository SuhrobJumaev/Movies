
using Microsoft.Extensions.Options;
using Movies.DataAccess;
using System.Runtime.CompilerServices;

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

    public static MovieOptions DtoToMovieOptions(this MovieOptionsDto optionsDto)
    {
        return new()
        {
            Title = optionsDto.Title,
            Year = optionsDto.Year,
            SortField = optionsDto.SortBy is null ? "id" : optionsDto.SortBy.Trim('-') ,
            SortOrder = optionsDto.SortBy is null ? SortOrder.Descending :
                optionsDto.SortBy.StartsWith('-') ? SortOrder.Descending : SortOrder.Ascending,
            Page = optionsDto.Page,
            PageSize =  optionsDto.PageSize,
        };
    }

    public static MoviesViewResponseDto MoviesToMoviesViewResponseDto(this IEnumerable<Movie> movies, int countMovies, int currentPage, int pageSize)
    {
        return new()
        {
            CurrentPage = currentPage,
            PageSize = pageSize,
            CountPage = (int)Math.Ceiling(countMovies / (decimal)pageSize),
            Movies = movies.MoviesToResponseDto(),
        };
    }
}
