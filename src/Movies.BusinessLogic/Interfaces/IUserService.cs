namespace Movies.BusinessLogic;

public interface IUserService
{
    Task<UserDtoResponse> CreateUserAsync(UserDto userDto, CancellationToken token = default);
    Task<UserDtoResponse?> GetUserByIdAsync(int id, CancellationToken token = default);
    Task<IEnumerable<UserDtoResponse>> GetAllUsersAsync(CancellationToken token = default);
    Task<UserDtoResponse?> EditUserAsync(UserDto userDto, CancellationToken token = default);
    ValueTask<bool> DeleteUserAsync(int id, CancellationToken token = default);
    Task<UserDtoResponse?> GetUserByEmailAndPasswordAsync(LoginDto loginDto, CancellationToken token = default);
}