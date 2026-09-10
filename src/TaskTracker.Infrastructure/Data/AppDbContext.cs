using Microsoft.EntityFrameworkCore;
using TaskTracker.Domain.Tasks;

namespace TaskTracker.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public DbSet<TaskItem> Tasks => Set<TaskItem>();
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}