using System.Globalization;
using TaskTracker.Application.Tasks;
using TaskTracker.Domain.Tasks;

namespace TaskTracker.Infrastructure.Tasks;

public class InMemoryTaskRepository : ITaskRepository
{
    private readonly List<TaskItem> _tasks = [];
    public Task<TaskItem> AddAsync(TaskItem task, CancellationToken cancellationToken)
    {
        _tasks.Add(task);
        return Task.FromResult(task);
    }

    public Task<TaskItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var task = _tasks.FirstOrDefault(task => task.Id == id);
        return Task.FromResult(task);
    }

    public Task<IReadOnlyList<TaskItem>> GetPagedAsync(
        int skip, 
        int take,
        bool? isCompleted,
        CancellationToken cancellationToken)
    {
        IEnumerable<TaskItem> query = _tasks;

        if (isCompleted.HasValue)
        {
            query = query.Where(task => task.IsCompleted == isCompleted.Value);
        }
        
        IReadOnlyList<TaskItem> result = query
            .OrderByDescending(task => task.CreatedAt)
            .Skip(skip)
            .Take(take)
            .ToList();
        
        return Task.FromResult(result);
    }

    public Task<int> CountAsync(bool? isCompleted, CancellationToken cancellationToken)
    {
        IEnumerable<TaskItem> query = _tasks;

        if (isCompleted.HasValue)
        {
            query = query.Where(task => task.IsCompleted == isCompleted.Value);
        }
        
        return Task.FromResult(query.Count());
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task DeleteAsync(TaskItem task, CancellationToken cancellationToken)
    {
        _tasks.Remove(task);
        
        return Task.CompletedTask;
    }
}