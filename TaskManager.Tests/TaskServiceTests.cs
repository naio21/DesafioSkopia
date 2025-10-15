using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using TaskManager.DTOs;
using TaskManager.Models;
using TaskManager.Repositories;
using TaskManager.Services.Implementations;
using Xunit;

namespace TaskManager.Tests;

public class TaskServiceTests
{
    private readonly Mock<ITaskRepository> _taskRepo = new();
    private readonly Mock<ITaskHistoryRepository> _historyRepo = new();
    private readonly Mock<IProjectRepository> _projectRepo = new();
    private readonly Mock<IUserRepository> _userRepo = new();
    private readonly TaskService _service;

    public TaskServiceTests()
    {
        _service = new TaskService(_taskRepo.Object, _historyRepo.Object, _projectRepo.Object, _userRepo.Object);
    }

    [Fact]
    public async Task CreateTask_ShouldThrow_WhenTaskLimitReached()
    {
        _projectRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new Project { ProjectId = 1 });
        _taskRepo.Setup(r => r.GetTaskCountForProjectAsync(1)).ReturnsAsync(20);
        var req = new CreateTaskRequest { Title = "Test", Priority = TaskPriority.Low };
        await Assert.ThrowsAsync<InvalidOperationException>(() => _service.CreateTaskAsync(1, req, 1));
    }

    [Fact]
    public async Task CreateTask_ShouldCreate_WhenUnderLimit()
    {
        _projectRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new Project { ProjectId = 1 });
        _taskRepo.Setup(r => r.GetTaskCountForProjectAsync(1)).ReturnsAsync(5);
        _taskRepo.Setup(r => r.CreateAsync(It.IsAny<TaskItem>())).ReturnsAsync(99);
        _historyRepo.Setup(r => r.CreateAsync(It.IsAny<TaskHistory>())).Returns(Task.CompletedTask);
        var req = new CreateTaskRequest { Title = "Test", Priority = TaskPriority.High };
        var id = await _service.CreateTaskAsync(1, req, 1);
        Assert.Equal(99, id);
    }

    [Fact]
    public async Task UpdateTask_ShouldRecordHistory_WhenFieldsChange()
    {
        var task = new TaskItem { TaskId = 1, Title = "Old", Description = "Desc", Status = TaskManager.Models.TaskStatus.Pending };
        _taskRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(task);
        _taskRepo.Setup(r => r.UpdateAsync(It.IsAny<TaskItem>())).ReturnsAsync(true);
        _historyRepo.Setup(r => r.CreateAsync(It.IsAny<TaskHistory>())).Returns(Task.CompletedTask);
        var req = new UpdateTaskRequest { Title = "New", Status = TaskManager.Models.TaskStatus.Completed };
        await _service.UpdateTaskAsync(1, req, 2);
        _historyRepo.Verify(r => r.CreateAsync(It.Is<TaskHistory>(h => h.FieldChanged == "Title")), Times.Once);
        _historyRepo.Verify(r => r.CreateAsync(It.Is<TaskHistory>(h => h.FieldChanged == "Status")), Times.Once);
    }

    [Fact]
    public async Task DeleteTask_ShouldThrow_WhenTaskNotFound()
    {
        _taskRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((TaskItem)null);
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.DeleteTaskAsync(1, 1));
    }

    [Fact]
    public async Task AddCommentToTask_ShouldRecordComment()
    {
        var task = new TaskItem { TaskId = 1 };
        _taskRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(task);
        _historyRepo.Setup(r => r.CreateAsync(It.IsAny<TaskHistory>())).Returns(Task.CompletedTask);
        await _service.AddCommentToTaskAsync(1, "Test comment", 2);
        _historyRepo.Verify(r => r.CreateAsync(It.Is<TaskHistory>(h => h.FieldChanged == "Comment" && h.Comment == "Test comment")), Times.Once);
    }
}
