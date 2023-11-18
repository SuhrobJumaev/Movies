
using FluentValidation;
using FluentValidation.Results;
using Movies.DataAccess;

namespace Movies.BusinessLogic;

public class RoleService : IRoleService
{
    private readonly IRoleRepository _roleRepository;
    private readonly IValidator<RoleDto> _validator;

    public RoleService(IRoleRepository roleRepository, IValidator<RoleDto> validator)
    {
        _roleRepository = roleRepository;
        _validator = validator;

    }

    public async Task<RoleDto> CreateRoleAsync(RoleDto roleDto, CancellationToken token = default)
    {
        _validator.Validate(roleDto, opt =>
        {
            opt.ThrowOnFailures();
            opt.IncludeRuleSets("Create");
        });

        Role role = roleDto.DtoToRole();

        int roleId = await _roleRepository.CreateRoleAsync(role, token);

        return roleDto with { Id = roleId };
    }

    public async ValueTask<bool> DeleteRoleAsync(int id, CancellationToken token = default)
    {
        if (id <= 0)
            return false;

        bool isDeleted = await _roleRepository.DeleteRoleAsync(id, token);

        return isDeleted;
    }

    public async Task<RoleDto?> EditRoleAsync(RoleDto roleDto, CancellationToken token = default)
    {
        _validator.Validate(roleDto, opt =>
        {
            opt.ThrowOnFailures();
            opt.IncludeRuleSets("Edit");
        });

        Role role = roleDto.DtoToRole();

        bool isUpdate = await _roleRepository.EditRoleAsync(role, token);

        if (!isUpdate)
            return null;

        return roleDto;
    }

    public async Task<IEnumerable<RoleDto>> GetAllRolesAsync(CancellationToken token = default)
    {
        IEnumerable<Role> roles = await _roleRepository.GetAllRolesAsync(token);

        if (roles is null)
            return Enumerable.Empty<RoleDto>();

        return roles.RolesToDto();
    }

    public async Task<RoleDto?> GetRoleByIdAsync(int id, CancellationToken token = default)
    {
        Role role = await _roleRepository.GetRoleByIdAsync(id, token);

        if (role is null)
            return null;

        return role.RoleToDto();
    }
}
