namespace Movies.BusinessLogic;

public interface IUserService
{
    Task<UserDto> CreateUserAsync(UserDto userDto, CancellationToken token = default);
    Task<UserDto> GetUserByIdAsync(int id, CancellationToken token = default);
    Task<IEnumerable<UserDto>> GetAllUsersAsync(CancellationToken token = default);
    Task<UserDto> EditUserAsync(UserDto userDto, CancellationToken token = default);
    ValueTask<bool> DeleteUserAsync(int id, CancellationToken token = default);
}