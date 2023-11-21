
using FluentValidation;

namespace Movies.BusinessLogic;

public class GenreValidator : AbstractValidator<GenreDto>
{
    public GenreValidator()
    {
        RuleSet(Utils.CreateRuleSetName,() =>
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
