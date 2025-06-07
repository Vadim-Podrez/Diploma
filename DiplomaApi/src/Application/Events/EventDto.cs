using System.Text.Json;

namespace DiplomaApi.Application.Events;

public sealed record EventDto(string SensorId, JsonElement Payload);

