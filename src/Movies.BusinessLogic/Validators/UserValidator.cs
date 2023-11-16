

using FluentValidation;
using Movies.DataAccess;

namespace Movies.BusinessLogic;

public class UserValidator : AbstractValidator<UserDto>
{
    private const string phonePattern = "^(992[0-9]{9})$";

    private IUserRepository _userRepository;

    public UserValidator(IUserRepository userRepository)
    {
        _userRepository = userRepository;

        RuleSet("Create", () =>
        {
            RuleFor(x => x.Name).NotEmpty().MinimumLength(3).MaximumLength(10);
            RuleFor(x => x.LastName).NotEmpty().MinimumLength(3).MaximumLength(10);
            RuleFor(x => x.Age).NotEmpty().GreaterThan((short)0).LessThan((short)100);
            RuleFor(x => x.Gender).Must(BeValidGender);
            RuleFor(x => x.Phone).Matches(phonePattern);
            RuleFor(x => x.Email).EmailAddress().MustAsync(IsEmailUnique).WithMessage("Пользователь с таким email'ом -{PropertyValue} уже существует.");
            RuleFor(x => x.Password).NotEmpty().MinimumLength(6).MaximumLength(20);
            RuleFor(x => x.RoleId).NotEmpty().Must(BeValidRoleId);
        });

        RuleSet("Edit", () =>
        {
            RuleFor(x => x.Id).NotEmpty().GreaterThan(0);
            RuleFor(x => x.Name).NotEmpty().MinimumLength(3).MaximumLength(10);
            RuleFor(x => x.LastName).NotEmpty().MinimumLength(3).MaximumLength(10);
            RuleFor(x => x.Age).NotEmpty().GreaterThan((short)0).LessThan((short)100);
            RuleFor(x => x.Gender).Must(BeValidGender);
            RuleFor(x => x.Phone).Matches(phonePattern);
            RuleFor(x => x.RoleId).NotEmpty().Must(BeValidRoleId);
        });

    }

    private bool BeValidGender(short gender)
    {
        Gender convertedGender = (Gender)gender;


        if(convertedGender != Gender.Male && convertedGender != Gender.Female)
            return false;

        return true;
    }

    private bool BeValidRoleId(short roleId)
    {
        Role convertedRole = (Role)roleId;

        if(convertedRole != Role.User && convertedRole != Role.Admin)
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

