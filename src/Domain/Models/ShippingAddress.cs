namespace Domain.Models;

public class ShippingAddress
{
    public string FullAddress { get; set; } = null!;
    public string? City { get; set; }
    public string? Country { get; set; }
    public string? ZipCode { get; set; }
    public double? Longitude { get; set; }
    public double? Latitude { get; set; }
}
