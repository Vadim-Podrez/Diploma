using DiplomaApi.Domain.Entities;

namespace DiplomaApi.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<TodoList> TodoLists { get; }

    DbSet<TodoItem> TodoItems { get; }

    /// <summary> Події, що приходять з MQTT-датчиків </summary>
    DbSet<Event> Events { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    
}
