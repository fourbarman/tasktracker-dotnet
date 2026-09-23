using System.ComponentModel.DataAnnotations;
using TaskTracker.Domain.Common;

namespace TaskTracker.Domain.Tasks;

/**
 * Бизнес сущность.
 */
public class TaskItem
{
    /// <summary>
    /// Max title length
    /// </summary>
    public const int MaxTitleLength = 200;
    /// <summary>
    /// Max description length
    /// </summary>
    public const int MaxDescriptionLength = 2000;
    /// <summary>
    /// Task id
    /// </summary>
    public Guid Id { get; private set; }
    /// <summary>
    /// Task title
    /// </summary>
    public string Title { get; private set; }
    /// <summary>
    /// Task description
    /// </summary>
    public string? Description { get; private set; }
    /// <summary>
    /// Task is completed
    /// </summary>
    public bool IsCompleted { get; private set; }
    /// <summary>
    /// Task creation timestamp
    /// </summary>
    public DateTimeOffset CreatedAt { get; private set; }
    /// <summary>
    /// Task completion timestamp
    /// </summary>
    public DateTimeOffset? CompletedAt { get; private set; }
    /// <summary>
    /// Task due timestamp
    /// </summary>
    public DateTimeOffset? DueDate { get; private set; }

    private TaskItem()
    {
        Title = string.Empty;
    }

    public TaskItem(string title, string? description, DateTimeOffset? dueDate)
    {
        ValidateTitle(title);
        ValidateDescription(description);
        ValidateDueDate(dueDate);
        
        Id = Guid.NewGuid();
        Title = title;
        Description = description;
        IsCompleted = false;
        CreatedAt = DateTimeOffset.UtcNow;
        CompletedAt = null;
        DueDate = dueDate;
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

    public void Update(string title, string? description, DateTimeOffset? dueDate)
    {
        Rename(title);
        ChangeDescription(description);
        ChangeDueDate(dueDate);
    }

    public void ChangeDueDate(DateTimeOffset? dueDate)
    {
        ValidateDueDate(dueDate);
        
        DueDate = dueDate;
    }

    private static void ValidateDueDate(DateTimeOffset? dueDate)
    {
        if (dueDate == null)
        {
            return;
        }

        if (dueDate.Value.Offset != TimeSpan.Zero)
        {
            throw new DomainValidationException(
                "dueDate",
                "DueDate must be in UTC");
        }

        if (dueDate.Value < DateTimeOffset.UtcNow)
        {
            throw new DomainValidationException(
                "dueDate",
                "DueDate cannot be in the past");
        }
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