using TaskTracker.Domain.Common;
using TaskTracker.Domain.Tasks;

namespace TaskTracker.Tests.Domain.Tasks;

public class TaskItemTests
{
    [Fact]
    public void Constructor_WhenTitleIsValid_CreateTask()
    {
        //Arrange
        var title = "Sample title";
        var description = "Sample description";
        
        //Act
        var task = new TaskItem(title, description);
        
        //Assert
        Assert.NotEqual(Guid.Empty, task.Id);
        Assert.Equal(title, task.Title);
        Assert.Equal(description, task.Description);
        Assert.False(task.IsCompleted);
        Assert.Null(task.CompletedAt);
    }

    [Fact]
    public void Constructor_WhenTitleIsEmpty_ThrowException()
    {
        //Arrange
        var title = "";

        //Act
        var action = () => new TaskItem(title, null);

        //Assert
        Assert.Throws<DomainValidationException>(action);
    }

    [Fact]
    public void Complete_WhenTaskIsNotCompleted_MarksTaskAsCompleted()
    {
        //Arrange
        var task = new TaskItem("Sample title", null);
        
        //Act
        task.Complete();
        
        //Assert
        Assert.True(task.IsCompleted);
        Assert.NotNull(task.CompletedAt);
    }

    [Fact]
    public void Rename_WhenTaskTitleIsValid_ChangeTitle()
    {
        //Arrange
        var task = new TaskItem("Old title", null);
        
        //Act
        task.Rename("New title");
        
        //Assert
        Assert.Equal("New title", task.Title);
    }

    [Fact]
    public void Rename_WhenTaskTitleIsEmpty_ThrowException()
    {
        //Arrange
        var task = new TaskItem("Old title", null);
        
        //Act
        var action = () => task.Rename("");
        
        //Assert
        Assert.Throws<DomainValidationException>(action);
    }

    [Fact]
    public void Constructor_WhenTitleIsTooLong_ThrowsDomainValidationException()
    {
        var title = new string('a', 201);
        
        var action = () => new TaskItem(title, null);
        
        Assert.Throws<DomainValidationException>(action);
    }
    
    [Fact]
    public void Constructor_WhenDescriptionIsTooLong_ThrowsDomainValidationException()
    {
        var description = new string('a', 2001);
        
        var action = () => new TaskItem("Valid title", description);
        
        Assert.Throws<DomainValidationException>(action);
    }
    
    [Fact]
    public void ChangeDescription_WhenTitleAndDescriptionAreValid_ChangeDescription()
    {
        var task = new TaskItem("Valid title", null);
        
        task.ChangeDescription("Valid description");
        
        Assert.Equal("Valid description", task.Description);
    }
    
    [Fact]
    public void ChangeDescription_WhenTitleIsValidAndDescriptionIsTooLong_ThrowsDomainValidationException()
    {
        var task = new TaskItem("Valid title", null);
        var description = new string('a', 2001);
        
        var action = () => task.ChangeDescription(description);
        
        Assert.Throws<DomainValidationException>(action);
    }
}