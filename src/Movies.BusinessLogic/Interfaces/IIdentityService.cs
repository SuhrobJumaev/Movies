
namespace Movies.BusinessLogic;

public interface IIdentityService
{
    string GenerateToken(UserDtoResponse? user);
}
