using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.BusinessLogic;

public class  MovieDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public int YearOfRelease { get; set; }
    public IFormFile Video { get; set; }
    public string? VideoName { get; set; } = string.Empty;
    public List<int> GenresIds { get; set; }
}
