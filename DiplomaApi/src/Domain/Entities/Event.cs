using System.Text.Json;
using NpgsqlTypes;

namespace DiplomaApi.Domain.Entities;

public class Event
{
    public int Id { get; set; }
    public int SensorId { get; set; } = default!;
    public Sensor Sensor { get; set; } = default!;

    public NpgsqlPoint Coords { get; set; }
    public DateTime Timestamp { get; set; }
    
    //public string? Type { get; set; }
    //public float? Level { get; set; }
    public JsonElement Payload { get; set; } = default!;
}
