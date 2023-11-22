
using FluentValidation;

namespace Movies.BusinessLogic;

public class MovieValidator : AbstractValidator<MovieDto>
{
    public MovieValidator()
    {
        RuleSet(Utils.CreateRuleSetName, () =>
        {
            RuleFor(x => x.Title).NotEmpty();
            RuleFor(x => x.YearOfRelease).NotEmpty();
            RuleFor(x => x.Video).NotEmpty();
        });

        RuleSet(Utils.EditRuleSetName, () =>
        {
            RuleFor(x => x.Id).NotEmpty().GreaterThan(0);
            RuleFor(x => x.Title).NotEmpty();
            RuleFor(x => x.YearOfRelease).NotEmpty();
        });
    }
}
