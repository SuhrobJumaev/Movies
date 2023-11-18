
namespace Movies.BusinessLogic;

public interface IIdentityService
{
    string GenerateToken(UserDtoResponse? user);
    Task<UserDtoResponse?> GetUserByEmailAndPasswordAsync(LoginDto loginDto, CancellationToken token = default);
}
