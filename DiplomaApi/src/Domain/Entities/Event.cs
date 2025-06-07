using System.Text.Json;

namespace DiplomaApi.Domain.Entities;


public class Event
{
    public long        Id         { get; set; }
    public string      SensorId   { get; set; } = default!;
    public DateTime    Timestamp  { get; set; }
    public JsonElement Payload    { get; set; }   // ← було string
}

