using FluentValidation;
using BookManagement.Features.Books;

namespace BookManagement.Validators;

public class CreateBookValidator : AbstractValidator<CreateBookRequest>
{
    public CreateBookValidator()
    {
        RuleFor(x => x.Title).NotEmpty().WithMessage("Title is required.");
        RuleFor(x => x.Author).NotEmpty().WithMessage("Author is required.");
        RuleFor(x => x.Year)
            .InclusiveBetween(1500, System.DateTime.UtcNow.Year)
            .WithMessage("Year must be a valid publication year.");
    }
}