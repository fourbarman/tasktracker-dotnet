using JetBrains.Annotations;
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

    private static (TaskService service, FakeRepository repository) CreateService()
    {
        var repository = new FakeRepository();
        var service = new TaskService(repository);
        
        return (service, repository);
    }
}