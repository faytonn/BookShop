using Shared.Abstractions.Payments;
using Stripe;
using Stripe.Checkout;

namespace Infrastructure.Providers.Payments.Stripe;

public sealed class StripePaymentService(StripeOptions options) : IPaymentService
{
    private readonly StripeOptions _options = options;

    public async Task<PaymentCheckoutSession> CreateCheckoutSessionAsync(PaymentCheckoutRequest request)
    {
        if (string.IsNullOrWhiteSpace(_options.SecretKey))
            throw new InvalidOperationException("Stripe secret key is missing. Configure Stripe:SecretKey.");

        if (string.IsNullOrWhiteSpace(_options.SuccessUrl) || string.IsNullOrWhiteSpace(_options.CancelUrl))
            throw new InvalidOperationException("Stripe SuccessUrl/CancelUrl are missing in configuration.");

        StripeConfiguration.ApiKey = _options.SecretKey;

        var amountInCents = (long)Math.Round(request.Amount * 100, MidpointRounding.AwayFromZero);
        var currency = string.IsNullOrWhiteSpace(_options.Currency) ? request.Currency : _options.Currency;

        var sessionOptions = new SessionCreateOptions
        {
            Mode = "payment",
            SuccessUrl = _options.SuccessUrl,
            CancelUrl = _options.CancelUrl,
            CustomerEmail = request.CustomerEmail,
            Metadata = new Dictionary<string, string>
            {
                ["orderId"] = request.OrderId.ToString(),
                ["orderCode"] = request.OrderCode
            },
            LineItems =
            [
                new SessionLineItemOptions
                {
                    Quantity = 1,
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = currency,
                        UnitAmount = amountInCents,
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = $"Order {request.OrderCode}"
                        }
                    }
                }
            ]
        };

        var service = new SessionService();
        var session = await service.CreateAsync(sessionOptions);

        return new PaymentCheckoutSession(session.Id, session.Url ?? string.Empty);
    }
}
