using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskTracker.Domain.Tasks;

namespace TaskTracker.Infrastructure.Configurations;

public class TaskItemConfiguration : IEntityTypeConfiguration<TaskItem>
{
    public void Configure(EntityTypeBuilder<TaskItem> builder)
    {
        builder.ToTable("tasks");
        
        builder.HasKey(task => task.Id);
        
        builder.Property(task => task.Id)
            .HasColumnName("id");
        
        builder.Property(task => task.Title)
            .HasColumnName("title")
            .HasMaxLength(TaskItem.MaxTitleLength)
            .IsRequired();

        builder.Property(task => task.Description)
            .HasColumnName("description")
            .HasMaxLength(TaskItem.MaxDescriptionLength);
        
        builder.Property(task => task.IsCompleted)
            .HasColumnName("is_completed")
            .IsRequired();

        builder.Property(task => task.CreatedAt)
            .HasColumnName("created_at")
            .HasColumnType("timestamp with time zone")
            .IsRequired();

        builder.Property(task => task.CompletedAt)
            .HasColumnName("completed_at")
            .HasColumnType("timestamp with time zone");

        builder.HasIndex(task => task.CreatedAt);
        
        builder.HasIndex(task => task.IsCompleted);
    }
}