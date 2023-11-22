
namespace Movies.BusinessLogic;

public  struct RegistrationDto
{
    public string Name { get; init; }
    public string LastName { get; init; }
    public short Age { get; init; }
    public short Gender { get; init; }
    public string Phone { get; init; }
    public string Email { get; init; }
    public string Password { get; set; }
    public string ConfirmPassword { get; init; }
}
