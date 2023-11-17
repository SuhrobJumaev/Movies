
using System.Text.RegularExpressions;

namespace Movies.DataAccess;

public partial class Movie
{
    public int Id { get; set; }
    public string Title { get; set; } =  string.Empty;
    public int YearOfRelease { get; set; }
    public List<int> GenresIds { get; set; }
    public IEnumerable<Genre> Genres { get; set; }
    public string Slug => GenerateSlug();

    private string GenerateSlug()
    {
        var sluggedTitle = SlugRegex().Replace(Title, string.Empty)
            .ToLower().Replace(" ", "-");
        return $"{sluggedTitle}-{YearOfRelease}";
    }

    [GeneratedRegex("[^0-9A-Za-z _-]", RegexOptions.NonBacktracking, 5)]
    private static partial Regex SlugRegex();
}
