using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.DataAccess;

public interface IGenreRepository
{
    ValueTask<int> CreateGenreAsync(Genre genre, CancellationToken token = default);
    Task<Genre?> GetGenreByIdAsync(int id, CancellationToken token = default);
    Task<IEnumerable<Genre>> GetAllGenresAsync(CancellationToken token = default);
    ValueTask<bool> EditGenreAsync(Genre genre, CancellationToken token = default);
    ValueTask<bool> DeleteGenreAsync(int id, CancellationToken token = default);
}
