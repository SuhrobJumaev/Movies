
namespace Movies.BusinessLogic;

public struct UsersViewResponseDto
{
    public int CurrentPage { get; set; }
    public int CountPage { get; set; }
    public int PageSize { get; set; }
    public IEnumerable<UserDtoResponse> Users { get; set; }
}
