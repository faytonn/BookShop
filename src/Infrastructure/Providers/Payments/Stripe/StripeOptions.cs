namespace Infrastructure.Providers.Payments.Stripe;

public sealed class StripeOptions
{
    public string SecretKey { get; set; } = string.Empty;
    public string SuccessUrl { get; set; } = string.Empty;
    public string CancelUrl { get; set; } = string.Empty;
    public string Currency { get; set; } = "usd";
}
