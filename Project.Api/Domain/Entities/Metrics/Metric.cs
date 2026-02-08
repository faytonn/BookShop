using Project.Api.Domain.Entities.Commons;
using System.Text.Json.Nodes;

namespace Project.Api.Domain.Entities.Metrics;

public class Metric : BaseEntity
{
    public required string Key { get; set; }
    public required string Value { get; set; } // JSON string - like Order.OrderItems
    public DateTime MeasuredAt { get; set; } = DateTime.UtcNow;
}
