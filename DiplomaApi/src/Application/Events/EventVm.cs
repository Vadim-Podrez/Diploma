using System.Text.Json;

namespace DiplomaApi.Application.Events;

public record EventVm(long Id, string SensorId, DateTime Timestamp, JsonElement Payload);
