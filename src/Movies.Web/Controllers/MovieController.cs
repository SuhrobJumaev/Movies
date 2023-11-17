using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Movies.BusinessLogic;

namespace Movies.Web;

[ApiController]
[Route("api/movies")]
[Authorize(Roles = Utils.AdminRole)]
public class MovieController : ControllerBase
{
    private readonly IMovieService _movieService;
    public MovieController(IMovieService movieService)
    {
        _movieService = movieService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateMovieAsync([FromBody] MovieDto movieDto, CancellationToken token)
    {
        MovieDtoResponse createdMovie = await _movieService.CreateMovieAsync(movieDto);

        return CreatedAtAction("GetMovie", new { id = createdMovie.Id},createdMovie);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllMoviesAsync(CancellationToken token)
    {
        IEnumerable<MovieDtoResponse> movies = await _movieService.GetAllMoviesAsync(token);

        return Ok(movies);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetMovieAsync(int id, CancellationToken token)
    {
        MovieDtoResponse? movie = await _movieService.GetMovieByIdAsync(id, token);

        if(movie is null)
            return NotFound();

        return Ok(movie);
    }
    
    [HttpPut]
    public async Task<IActionResult> EditMovieAsync([FromBody] MovieDto movieDto, CancellationToken token)
    {
        MovieDtoResponse? updatedMovie = await _movieService.EditMovieAsync(movieDto, token);

        if(updatedMovie is null)
            return NotFound();

        return Ok(updatedMovie);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMovieAsync(int id, CancellationToken token)
    {
        bool isDeleted = await _movieService.DeleteMovieAsync(id,token);

        if(!isDeleted)
            return NotFound();

        return Ok();
    }
}
