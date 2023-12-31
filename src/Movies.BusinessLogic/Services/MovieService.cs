﻿using FluentValidation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Movies.DataAccess;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Movies.BusinessLogic;

public class MovieService : IMovieService
{
    private readonly IMovieRepository _movieRepository;
    private readonly IGenreRepository _genreRepository;
    private readonly IValidator<MovieDto> _validator;
    private readonly IValidator<MovieOptions> _optionsValidator;
    private  readonly IVideoService _videoService;
    private readonly string _pathToWwwRoot;

    public MovieService(
        IMovieRepository movieRepository, 
        IGenreRepository genreRepository, 
        IValidator<MovieDto> validator, 
        IValidator<MovieOptions> optionsValidator,
        IVideoService videoService,
        IWebHostEnvironment webHostEnvironment)
    {
        _movieRepository = movieRepository;
        _genreRepository = genreRepository;
        _validator = validator;
        _optionsValidator = optionsValidator;
        _videoService = videoService;
        _pathToWwwRoot = webHostEnvironment.WebRootPath;

    }

    public async Task<MovieDtoResponse> CreateMovieAsync(MovieDto movieDto, CancellationToken token = default)
    {
        _validator.Validate(movieDto, opt =>
        {
            opt.ThrowOnFailures();
            opt.IncludeRuleSets(Utils.CreateRuleSetName);
        });

        string videoName = await _videoService.SaveVideoAsync(movieDto.Video, token);
        
        movieDto.VideoName = videoName;
        
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
        
        Movie existsMovie = await _movieRepository.GetAsync(id);

        bool isDeleted = await _movieRepository.DeleteAsync(id, token);

        if (isDeleted)
        {
            _videoService.DeleteVideo(existsMovie.VideoName);
        }

        return isDeleted;
    }

    public async Task<MovieDtoResponse?> EditMovieAsync(MovieDto movieDto, CancellationToken token = default)
    {
         _validator.Validate(movieDto, opt =>
        {
            opt.ThrowOnFailures();
            opt.IncludeRuleSets(Utils.EditRuleSetName);
        });

        

        Movie existsMovie = await _movieRepository.GetAsync(movieDto.Id);

        if (movieDto.Video is not null)
        {   
            _videoService.DeleteVideo(existsMovie.VideoName);

            string videoName = await _videoService.SaveVideoAsync(movieDto.Video, token);
            movieDto.VideoName = videoName;
        }
        else
        {
            movieDto.VideoName = existsMovie.VideoName;
        }

        Movie movie = movieDto.DtoToMovie();

        IEnumerable<int> movieGenreIds = await _movieRepository.GetGenreIdsByMovieId(movie.Id, token);

        bool isUpdated  = await _movieRepository.EditAsync(movie, movieGenreIds , token);
        
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

    public async IAsyncEnumerable<byte[]> StreamVideoAsync(string videoName, [EnumeratorCancellation] CancellationToken token = default)
    {
        using var fileStream = new FileStream(_pathToWwwRoot + "/" + videoName, FileMode.Open, FileAccess.Read);
        
        var buffer = new byte[Utils.ChunkSize];
        int bytesRead;

        while ((bytesRead = await fileStream.ReadAsync(buffer, 0, buffer.Length, token)) > 0)
        {
            var chunk = new byte[bytesRead];
            Array.Copy(buffer, chunk, bytesRead);
            yield return chunk;

            await Task.Delay(100);
        }
    }

}
