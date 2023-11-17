using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.DataAccess;

public interface IGenreRepository
{
    Task<List<int>> GetAllGenreIdsAsync(CancellationToken token = default);
}
