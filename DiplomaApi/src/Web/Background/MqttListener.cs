
using System.Text;
using System.Text.Json;
using DiplomaApi.Application.Common.Interfaces;
using DiplomaApi.Application.Events;
using DiplomaApi.Domain.Entities;
using DiplomaApi.Web.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Client;
using NpgsqlTypes;


namespace DiplomaApi.Infrastructure.Mqtt;

public sealed class MqttListener : BackgroundService
{
    private readonly IServiceProvider _sp;
    private readonly ILogger<MqttListener> _logger;

    public MqttListener(IServiceProvider sp, ILogger<MqttListener> logger)
    {
        _sp     = sp;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        _logger.LogInformation("Starting MQTT listener …");

        var factory = new MqttFactory();
        using var client = factory.CreateMqttClient();

        var opts = new MqttClientOptionsBuilder()
            .WithTcpServer("mosquitto", 1883)    // name із docker-compose
            .Build();

        // 1. підключення
        await client.ConnectAsync(opts, ct);

        // 2. підписка на топік
        var subs = factory.CreateSubscribeOptionsBuilder()
            .WithTopicFilter(f => f.WithTopic("rf/events/#"))
            .Build();
        await client.SubscribeAsync(subs, ct);
        _logger.LogInformation("Subscribed to rf/events/#");

        // 3. обробка вхідних повідомлень
        client.ApplicationMessageReceivedAsync += async e =>
        {
            try
            {
                
                string json = Encoding.UTF8.GetString(e.ApplicationMessage.PayloadSegment);
                _logger.LogInformation("Received MQTT raw payload: {Json}", json); 

                var dto = JsonSerializer.Deserialize<EventDto>(
                    json, 
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }); 
                if (dto is null)                         // ← захист від некоректного JSON  
                {
                    _logger.LogWarning("Received invalid MQTT payload: {Json}", json);
                    return;                               // пропускаємо цей пакет
                }

                await using var scope = _sp.CreateAsyncScope();
                var db  = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
                var hub = scope.ServiceProvider.GetRequiredService<IHubContext<EventHub>>();

                
                /*NpgsqlPoint coords = default;
                if (dto.Payload.TryGetProperty("coords", out var coordsProp) && coordsProp.GetArrayLength() == 2)
                {
                    var lat = coordsProp[0].GetDouble();
                    var lng = coordsProp[1].GetDouble();
                    coords = new NpgsqlPoint(lat, lng);
                }
                else
                {
                    coords = new NpgsqlPoint(0, 0);
                }*/
                
                db.Events.Add(new Event
                {
                    SensorId  = dto.SensorId,
                    Timestamp = DateTime.UtcNow,
                    Payload   = dto.Payload,
                    Coords    = dto.Coords
                });
                await db.SaveChangesAsync(ct);

                await hub.Clients.All.SendAsync("newEvent", dto, ct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to process MQTT message");
            }
        };

        // 4. не даємо BackgroundService завершитись
        await Task.Delay(Timeout.Infinite, ct);
    }

}
