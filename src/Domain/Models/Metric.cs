namespace Domain.Models;

public class Metric : Entity<Guid>
{
    public required string Key { get; set; }
    public required string Value { get; set; }
    public DateTime MeasuredAt { get; set; } = DateTime.UtcNow;
}
