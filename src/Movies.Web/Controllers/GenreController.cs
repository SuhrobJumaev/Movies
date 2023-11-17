using Microsoft.AspNetCore.Mvc;
using Movies.BusinessLogic;

namespace Movies.Web;

[ApiController]
[Route("api/genres")]
public class GenreController : ControllerBase
{
    private IGenreService _genreService;

    public GenreController(IGenreService genreService)
    {
        _genreService = genreService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateGenreAsync([FromBody] GenreDto genreDto, CancellationToken token)
    {
        GenreDto createdGenre = await _genreService.CreateGenreAsync(genreDto);

        return CreatedAtAction("GetGenre", new { id = createdGenre.Id }, createdGenre);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllGenresAsync(CancellationToken token)
    {
        IEnumerable<GenreDto> genres = await _genreService.GetAllGenresAsync(token);

        return Ok(genres);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetGenreAsync(int id, CancellationToken token)
    {
        GenreDto? genre = await _genreService.GetGenreByIdAsync(id, token);

        if (genre is null)
            return NotFound();

        return Ok(genre);
    }

    [HttpPut]
    public async Task<IActionResult> EditGenreAsync([FromBody] GenreDto genreDto, CancellationToken token)
    {
        GenreDto? updatedGenre = await _genreService.EditGenreAsync(genreDto, token);

        if (updatedGenre is null)
            return NotFound();

        return Ok(updatedGenre);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteGenreAsync(int id, CancellationToken token)
    {
        bool isDeleted = await _genreService.DeleteGenreAsync(id, token);

        if (!isDeleted)
            return NotFound();

        return Ok();
    }
}
