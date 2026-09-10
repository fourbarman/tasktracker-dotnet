using TaskTracker.Domain.Common;

namespace TaskTracker.Domain.Tasks;

/**
 * Бизнес сущность.
 */
public class TaskItem
{
    public Guid Id { get; private set; }
    public string Title { get; private set; }
    public string? Description { get; private set; }
    public bool IsCompleted { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? CompletedAt { get; private set; }

    private TaskItem()
    {
        Title = string.Empty;
    }

    public TaskItem(string title, string? description)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new DomainValidationException("title", "Title cannot be null or whitespace.");
        }
        
        Id = Guid.NewGuid();
        Title = title;
        Description = description;
        IsCompleted = false;
        CreatedAt = DateTimeOffset.UtcNow;
        CompletedAt = null;
    }

    public void Complete()
    {
        if (IsCompleted)
        {
            return;
        }
        
        IsCompleted = true;
        CompletedAt = DateTimeOffset.UtcNow;
    }

    public void Rename(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new DomainValidationException(
                "title", "Title cannot be null or whitespace.");
        }
        
        Title = title;
    }

    public void ChangeDescription(string? newDescription)
    {
        Description = newDescription;
    }

    public void Update(string title, string? description)
    {
        Rename(title);
        ChangeDescription(description);
    }
}