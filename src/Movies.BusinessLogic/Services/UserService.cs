
namespace Movies.BusinessLogic;

public class UserService : IUserService
{
    public Task<UserDto> CreateUserAsync(UserDto userDto, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public ValueTask<bool> DeleteUserAsync(int id, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task<UserDto> EditUserAsync(UserDto userDto, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<UserDto>> GetAllUsersAsync(CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task<UserDto> GetUserByIdAsync(int id, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }
}