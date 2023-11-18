using FluentValidation;
using Movies.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.BusinessLogic;

public class MovieService : IMovieService
{
    private readonly IMovieRepository _movieRepository;
    private readonly IGenreRepository _genreRepository;
    private readonly IValidator<MovieDto> _validator;
    private readonly IValidator<MovieOptions> _optionsValidator;

    public MovieService(
        IMovieRepository movieRepository, 
        IGenreRepository genreRepository, 
        IValidator<MovieDto> validator, 
        IValidator<MovieOptions> optionsValidator)
    {
        _movieRepository = movieRepository;
        _genreRepository = genreRepository;
        _validator = validator;
        _optionsValidator = optionsValidator;
    }

    public async Task<MovieDtoResponse> CreateMovieAsync(MovieDto movieDto, CancellationToken token = default)
    {
        _validator.Validate(movieDto, opt =>
        {
            opt.ThrowOnFailures();
            opt.IncludeRuleSets("Create");
        });

        Movie movie = movieDto.DtoToMovie();

        int movieId = await _movieRepository.CreateAsync(movie);

        Movie createdMovie = await _movieRepository.GetAsync(movieId, token);

        IEnumerable<int> genreIds = await _movieRepository.GetGenreIdsByMovieId(movieId, token);
        
        if(genreIds.Any())
            createdMovie.Genres = await _movieRepository.GetGenresByIdsAsync(genreIds, token);
        
        return createdMovie.MovieToResponseDto();
    }
   
    public async ValueTask<bool> DeleteMovieAsync(int id, CancellationToken token = default)
    {
        if (id <= 0)
            return false;

        bool isDeleted = await _movieRepository.DeleteAsync(id, token);

        return isDeleted;
    }

    public async Task<MovieDtoResponse?> EditMovieAsync(MovieDto movieDto, CancellationToken token = default)
    {
         _validator.Validate(movieDto, opt =>
        {
            opt.ThrowOnFailures();
            opt.IncludeRuleSets("Edit");
        });

        Movie movie = movieDto.DtoToMovie();

        IEnumerable<int> movieGenreIds = await _movieRepository.GetGenreIdsByMovieId(movie.Id, token);

        bool isUpdated  = await _movieRepository.EditAsync(movie, movieGenreIds);
        
        if(!isUpdated) 
            return null;

        Movie updatedMovie = await _movieRepository.GetAsync(movie.Id);

        IEnumerable<int> genreIds = await _movieRepository.GetGenreIdsByMovieId(updatedMovie.Id, token);

        if (genreIds.Any())
            updatedMovie.Genres = await _movieRepository.GetGenresByIdsAsync(genreIds, token);
        
        return updatedMovie.MovieToResponseDto();
    }

    public async Task<MoviesViewResponseDto> GetAllMoviesAsync(MovieOptionsDto optionDto, CancellationToken token = default)
    {
        MovieOptions options = optionDto.DtoToMovieOptions();

        _optionsValidator.Validate(options, opt => opt.ThrowOnFailures());

        IEnumerable<Movie> movies = await _movieRepository.GetAllAsync(options, token);

        if(movies is null)
            return new();

        int countMovies = await _movieRepository.GetCountMovies(options, token);

        return movies.MoviesToMoviesViewResponseDto(countMovies,options.Page, options.PageSize);
    }

    public async Task<MovieDtoResponse?> GetMovieByIdAsync(int id, CancellationToken token = default)
    {
        Movie movie = await _movieRepository.GetAsync(id, token);

        if(movie is null)
            return null;

        IEnumerable<int> genreIds = await _movieRepository.GetGenreIdsByMovieId(movie.Id, token);

        if (genreIds.Any())
        {
            string ids = string.Join(",", genreIds);
            movie.Genres = await _movieRepository.GetGenresByIdsAsync(genreIds, token);
        }

        return movie.MovieToResponseDto();
    }
    
}
