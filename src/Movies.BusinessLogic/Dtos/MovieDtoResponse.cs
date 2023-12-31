﻿
using Movies.DataAccess;
using System.ComponentModel;

namespace Movies.BusinessLogic;

public class MovieDtoResponse
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Slug { get; set; }
    public int YearOfRelease { get; set; }
    public string Video { get; set; }
    public IEnumerable<Genre> Genres { get; set; }
}
