namespace Project.Api.Application.Validators
{
    public class LanguageValidator : AbstractValidator<Language>
    {
        public LanguageValidator() 
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .Length(1, 64);
        }
    }
}
