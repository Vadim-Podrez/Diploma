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
    options.AddPolicy("dash", policy =>
    {
        policy.WithOrigins("http://localhost:8080", "http://127.0.0.1:8080")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
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
    await app.InitialiseDatabaseAsync();   // ← залишилось як було
}
else
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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

app.UseCors("dash");

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
