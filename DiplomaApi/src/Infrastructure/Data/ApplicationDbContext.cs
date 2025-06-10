using System.Reflection;
using DiplomaApi.Application.Common.Interfaces;
using DiplomaApi.Domain.Entities;
using DiplomaApi.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DiplomaApi.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<TodoList> TodoLists => Set<TodoList>();

    public DbSet<TodoItem> TodoItems => Set<TodoItem>();
    public DbSet<Event> Events => Set<Event>();
    
    public DbSet<Sensor> Sensors { get; set; }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Підтягнути ВСІ конфігурації з поточної збірки
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(ApplicationDbContext).Assembly);
        
        modelBuilder.Entity<Sensor>(entity =>
        {
            entity.Property(e => e.Coords).HasColumnType("point");
            entity.Property(e => e.Coverage).HasDefaultValue(0);
            entity.Property(e => e.Status).HasDefaultValue("online");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
        });

        modelBuilder.Entity<Event>(entity =>
        {
            entity.Property(e => e.Coords).HasColumnType("point");
            entity.Property(e => e.Payload).HasColumnType("jsonb");

            entity.HasOne(e => e.Sensor)
                .WithMany(s => s.Events)
                .HasForeignKey(e => e.SensorId);
        });
        
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
