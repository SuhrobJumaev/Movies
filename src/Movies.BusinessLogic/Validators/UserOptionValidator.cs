
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Movies.DataAccess;

namespace Movies.BusinessLogic;

public class UserOptionValidator : AbstractValidator<MyUserOptions>
{
    private static readonly string[] AcceptableSortFields =
    {
       "id", "name", "age"
    };

    public UserOptionValidator()
    {
        
        RuleFor(x => x.SortField)
            .Must(x => x is null || AcceptableSortFields.Contains(x, StringComparer.OrdinalIgnoreCase))
            .WithMessage(Utils.ValidationErrorMessage.UserSortingErrorMessage);

        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .InclusiveBetween(10, 50)
            .WithMessage(Utils.ValidationErrorMessage.UserPaginationErrorMessage);
    }
}
