using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.BusinessLogic;

public readonly struct MovieDto
{
    public int Id { get; init; }
    public string Title { get; init; }
    public int YearOfRelease { get; init; }
   
    public List<int> GenresIds { get; init; }
}
