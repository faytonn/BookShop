namespace Shared.Abstractions.Payments;

public interface IPaymentService
{
    Task<PaymentCheckoutSession> CreateCheckoutSessionAsync(PaymentCheckoutRequest request);
}

public sealed record PaymentCheckoutRequest(
    decimal Amount,
    string Currency,
    Guid OrderId,
    string OrderCode,
    string? CustomerEmail);

public sealed record PaymentCheckoutSession(
    string SessionId,
    string CheckoutUrl);
