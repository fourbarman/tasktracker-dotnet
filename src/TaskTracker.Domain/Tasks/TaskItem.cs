using TaskTracker.Domain.Common;

namespace TaskTracker.Domain.Tasks;

/**
 * Бизнес сущность.
 */
public class TaskItem
{
    private const int MaxTitleLength = 200;
    private const int MaxDescriptionLength = 2000;
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
        ValidateTitle(title);
        ValidateDescription(description);
        
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
        ValidateTitle(title);
        
        Title = title;
    }

    public void ChangeDescription(string? newDescription)
    {
        ValidateDescription(newDescription);
        
        Description = newDescription;
    }

    public void Update(string title, string? description)
    {
        Rename(title);
        ChangeDescription(description);
    }

    private static void ValidateTitle(string title)
    {
        if (String.IsNullOrWhiteSpace(title))
        {
            throw new DomainValidationException("title", "Title cannot be null or whitespace.");
        }

        if (title.Length > MaxTitleLength)
        {
            throw new DomainValidationException("title", $"Title cannot be longer than {MaxTitleLength} characters.");
        }
    }

    private static void ValidateDescription(string? description)
    {
        if (description is null)
        {
            return;
        }

        if (description.Length > MaxDescriptionLength)
        {
            throw new DomainValidationException("description",  $"Description cannot be longer than {MaxDescriptionLength} characters.");
        }
    }
}