using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.BusinessLogic;

public struct MoviesViewResponseDto
{
    public int CurrentPage { get; set; }
    public int CountPage { get; set; }
    public int PageSize { get; set; }
    public IEnumerable<MovieDtoResponse> Movies {get;set;}
}
