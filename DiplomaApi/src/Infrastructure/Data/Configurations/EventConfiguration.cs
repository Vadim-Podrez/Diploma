using DiplomaApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiplomaApi.Infrastructure.Data.Configurations;

public sealed class EventConfiguration : IEntityTypeConfiguration<Event>
{
    public void Configure(EntityTypeBuilder<Event> builder)
    {
        builder.ToTable("events");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.SensorId)
            .IsRequired()
            .HasMaxLength(64);

        builder.Property(e => e.Timestamp)
            .IsRequired();

        // зберігаємо як jsonb (Npgsql)
        builder.Property(e => e.Payload)
            .IsRequired()
            .HasColumnType("jsonb");
    }
    
    
}

