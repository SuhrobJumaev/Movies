namespace Movies.DataAccess;

public sealed class User 
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public short Age { get; init; }
    public Gender Gender { get; init; }
    public string Phone { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public short RoleId { get; set; }
    public string RoleName { get; set; } = string.Empty;
} 