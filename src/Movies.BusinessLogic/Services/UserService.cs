
using FluentValidation;
using Movies.BusinessLogic.Helpers;
using Movies.DataAccess;

namespace Movies.BusinessLogic;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    private readonly IValidator<UserDto> _validator;
    private readonly IValidator<ChangePasswordDto> _changePasswordValidator;
    private readonly IValidator<MyUserOptions> _optionsValidator;

    public UserService(
        IUserRepository userRepository, 
        IValidator<UserDto>  validator, 
        IValidator<MyUserOptions> optionsValidator,
        IValidator<ChangePasswordDto> changePasswordValidator)
    {
        _userRepository = userRepository;
        _validator = validator;
        _changePasswordValidator = changePasswordValidator;
        _optionsValidator = optionsValidator;
    }

    public async ValueTask<bool> ChangePasswordAsync(ChangePasswordDto changePasswordDto, CancellationToken token = default)
    {
        _changePasswordValidator.ValidateAndThrow(changePasswordDto);

        changePasswordDto.OldPassword = PasswordHashHelper.PasswordHash(changePasswordDto.OldPassword, changePasswordDto.Email);

        User user = await _userRepository.GetUserByEmailAndPasswordAsync(changePasswordDto.Email, changePasswordDto.OldPassword, token);

        if (user is null)
            return false;

        changePasswordDto.NewPassword = PasswordHashHelper.PasswordHash(changePasswordDto.NewPassword, changePasswordDto.Email);

        bool isChangePassword = await _userRepository.ChangeUserPasswordAsync(changePasswordDto.UserId, changePasswordDto.NewPassword, token);

        if (!isChangePassword)
            return false;

        return true;
    }

    public async Task<UserDtoResponse> CreateUserAsync(UserDto userDto, CancellationToken token = default)
    {
        await _validator.ValidateAsync(userDto, opt =>
        {
            opt.ThrowOnFailures();
            opt.IncludeRuleSets("Create");
        }, token);

        User user = userDto.DtoToUser();

        user.Password = PasswordHashHelper.PasswordHash(user.Password,user.Email);

        int userId = await _userRepository.CreateAsync(user, token);
        
        User createdUser = await _userRepository.GetAsync(userId);

        return createdUser.UserToResponseDto();
    }

    public async ValueTask<bool> DeleteUserAsync(int id, CancellationToken token = default)
    {
        if(id <= 0)
            return false;
        

        bool isDeleted = await _userRepository.DeleteAsync(id, token);

        return isDeleted;
    }

    public async Task<UserDtoResponse?> EditProfileAsync(UserDto userDto, CancellationToken token = default)
    {
         _validator.Validate(userDto, opt =>
        {
            opt.ThrowOnFailures();
            opt.IncludeRuleSets("EditProfile");
        });

        User user = userDto.DtoToUser();

        bool isUpdated = await _userRepository.EditProfileAsync(user, token);

        if (!isUpdated)
            return null;

        User updatedUser = await _userRepository.GetAsync(user.Id, token);

        return updatedUser.UserToResponseDto();
    }

    public async Task<UserDtoResponse?> EditUserAsync(UserDto userDto, CancellationToken token = default)
    {
        await _validator.ValidateAsync(userDto, opt =>
        {
            opt.ThrowOnFailures();
            opt.IncludeRuleSets("Edit");
        }, token);

        User user = userDto.DtoToUser();

        bool isUpdated = await _userRepository.EditAsync(user, token);

        if(!isUpdated)
            return null;
        
        User updatedUser = await _userRepository.GetAsync(user.Id, token);

        return updatedUser.UserToResponseDto();
        
    }

    public async Task<UsersViewResponseDto> GetAllUsersAsync(UserOptionsDto optionsDto,CancellationToken token = default)
    {
        MyUserOptions options = optionsDto.DtoToUserOptions();

        _optionsValidator.ValidateAndThrow(options);

        IEnumerable<User> users =  await _userRepository.GetAllAsync(options, token);

        if (users is null)
            return new();
        
        int countMovies = await _userRepository.GetCountUsers(options, token);

        return users.UsersToUsersViewResponseDto(countMovies, options.Page, options.PageSize);
    }

    public async Task<UserDtoResponse?> GetUserByIdAsync(int id, CancellationToken token = default)
    {
        User user = await _userRepository.GetAsync(id, token);

        if(user is null)
        {
            return null;
        }

        return user.UserToResponseDto();
    }
}