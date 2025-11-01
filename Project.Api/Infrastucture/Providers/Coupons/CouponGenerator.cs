namespace Project.Api.Infrastucture.Providers.Coupons;

public sealed class CouponGenerator
{
    private const string Characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    private const int DefaultCodeLength = 8;

    public string GenerateCouponCode(int length = DefaultCodeLength)
    {
        var random = new Random();
        var code = new char[length];

        for (int i = 0; i < length; i++)
        {
            code[i] = Characters[random.Next(Characters.Length)];
        }

        return new string(code);
    }

    public string GenerateUniqueCouponCode(Func<string, bool> isCodeExists, int length = DefaultCodeLength)
    {
        string code;
        int attempts = 0;
        const int maxAttempts = 100;

        do
        {
            code = GenerateCouponCode(length);
            attempts++;

            if (attempts >= maxAttempts)
            {
                throw new InvalidOperationException("Failed to generate unique coupon code after multiple attempts.");
            }
        } while (isCodeExists(code));

        return code;
    }
}
