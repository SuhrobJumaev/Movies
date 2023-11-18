

namespace Movies.DataAccess;

public class MyUserOptions
{
    public string? Search { get; set; }
    public string? SortField { get; set; }
    public SortOrder? SortOrder { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
}
