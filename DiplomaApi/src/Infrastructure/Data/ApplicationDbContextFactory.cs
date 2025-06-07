using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DiplomaApi.Infrastructure.Data;

public class ApplicationDbContextFactory
    : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseNpgsql(
                // тут можна читати з env або жорстко прописати для design-time
                Environment.GetEnvironmentVariable("DESIGN_CONNECTION")
                ?? "Host=localhost;Port=5432;Database=dronedb;Username=drone;Password=drone123")
            .Options;

        return new ApplicationDbContext(options);
    }
}
