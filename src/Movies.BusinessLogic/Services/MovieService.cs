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

    public MovieService(IMovieRepository movieRepository, IGenreRepository genreRepository, IValidator<MovieDto> validator)
    {
        _movieRepository = movieRepository;
        _genreRepository = genreRepository;
        _validator = validator;
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

  
    public async Task<IEnumerable<MovieDtoResponse>> GetAllMoviesAsync(CancellationToken token = default)
    {
        IEnumerable<Movie> movies = await _movieRepository.GetAllAsync(token);

        if(movies is null)
            return Enumerable.Empty<MovieDtoResponse>();

        return movies.MoviesToResponseDto();
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
