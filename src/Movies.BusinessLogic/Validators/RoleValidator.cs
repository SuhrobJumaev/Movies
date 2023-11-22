
using FluentValidation;

namespace Movies.BusinessLogic;

public class RoleValidator : AbstractValidator<RoleDto>
{
    public RoleValidator()
    {
        RuleSet(Utils.CreateRuleSetName, () =>
        {
            RuleFor(x => x.Name).NotEmpty();
        });

        RuleSet(Utils.EditRuleSetName, () =>
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Id).NotEmpty().GreaterThan(0);
        });
    }
}
