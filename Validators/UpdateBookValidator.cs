using FluentValidation;
using BookManagement.Features.Books;

namespace BookManagement.Validators;

public class UpdateBookValidator : AbstractValidator<UpdateBookRequest>
{
    public UpdateBookValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Author).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Year).InclusiveBetween(1450, DateTime.UtcNow.Year + 1);
    }
}