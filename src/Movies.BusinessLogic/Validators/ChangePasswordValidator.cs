using FluentValidation;

namespace Movies.BusinessLogic;

public class ChangePasswordValidator : AbstractValidator<ChangePasswordDto>
{
    public ChangePasswordValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.OldPassword).NotEmpty().MinimumLength(6).MaximumLength(20);
        RuleFor(x => x.NewPassword).NotEmpty().MinimumLength(6).MaximumLength(20);
        RuleFor(x => x.ConfirmPassword).Equal(x => x.NewPassword);
    }
}
