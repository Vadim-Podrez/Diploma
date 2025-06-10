using System.Text.Json;
using NpgsqlTypes;

namespace DiplomaApi.Application.Events;

public sealed record EventDto(int SensorId, NpgsqlPoint Coords, JsonElement Payload);

