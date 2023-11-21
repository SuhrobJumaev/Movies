
using FluentValidation;
using Movies.DataAccess;

namespace Movies.BusinessLogic;

public class MovieOptionValidator : AbstractValidator<MovieOptions>
{
    private static readonly string[] AcceptableSortFields =
    {
       "id", "title", "year"
    };

    public MovieOptionValidator()
    {
        RuleFor(x => x.Year)
            .LessThanOrEqualTo(DateTime.UtcNow.Year);

        RuleFor(x => x.SortField)
            .Must(x => x is null || AcceptableSortFields.Contains(x, StringComparer.OrdinalIgnoreCase))
            .WithMessage(Utils.ValidationErrorMessage.MovieSortingErrorMessage);

        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .InclusiveBetween(10, 50)
            .WithMessage(Utils.ValidationErrorMessage.MoviePaginationErrorMessage);
    }
}
