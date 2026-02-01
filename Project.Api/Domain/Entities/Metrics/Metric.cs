using Project.Api.Domain.Entities.Commons;
using System.Text.Json.Nodes;

namespace Project.Api.Domain.Entities.Metrics;

public class Metric : Entity
{
    public required string Key { get; set; }
    public required JsonObject Value { get; set; }
    public DateTime MeasuredAt { get; set; }
}
