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

    protected override void OnModelCreating(ModelBuilder builder)
    {
        // Підтягнути ВСІ конфігурації з поточної збірки
        builder.ApplyConfigurationsFromAssembly(
            typeof(ApplicationDbContext).Assembly);
        
        builder.Entity<Event>()
            .Property(e => e.Payload)
            .HasColumnType("jsonb");       // явно вказуємо
        
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
