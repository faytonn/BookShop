namespace Application.Validators
{
    public class CategoryValidator : AbstractValidator<CategoryRequest>
    {
        public CategoryValidator()
        {
            RuleFor(req => req.Name).
                NotEmpty().NotNull().WithMessage("Category cannot be empty!")
                .Length(3, 32).WithMessage("Category name should have a length of between 3 and 32!");

            RuleFor(req => req.PriorityLevel)
                .GreaterThanOrEqualTo(0).LessThanOrEqualTo(4).WithMessage("The priority level should be between 0 and 4.");

            RuleFor(req => req.ParentId)
                .Must(id => id is null || !id.Equals(Guid.Empty)).WithMessage("Id cannot be an empty Guid.");

        }
    }
}
