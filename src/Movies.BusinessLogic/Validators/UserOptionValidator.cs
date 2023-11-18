
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
            .WithMessage("Вы может сортировать только по колонке name и age");

        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .InclusiveBetween(10, 50)
            .WithMessage("Вы можете выбрать  от 10 до 50 записей на странице");
    }
}
