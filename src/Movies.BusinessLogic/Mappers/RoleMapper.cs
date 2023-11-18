
using Movies.DataAccess;

namespace Movies.BusinessLogic;

public static class RoleMapper
{
    public static Role DtoToRole(this RoleDto roleDto)
    {
        return new()
        {
            Id = roleDto.Id,
            Name = roleDto.Name,
        };
    }

    public static IEnumerable<RoleDto> RolesToDto(this IEnumerable<Role> roles)
    {
        IEnumerable<RoleDto> rolesDto = Enumerable.Empty<RoleDto>();

        rolesDto = roles.Select(g => new RoleDto
        {
            Id = g.Id,
            Name = g.Name,

        }).ToList();

        return rolesDto;
    }

    public static RoleDto RoleToDto(this Role role)
    {
        return new()
        {
            Id = role.Id,
            Name = role.Name,
        };  
    }
}
