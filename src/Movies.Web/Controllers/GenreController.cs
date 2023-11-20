using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Movies.BusinessLogic;
namespace Movies.Web;

[ApiController]
[Route("api/genres")]
[Asp.Versioning.ApiVersion(Utils.API_VERSION_1)]
[Authorize(Utils.AdminRole)]
public class GenreController : ControllerBase
{
    private IGenreService _genreService;

    public GenreController(IGenreService genreService)
    {
        _genreService = genreService;
    }
   
    [HttpPost]
    [ProducesResponseType(typeof(GenreDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationFailureResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateGenreAsync([FromBody] GenreDto genreDto, CancellationToken token)
    {
        GenreDto createdGenre = await _genreService.CreateGenreAsync(genreDto);

        return CreatedAtAction("GetGenre", new { id = createdGenre.Id }, createdGenre);
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<GenreDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllGenresAsync(CancellationToken token)
    {
        IEnumerable<GenreDto> genres = await _genreService.GetAllGenresAsync(token);

        return Ok(genres);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(GenreDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetGenreAsync(int id, CancellationToken token)
    {
        GenreDto? genre = await _genreService.GetGenreByIdAsync(id, token);

        if (genre is null)
            return NotFound();

        return Ok(genre);
    }

    [HttpPut]
    [ProducesResponseType(typeof(GenreDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ValidationFailureResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> EditGenreAsync([FromBody] GenreDto genreDto, CancellationToken token)
    {
        GenreDto? updatedGenre = await _genreService.EditGenreAsync(genreDto, token);

        if (updatedGenre is null)
            return NotFound();

        return Ok(updatedGenre);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteGenreAsync(int id, CancellationToken token)
    {
        bool isDeleted = await _genreService.DeleteGenreAsync(id, token);

        if (!isDeleted)
            return NotFound();

        return Ok();
    }
}
