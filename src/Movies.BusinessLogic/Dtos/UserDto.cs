namespace Movies.BusinessLogic;

public readonly struct UserDto
{
    public int Id { get; init; }
    public string Name { get; init; }
    public string LastName { get; init; }
    public short Age { get; init;}
    public short Gender { get; init; }
    public string Phone { get; init; }
    public string Email { get; init; }
    public short RoleId { get; init; }
    public string Password { get; init; }
}