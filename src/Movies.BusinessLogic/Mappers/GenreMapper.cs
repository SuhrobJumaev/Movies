
using Movies.DataAccess;

namespace Movies.BusinessLogic;

public static class GenreMapper
{
    public static Genre DtoToGenre(this GenreDto genreDto)
    {
        return new()
        {
            Id = genreDto.Id,
            Name = genreDto.Name,
        };
    }

    public static IEnumerable<GenreDto> GenresToDto(this IEnumerable<Genre> genres)
    {
        IEnumerable<GenreDto> genresDto = Enumerable.Empty<GenreDto>();

        genresDto = genres.Select(g => new GenreDto
        {
            Id = g.Id,
            Name = g.Name,

        }).ToList();

        return genresDto;
    }

    public static GenreDto GenreToDto(this Genre genre)
    {
        return new()
        {
            Id = genre.Id,
            Name = genre.Name,
        };  
    }
}
