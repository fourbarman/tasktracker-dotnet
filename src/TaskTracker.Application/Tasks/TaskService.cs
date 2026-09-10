using TaskTracker.Application.Common;
using TaskTracker.Domain.Tasks;

namespace TaskTracker.Application.Tasks;

public class TaskService
{
    private readonly ITaskRepository _taskRepository;

    public TaskService(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<TaskResponse?> UpdateAsync(
        Guid id, 
        UpdateTaskRequest request, 
        CancellationToken cancellationToken)
    {
        var task = await _taskRepository.GetByIdAsync(id, cancellationToken);
        
        if (task == null)
            return null;
        
        task.Update(request.Title, request.Description);
        
        await _taskRepository.SaveChangesAsync(cancellationToken);

        return ToResponse(task);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var task = await _taskRepository.GetByIdAsync(id, cancellationToken);
        
        if (task == null) return false;
        
        await _taskRepository.DeleteAsync(task, cancellationToken);
        await _taskRepository.SaveChangesAsync(cancellationToken);
        
        return true;
    }

    public async Task<TaskResponse> CreateAsync(CreateTaskRequest request, CancellationToken cancellationToken)
    {
        var task = new TaskItem(request.Title, request.Description);
        
        await _taskRepository.AddAsync(task, cancellationToken);
        await _taskRepository.SaveChangesAsync(cancellationToken);
        
        return ToResponse(task);
    }

    public async Task<PagedResponse<TaskResponse>> GetAllPagedAsync(GetTasksRequest request, CancellationToken cancellationToken)
    {
        var errors = new Dictionary<string, string[]>();

        if (request.Page <= 0)
        {
            errors["page"] = ["Page number cannot be less than zero"];
        }

        if (request.PageSize <= 0 || request.PageSize > 100)
        {
            errors["pageSize"] = ["Page size cannot be less than zero and greater than 100"];
        }

        if (errors.Count > 0)
        {
            throw new ApplicationValidationException(errors);
        }
        
        var skip = (request.Page - 1) * request.PageSize;
        
        var tasks = await _taskRepository.GetPagedAsync(
            skip,
            request.PageSize,
            request.IsCompleted,
            cancellationToken);
        
        var totalCount = await _taskRepository.CountAsync(request.IsCompleted, cancellationToken);
        
        var items = tasks
            .Select(ToResponse)
            .ToList();
        
        return new PagedResponse<TaskResponse>(
            items, 
            request.Page, 
            request.PageSize, 
            totalCount);
    }

    public async Task<TaskResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var task = await _taskRepository.GetByIdAsync(id, cancellationToken);
        if (task is null)
        {
            return null;
        }
        
        return ToResponse(task);
    }

    public async Task<bool> CompleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var task = await _taskRepository.GetByIdAsync(id, cancellationToken);

        if (task is null)
        {
            return false;
        }
        
        task.Complete();
        
        await _taskRepository.SaveChangesAsync(cancellationToken);
        
        return true;
    }

    private static TaskResponse ToResponse(TaskItem task)
    {
        return new TaskResponse(
            task.Id,
            task.Title,
            task.Description,
            task.IsCompleted,
            task.CreatedAt,
            task.CompletedAt
        );
    }
}