using DiplomaApi.Infrastructure.Data;
using DiplomaApi.Web.Hubs;
using  DiplomaApi.Infrastructure.Mqtt;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.AddKeyVaultIfConfigured();
builder.AddApplicationServices();
builder.AddInfrastructureServices();
builder.AddWebServices();

builder.Services.AddSignalR();
builder.Services.AddHostedService<MqttListener>();

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

app.MapHub<EventHub>("/eventHub");

app.Map("/", () => Results.Redirect("/api"));

app.MapEndpoints();

app.Run();

public partial class Program { }
