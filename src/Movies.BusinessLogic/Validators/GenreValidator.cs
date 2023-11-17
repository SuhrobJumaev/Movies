
using FluentValidation;

namespace Movies.BusinessLogic;

public class GenreValidator : AbstractValidator<GenreDto>
{
    public GenreValidator()
    {
        RuleSet("Create",() =>
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
