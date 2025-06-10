namespace DiplomaApi.Domain.Entities;

// Models/Sensor.cs
public class Sensor
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Type { get; set; } = default!;
    public NpgsqlTypes.NpgsqlPoint Coords { get; set; }
    public float Coverage { get; set; }
    public float? Direction { get; set; }
    public string Status { get; set; } = "online";
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public ICollection<Event> Events { get; set; } = new List<Event>();
}
