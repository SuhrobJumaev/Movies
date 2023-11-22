namespace Movies.BusinessLogic;

using Microsoft.AspNetCore.Mvc.RazorPages;
using Movies.DataAccess;
using System.Numerics;
using System.Xml.Linq;

public static class UserMapper
{
    public static User DtoToUser(this UserDto userDto)
    {
        return new()
        {
            Id = userDto.Id,
            Name = userDto.Name,
            LastName = userDto.LastName,
            Age = userDto.Age,
            Gender = (Gender)userDto.Gender,
            Phone = userDto.Phone,
            Email = userDto.Email,
            Password = userDto.Password,
            RoleId = userDto.RoleId,
        };
    }

    public static IEnumerable<UserDtoResponse> UsersToResponseDto(this IEnumerable<User> users)
    {
        IEnumerable<UserDtoResponse> usersDto = Enumerable.Empty<UserDtoResponse>();

        usersDto = users.Select(u => new UserDtoResponse
        {
            Id = u.Id,
            Name = u.Name,
            LastName = u.LastName,
            Age = u.Age,
            RoleId = (short)u.RoleId,
            Gender = (short)u.Gender,
            Email = u.Email,
            Phone = u.Phone,
            RoleName = u.RoleName,
        }).ToList();

        return usersDto;
    }
    public static UserDtoResponse UserToResponseDto(this User user)
    {
        return new()
        {
            Id = user.Id,
            Name = user.Name,
            LastName = user.LastName,
            Age = user.Age,
            RoleId = user.RoleId,
            Gender = (short)user.Gender,
            Email = user.Email,
            Phone = user.Phone,
            RoleName = user.RoleName,
        };
    }

    public static MyUserOptions DtoToUserOptions(this UserOptionsDto optionsDto)
    {
        return new()
        {
            Search = optionsDto.Search,
            SortField = optionsDto.SortBy is null ? "id" : optionsDto.SortBy.Trim('-'),
            SortOrder = optionsDto.SortBy is null ? SortOrder.Descending :
                optionsDto.SortBy.StartsWith('-') ? SortOrder.Descending : SortOrder.Ascending,
            Page = optionsDto.Page,
            PageSize = optionsDto.PageSize,
        };
    }

    public static UsersViewResponseDto UsersToUsersViewResponseDto(this IEnumerable<User> users, int countMovies, int currentPage, int pageSize)
    {
        return new()
        {
            CurrentPage = currentPage,
            PageSize = pageSize,
            CountPage = (int)Math.Ceiling(countMovies / (decimal)pageSize),
            Users = users.UsersToResponseDto(),
        };
    }

    public static User RegistrationDtoToUser(this RegistrationDto dto)
    {
        return new()
        {
            Name = dto.Name,
            LastName = dto.LastName,
            Age = dto.Age,
            Gender = (Gender)dto.Gender,
            Phone = dto.Phone,
            Email = dto.Email,
            Password = dto.Password,
            RoleId = Utils.UserRoleByDefault,
        };
    }
}

