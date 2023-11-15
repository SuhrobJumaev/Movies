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
            RoleId = (Role)userDto.RoleId,
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
            RoleId = (short)user.RoleId,
            Gender = (short)user.Gender,
            Email = user.Email,
            Phone = user.Phone,
            RoleName = user.RoleName,
        };
    }
}

