namespace Application.Validators;

public sealed class LoginValidator : AbstractValidator<LoginRequest>
{
    public LoginValidator()
    {
        RuleFor(r => r.Email)
            .NotEmpty().NotNull().WithMessage("Email cannot be empty!")
            .Length(5, 256).WithMessage("Email must be between 5 and 256 characters length!");
        RuleFor(r => r.Password)
            .NotEmpty().NotNull().WithMessage("Email cannot be empty!")
            .Length(8, 64).WithMessage("Password must be between 8 and 64 characters length!");
    }
}

public sealed class RegisterValidator : AbstractValidator<RegisterRequest>
{
    public RegisterValidator()
    {
        RuleFor(r => r.Email)
            .NotEmpty().NotNull().WithMessage("Email cannot be empty!")
            .Length(5, 256).WithMessage("Email must be between 5 and 256 characters length!");
        RuleFor(r => r.Password)
            .NotEmpty().NotNull().WithMessage("Email cannot be empty!")
            .Length(8, 64).WithMessage("Password must be between 8 and 64 characters length!");
        RuleFor(r => r.UserRole)
            .IsInEnum().WithMessage("Role should only be in enum.");
    }
}
