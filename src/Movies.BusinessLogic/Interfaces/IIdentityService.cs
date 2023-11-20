
namespace Movies.BusinessLogic;

public interface IIdentityService
{
    string GenerateToken(UserDtoResponse? user);
    Task<UserDtoResponse?> GetUserByEmailAndPasswordAsync(LoginDto loginDto, CancellationToken token = default);
    Task<UserDtoResponse> RegistrationUser(RegistrationDto registrationDto, CancellationToken token = default);
}
