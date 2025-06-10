using System.Text.Json;

namespace DiplomaApi.Application.Events;

public record EventVm(int Id, int SensorId, DateTime Timestamp, JsonElement Payload);
