
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Movies.BusinessLogic.Helpers;
using Movies.DataAccess;

namespace Movies.BusinessLogic;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    private readonly IValidator<UserDto> _validator;
    private readonly IValidator<LoginDto> _loginValidator;

    public UserService(
        IUserRepository userRepository, 
        IValidator<UserDto>  validator, 
        IValidator<LoginDto> loginValidator)
    {
        _userRepository = userRepository;
        _validator = validator;
        _loginValidator = loginValidator;
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

    public async Task<IEnumerable<UserDtoResponse>> GetAllUsersAsync(CancellationToken token = default)
    {
        IEnumerable<User> users =  await _userRepository.GetAllAsync(token);
        
        if(users is null)
        {
            return Enumerable.Empty<UserDtoResponse>();
        }

        return users.UsersToResponseDto();

    }

    public async Task<UserDtoResponse?> GetUserByEmailAndPasswordAsync(LoginDto loginDto, CancellationToken token = default)
    {
        _loginValidator.Validate(loginDto, opt => opt.ThrowOnFailures());

        loginDto.Password = PasswordHashHelper.PasswordHash(loginDto.Password, loginDto.Email);

        User user = await _userRepository.GetUserByEmailAndPasswordAsync(loginDto.Email, loginDto.Password, token);

        if (user is null)
            return null;

        return user.UserToResponseDto();
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