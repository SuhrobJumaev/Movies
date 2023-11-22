using FluentValidation;
using Movies.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.BusinessLogic.Validators
{
    public class RegistrationValidator : AbstractValidator<RegistrationDto>
    {
        private const string phonePattern = "^(992[0-9]{9})$";
        private readonly IUserRepository _userRepository;

        public RegistrationValidator(IUserRepository userRepository)
        {
            _userRepository = userRepository;

            RuleFor(x => x.Name).NotEmpty().MinimumLength(3).MaximumLength(10);
            RuleFor(x => x.LastName).NotEmpty().MinimumLength(3).MaximumLength(10);
            RuleFor(x => x.Age).NotEmpty().GreaterThan((short)0).LessThan((short)100);
            RuleFor(x => x.Gender).Must(BeValidGender);
            RuleFor(x => x.Phone).Matches(phonePattern);
            RuleFor(x => x.Email).EmailAddress().MustAsync(IsEmailUnique).WithMessage(Utils.ValidationErrorMessage.EmailAlreadyExistsErrorMessage);
            RuleFor(x => x.Password).NotEmpty().MinimumLength(6).MaximumLength(20);
            RuleFor(x => x.ConfirmPassword).Equal(x => x.Password);
        }

        private bool BeValidGender(short gender)
        {
            Gender convertedGender = (Gender)gender;

            if (convertedGender != Gender.Male && convertedGender != Gender.Female)
                return false;

            return true;
        }

        private async Task<bool> IsEmailUnique(string email, CancellationToken token = default)
        {
            User user = await _userRepository.GetUserByEmailAsync(email, token);

            if (user is null)
                return true;

            return false;
        }
    }
}
