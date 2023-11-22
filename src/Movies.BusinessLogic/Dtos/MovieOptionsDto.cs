
using System.ComponentModel;

namespace Movies.BusinessLogic;

public class MovieOptionsDto
{
    private int _page;
    private int _pageSize;
    
      
    public string? Title { get; set; } = null;
    public int? Year { get; set; } = null;
    public string? SortBy { get; set; } = null;
    
    public int Page 
    { 
        get => _page;
        set => _page = value == 0 ? 1 : value; 
    }

    public int PageSize 
    {   
        get => _pageSize; 
        set => _pageSize = value == 0 ? 10 : value; 
    }
}

