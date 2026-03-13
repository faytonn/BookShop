namespace Application.Validators;

public sealed class AddOrderValidator : AbstractValidator<AddOrderRequest>
{
    public AddOrderValidator()
    {
        RuleFor(x => x.OrderItems)
            .NotNull()
            .Must(items => items.Count > 0)
            .WithMessage("At least one order item is required.");

        RuleForEach(x => x.OrderItems).ChildRules(item =>
        {
            item.RuleFor(i => i.Id)
                .NotEmpty()
                .WithMessage("Order item Id must not be empty.");

            item.RuleFor(i => i.Quantity)
                .GreaterThan(0)
                .WithMessage("Quantity must be greater than zero.");
        });

        RuleFor(x => x.ShippingAddress)
            .NotNull()
            .WithMessage("Shipping address is required.");

        RuleFor(x => x.ShippingAddress.FullAddress)
            .NotEmpty()
            .MaximumLength(500)
            .WithMessage("Full address is required and must be under 500 characters.");
    }
}

public sealed class UpdateOrderStatusValidator : AbstractValidator<UpdateOrderStatusRequest>
{
    public UpdateOrderStatusValidator()
    {
        RuleFor(x => x.NewStatus)
            .IsInEnum()
            .WithMessage("Invalid order status.");

        RuleFor(x => x.Description)
            .MaximumLength(1000)
            .When(x => x.Description is not null);

        RuleFor(x => x.PictureUrl)
            .MaximumLength(2048)
            .When(x => x.PictureUrl is not null);
    }
}
