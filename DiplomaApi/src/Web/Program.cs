using DiplomaApi.Application.Common.Interfaces;
using DiplomaApi.Application.Events;
using DiplomaApi.Infrastructure.Data;
using DiplomaApi.Web.Hubs;
using  DiplomaApi.Infrastructure.Mqtt;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.AddKeyVaultIfConfigured();
builder.AddApplicationServices();
builder.AddInfrastructureServices();
builder.AddWebServices();

builder.Services.AddSignalR();
builder.Services.AddHostedService<MqttListener>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy
            .AllowAnyHeader()
            .AllowAnyMethod()
            .SetIsOriginAllowed(_ => true) // Дозволити будь-який origin для тестів
            .AllowCredentials();           // ВАЖЛИВО для SignalR!
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    await app.InitialiseDatabaseAsync();
}

if (!app.Environment.IsEnvironment("Migration"))
{
    await app.InitialiseDatabaseAsync();
}
else
{
    app.UseHsts();
}

app.UseHealthChecks("/health");
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSwaggerUi(settings =>
{
    settings.Path = "/api";
    settings.DocumentPath = "/api/specification.json";
});


app.UseExceptionHandler(options => { });

app.UseCors();

app.MapHub<EventHub>("/eventHub");

app.Map("/", () => Results.Redirect("/api"));

app.MapEndpoints();

app.MapGet("/api/events/latest/{n:int}", async (int n, IApplicationDbContext db) =>
    {
        var events = await db.Events
            .OrderByDescending(e => e.Timestamp)
            .Take(n)
            .Select(e => new EventVm(e.Id, e.SensorId, e.Timestamp, e.Payload))
            .ToListAsync();

        return Results.Ok(events);
    })
    .WithName("LatestEvents")
    .WithOpenApi();

app.Run();

public partial class Program { }
