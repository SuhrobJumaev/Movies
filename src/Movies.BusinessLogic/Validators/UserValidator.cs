using FluentValidation;
using Movies.DataAccess;

namespace Movies.BusinessLogic;

public class UserValidator : AbstractValidator<UserDto>
{
    private const string phonePattern = "^(992[0-9]{9})$";

    private IUserRepository _userRepository;
    private IRoleRepository _roleRepository;

    public UserValidator(IUserRepository userRepository, IRoleRepository roleRepository)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;

        RuleSet(Utils.CreateRuleSetName, () =>
        {
            RuleFor(x => x.Name).NotEmpty().MinimumLength(3).MaximumLength(10);
            RuleFor(x => x.LastName).NotEmpty().MinimumLength(3).MaximumLength(10);
            RuleFor(x => x.Age).NotEmpty().GreaterThan((short)0).LessThan((short)100);
            RuleFor(x => x.Gender).Must(BeValidGender);
            RuleFor(x => x.Phone).Matches(phonePattern);
            RuleFor(x => x.Email).EmailAddress().MustAsync(IsEmailUnique).WithMessage(Utils.ValidationErrorMessage.EmailAlreadyExistsErrorMessage);
            RuleFor(x => x.Password).NotEmpty().MinimumLength(6).MaximumLength(20);
            RuleFor(x => x.RoleId).NotEmpty().MustAsync(IsValidRoleId);
        });

        RuleSet(Utils.EditRuleSetName, () =>
        {
            RuleFor(x => x.Id).NotEmpty().GreaterThan(0);
            RuleFor(x => x.Name).NotEmpty().MinimumLength(3).MaximumLength(10);
            RuleFor(x => x.LastName).NotEmpty().MinimumLength(3).MaximumLength(10);
            RuleFor(x => x.Age).NotEmpty().GreaterThan((short)0).LessThan((short)100);
            RuleFor(x => x.Gender).Must(BeValidGender);
            RuleFor(x => x.Phone).Matches(phonePattern);
            RuleFor(x => x.RoleId).NotEmpty().MustAsync(IsValidRoleId);
        });

        RuleSet(Utils.EditProfileRuleSetName, () =>
        {
            RuleFor(x => x.Id).NotEmpty().GreaterThan(0);
            RuleFor(x => x.Name).NotEmpty().MinimumLength(3).MaximumLength(10);
            RuleFor(x => x.LastName).NotEmpty().MinimumLength(3).MaximumLength(10);
            RuleFor(x => x.Age).NotEmpty().GreaterThan((short)0).LessThan((short)100);
            RuleFor(x => x.Gender).Must(BeValidGender);
            RuleFor(x => x.Phone).Matches(phonePattern);
        });
       
    }

    private bool BeValidGender(short gender)
    {
        Gender convertedGender = (Gender)gender;

        if(convertedGender != Gender.Male && convertedGender != Gender.Female)
            return false;

        return true;
    }

    private async Task<bool> IsValidRoleId(short roleId, CancellationToken token = default)
    {
        Role? role = await  _roleRepository.GetRoleByIdAsync(roleId, token);
        
        if(role is null)
            return false;

        return true;
    }

    private  async Task<bool> IsEmailUnique(string email, CancellationToken token = default)
    {
        User user =  await _userRepository.GetUserByEmailAsync(email, token);

        if (user is null)
            return true;

        return false;
    }
}

