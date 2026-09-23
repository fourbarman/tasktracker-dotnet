using TaskTracker.Domain.Tasks;

namespace TaskTracker.Application.Tasks;

public interface ITaskRepository
{
    Task<TaskItem> AddAsync(TaskItem task, CancellationToken cancellationToken);

    Task<TaskItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// skip — сколько записей пропустить
    /// take — сколько записей взять
    /// </summary>
    Task<IReadOnlyList<TaskItem>> GetPagedAsync(
        int skip,
        int take,
        bool? isCompleted,
        DateTimeOffset? dueBefore,
        CancellationToken cancellationToken);

    Task<int> CountAsync(
        bool? isCompleted,
        DateTimeOffset? dueBefore,
        CancellationToken cancellationToken);

    Task SaveChangesAsync(CancellationToken cancellationToken);

    Task DeleteAsync(TaskItem task, CancellationToken cancellationToken);
}