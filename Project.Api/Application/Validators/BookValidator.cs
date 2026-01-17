namespace Project.Api.Application.Validators;

public sealed class BookValidator : AbstractValidator<BookRequest>
{
    public BookValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(256);

        RuleFor(x => x.Price)
            .NotEmpty()
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.ReleaseDate)
            .Must(date => date <= DateTime.Now);

        RuleFor(x => x.LanguageIds)
            .NotEmpty()
            .Must(languageIds => languageIds.Count != 0);
    }
}
