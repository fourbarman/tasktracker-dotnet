using Microsoft.EntityFrameworkCore;
using TaskTracker.Application.Tasks;
using TaskTracker.Domain.Tasks;
using TaskTracker.Infrastructure.Data;

namespace TaskTracker.Infrastructure.Tasks;

public class EfTaskRepository : ITaskRepository
{
    private readonly AppDbContext _dbContext;

    public EfTaskRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<TaskItem> AddAsync(TaskItem task, CancellationToken cancellationToken)
    {
        await _dbContext.Tasks.AddAsync(task, cancellationToken);
        
        return task;
    }

    public async Task<TaskItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _dbContext.Tasks.FirstOrDefaultAsync(task => task.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<TaskItem>> GetPagedAsync(
        int skip, 
        int take, 
        bool? isCompleted,
        CancellationToken cancellationToken)
    {
        var query = _dbContext.Tasks.AsNoTracking();
        
        if (isCompleted.HasValue)
        {
            query = query.Where(task => task.IsCompleted == isCompleted.Value);
        }
        
        return await query
            .OrderByDescending(task => task.CreatedAt)
            .Skip(skip)
            .Take(take)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> CountAsync(bool? isCompleted, CancellationToken cancellationToken)
    {
        var query = _dbContext.Tasks.AsNoTracking();
        if (isCompleted.HasValue)
        {
            query = query.Where(task => task.IsCompleted == isCompleted.Value);
        }
        
        return await query.CountAsync(cancellationToken);
    }

    public Task DeleteAsync(TaskItem task, CancellationToken cancellationToken)
    {
        _dbContext.Tasks.Remove(task);
        
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}