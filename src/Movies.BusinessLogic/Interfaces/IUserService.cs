namespace Movies.BusinessLogic;

public interface IUserService
{
    Task<UserDtoResponse> CreateUserAsync(UserDto userDto, CancellationToken token = default);
    Task<UserDtoResponse?> GetUserByIdAsync(int id, CancellationToken token = default);
    Task<UsersViewResponseDto> GetAllUsersAsync(UserOptionsDto optionsDto,CancellationToken token = default);
    Task<UserDtoResponse?> EditUserAsync(UserDto userDto, CancellationToken token = default);
    Task<UserDtoResponse?> EditProfileAsync(UserDto userDto, CancellationToken token = default);
    ValueTask<bool> DeleteUserAsync(int id, CancellationToken token = default);
    ValueTask<bool> ChangePasswordAsync(ChangePasswordDto user, CancellationToken token = default);
}