using JetBrains.Annotations;
using TaskTracker.Application.Common;
using TaskTracker.Application.Tasks;

namespace TaskTracker.Tests.Application.Tasks;

public class TaskServiceTest
{
    [Fact]
    public async Task CreateAsync_WhenRequestIsValid_CreatesTask()
    {
        var (service, _) = CreateService();

        var request = new CreateTaskRequest(
            "Test title",
            "Test content"
        );

        var result = await service.CreateAsync(
            request, 
            CancellationToken.None);
        
        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal("Test title", result.Title);
        Assert.Equal("Test content", result.Description);
        Assert.False(result.IsCompleted);
        Assert.Null(result.CompletedAt);
    }

    [Fact]
    public async Task GetByIdAsync_WhenTaskExists_ReturnsTask()
    {
        var (service, _) = CreateService();

        var created = await service.CreateAsync(
            new CreateTaskRequest("Test title", "Test content"),
            CancellationToken.None
            );

        var result = await service.GetByIdAsync(
            created.Id,
            CancellationToken.None);
        
        Assert.NotNull(result);
        Assert.Equal(created.Id, result.Id);
        Assert.Equal("Test title", result.Title);
    }

    [Fact]
    public async Task GetByIdAsync_WhenTaskDoesNotExist_ReturnsNull()
    {
        var (service, _) = CreateService();
        
        var result = await service.GetByIdAsync(
            Guid.NewGuid(),
            CancellationToken.None);
        
        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateAsync_WhenTaskExists_UpdatesTitleAndDescription()
    {
        var (service, _) = CreateService();

        var created = await service.CreateAsync(
            new CreateTaskRequest("Test title", "Test content"),
            CancellationToken.None);

        var result = await service.UpdateAsync(
            created.Id,
            new UpdateTaskRequest("New Test Title", "New Test Description"),
            CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(created.Id, result.Id);
        Assert.Equal("New Test Title", result.Title);
    }

    [Fact]
    public async Task CompleteAsync_WhenTaskExists_CompletesTask()
    {
        var (service, _) = CreateService();

        var created = await service.CreateAsync(
            new CreateTaskRequest("Test title", "Test content"),
            CancellationToken.None);

        var completed = await service.CompleteAsync(
            created.Id,
            CancellationToken.None
            );

        var result = await service.GetByIdAsync(
            created.Id,
            CancellationToken.None);
        
        Assert.True(completed);
        Assert.NotNull(result);
        Assert.True(result.IsCompleted);
        Assert.NotNull(result.CompletedAt);
    }
    
    [Fact]
    public async Task DeleteAsync_WhenTaskExists_DeletesTask()
    {
        var (service, _) = CreateService();
        
        var created = await service.CreateAsync(
            new CreateTaskRequest("Test title", "Test content"),
            CancellationToken.None);

        var deleted = await service.DeleteAsync(
            created.Id,
            CancellationToken.None
            );

        var result = await service.GetByIdAsync(
            created.Id,
            CancellationToken.None
            );
        
        Assert.True(deleted);
        Assert.Null(result);
    }
    /// <summary>
    /// Negative
    /// </summary>
    [Fact]
    public async Task UpdateAsync_WhenTaskDoesNotExist_ReturnsNull()
    {
        var (service, _) = CreateService();

        var result = await service.UpdateAsync(
            Guid.NewGuid(), 
            new UpdateTaskRequest("New Test Title", "New Test Content"),
            CancellationToken.None
            );
        
        Assert.Null(result);
    }

    [Fact]
    public async Task CompleteAsync_WhenTaskDoesNotExist_ReturnsFalse()
    {
        var (service, _) = CreateService();

        var result = await service.CompleteAsync(
            Guid.NewGuid(), 
            CancellationToken.None
            );
        
        Assert.False(result);
    }

    [Fact]
    public async Task DeleteAsync_WhenTaskDoesNotExist_ReturnsFalse()
    {
        var (service, _) = CreateService();
        
        var result = await service.DeleteAsync(
            Guid.NewGuid(),
            CancellationToken.None
            );
        
        Assert.False(result);
    }

    /// <summary>
    /// Pagination & Foltering
    /// </summary>

    [Fact]
    public async Task GetAllPagedAsync_WhenIsCompletedFilterIdSet_ReturnsFilteredTasks()
    {
        var (service, _) = CreateService();

        var first = await service.CreateAsync(
            new  CreateTaskRequest("Test title 1", "Test content 1"),
            CancellationToken.None
            );

        var second = await service.CreateAsync(
            new  CreateTaskRequest("Test title 2", "Test content 2"),
            CancellationToken.None
            );
        
        await service.CompleteAsync(second.Id, CancellationToken.None);

        var result = await service.GetAllPagedAsync(
            new GetTasksRequest(
                Page: 1,
                PageSize: 20,
                IsCompleted: true),
            CancellationToken.None
            );
        
        Assert.Equal(1, result.TotalCount);
        Assert.Single(result.Items);
        Assert.Equal(second.Id, result.Items[0].Id);
        Assert.True(result.Items[0].IsCompleted);
    }

    /// <summary>
    /// ApplicationValidationException check
    /// </summary>
    
    [Fact]
    public async Task GetAllPagedAsync_WhenPageIsInvalid_ThrowsApplicationValidationException()
    {
        var (service, _) = CreateService();

        var action = async () => await service.GetAllPagedAsync(
            new GetTasksRequest(
                Page: 0,
                PageSize: 20,
                IsCompleted: null),
            CancellationToken.None
        );

        await Assert.ThrowsAsync<ApplicationValidationException>(action);
    }
    
    [Fact]
    public async Task GetAllPagedAsync_WhenPageSizeIsInvalid_ThrowsApplicationValidationException()
    {
        var (service, _) = CreateService();

        var action = async () => await service.GetAllPagedAsync(
            new GetTasksRequest(
                Page: 1,
                PageSize: 101,
                IsCompleted: null),
            CancellationToken.None
        );

        await Assert.ThrowsAsync<ApplicationValidationException>(action);
    }

    private static (TaskService service, FakeRepository repository) CreateService()
    {
        var repository = new FakeRepository();
        var service = new TaskService(repository);
        
        return (service, repository);
    }
}