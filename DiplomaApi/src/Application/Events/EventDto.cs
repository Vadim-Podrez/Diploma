using System.Text.Json;
using NpgsqlTypes;

namespace DiplomaApi.Application.Events;

public sealed record EventDto(string SensorId, NpgsqlPoint Coords, JsonElement Payload);

