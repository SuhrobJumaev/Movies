using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Movies.BusinessLogic;
using System.Runtime.CompilerServices;

namespace Movies.Web;

[Authorize]
[ApiController]
[Route("api/movies")]
[ApiVersion(Utils.API_VERSION_1)]
public class MovieController : ControllerBase
{
    private readonly IMovieService _movieService;
    public MovieController(IMovieService movieService)
    {
        _movieService = movieService;
    }

    [Authorize(Roles = Utils.AdminRole)]
    [HttpPost]
    [ProducesResponseType(typeof(MovieDtoResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationFailureResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateMovieAsync([FromForm] MovieDto movieDto, CancellationToken token = default)
    {
        MovieDtoResponse createdMovie = await _movieService.CreateMovieAsync(movieDto, token);

        return CreatedAtAction("GetMovie", new { id = createdMovie.Id},createdMovie);
    }

    [HttpGet]
    [ProducesResponseType(typeof(MoviesViewResponseDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllMoviesAsync([FromQuery] MovieOptionsDto optionsDto, CancellationToken token = default)
    {
        MoviesViewResponseDto movies = await _movieService.GetAllMoviesAsync(optionsDto,token);

        return Ok(movies);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(MovieDtoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMovieAsync(int id, CancellationToken token = default)
    {
        MovieDtoResponse? movie = await _movieService.GetMovieByIdAsync(id, token);

        if(movie is null)
            return NotFound();

        return Ok(movie);
    }

    [HttpGet("stream-movie/{videoName}")]
    [ProducesResponseType(typeof(IAsyncEnumerable<byte[]>), StatusCodes.Status200OK)]
    [AllowAnonymous]
    public async IAsyncEnumerable<byte[]> SrteamVideo(string videoName, [EnumeratorCancellation] CancellationToken token = default)
    {
        await foreach (var chunk in _movieService.StreamVideoAsync(videoName).WithCancellation(token))
            yield return chunk;
    }

    [Authorize(Roles = Utils.AdminRole)]
    [HttpPut]
    [ProducesResponseType(typeof(MovieDtoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ValidationFailureResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> EditMovieAsync([FromForm] MovieDto movieDto, CancellationToken token = default)
    {
        MovieDtoResponse? updatedMovie = await _movieService.EditMovieAsync(movieDto, token);

        if(updatedMovie is null)
            return NotFound();

        return Ok(updatedMovie);
    }

    [Authorize(Roles = Utils.AdminRole)]
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteMovieAsync(int id, CancellationToken token = default)
    {
        bool isDeleted = await _movieService.DeleteMovieAsync(id,token);

        if(!isDeleted)
            return NotFound();

        return Ok();
    }
}
