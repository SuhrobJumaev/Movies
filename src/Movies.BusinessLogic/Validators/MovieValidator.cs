
using FluentValidation;

namespace Movies.BusinessLogic;

public class MovieValidator : AbstractValidator<MovieDto>
{
    public MovieValidator()
    {
        RuleSet("Create", () =>
        {
            RuleFor(x => x.Title).NotEmpty();
            RuleFor(x => x.YearOfRelease).NotEmpty();
        });

        RuleSet("Edit", () =>
        {
            RuleFor(x => x.Id).NotEmpty().GreaterThan(0);
            RuleFor(x => x.Title).NotEmpty();
            RuleFor(x => x.YearOfRelease).NotEmpty();
        });
    }
}
