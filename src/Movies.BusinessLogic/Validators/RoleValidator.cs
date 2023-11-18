
using FluentValidation;

namespace Movies.BusinessLogic;

public class RoleValidator : AbstractValidator<RoleDto>
{
    public RoleValidator()
    {
        RuleSet("Create", () =>
        {
            RuleFor(x => x.Name).NotEmpty();
        });

        RuleSet("Edit", () =>
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Id).NotEmpty().GreaterThan(0);
        });
    }
}
