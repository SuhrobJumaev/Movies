using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Movies.BusinessLogic;
namespace Movies.Web;

[Authorize]
[ApiController]
[Route("api/movies")]
[Asp.Versioning.ApiVersion(Utils.API_VERSION_1)]
public class MovieController : ControllerBase
{
    private readonly IMovieService _movieService;
    public MovieController(IMovieService movieService)
    {
        _movieService = movieService;
    }

    [Authorize(Utils.AdminRole)]
    [HttpPost]
    [ProducesResponseType(typeof(MovieDtoResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationFailureResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateMovieAsync([FromBody] MovieDto movieDto, CancellationToken token)
    {
        MovieDtoResponse createdMovie = await _movieService.CreateMovieAsync(movieDto);

        return CreatedAtAction("GetMovie", new { id = createdMovie.Id},createdMovie);
    }

    [HttpGet]
    [ProducesResponseType(typeof(MoviesViewResponseDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllMoviesAsync([FromQuery] MovieOptionsDto optionsDto, CancellationToken token)
    {
        MoviesViewResponseDto movies = await _movieService.GetAllMoviesAsync(optionsDto,token);

        return Ok(movies);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(MovieDtoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMovieAsync(int id, CancellationToken token)
    {
        MovieDtoResponse? movie = await _movieService.GetMovieByIdAsync(id, token);

        if(movie is null)
            return NotFound();

        return Ok(movie);
    }

    [Authorize(Utils.AdminRole)]
    [HttpPut]
    [ProducesResponseType(typeof(MovieDtoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ValidationFailureResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> EditMovieAsync([FromBody] MovieDto movieDto, CancellationToken token)
    {
        MovieDtoResponse? updatedMovie = await _movieService.EditMovieAsync(movieDto, token);

        if(updatedMovie is null)
            return NotFound();

        return Ok(updatedMovie);
    }

    [Authorize(Utils.AdminRole)]
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteMovieAsync(int id, CancellationToken token)
    {
        bool isDeleted = await _movieService.DeleteMovieAsync(id,token);

        if(!isDeleted)
            return NotFound();

        return Ok();
    }
}
