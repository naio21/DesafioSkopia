using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using TaskManager.DTOs;
using TaskManager.Models;
using TaskManager.Repositories;
using TaskManager.Services.Implementations;
using Xunit;

namespace TaskManager.Tests;

public class ProjectServiceTests
{
    private readonly Mock<IProjectRepository> _projectRepo = new();
    private readonly Mock<IUserRepository> _userRepo = new();
    private readonly ProjectService _service;

    public ProjectServiceTests()
    {
        _service = new ProjectService(_projectRepo.Object, _userRepo.Object);
    }

    [Fact]
    public async Task GetUserProjects_ShouldReturnProjects()
    {
        _projectRepo.Setup(r => r.GetUserProjectsAsync(1)).ReturnsAsync(new List<Project> {
            new Project { ProjectId = 1, Name = "P1", CreatedAt = System.DateTime.UtcNow },
            new Project { ProjectId = 2, Name = "P2", CreatedAt = System.DateTime.UtcNow }
        });
        _projectRepo.Setup(r => r.GetTaskCountAsync(It.IsAny<int>())).ReturnsAsync(5);
        var result = await _service.GetUserProjectsAsync(1);
        Assert.Equal(2, ((List<ProjectResponse>)result).Count);
        Assert.All(result, p => Assert.Equal(5, p.TaskCount));
    }

    [Fact]
    public async Task CreateProject_ShouldThrow_WhenUserNotFound()
    {
        _userRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((User)null);
        var req = new CreateProjectRequest { Name = "Test" };
        await Assert.ThrowsAsync<System.UnauthorizedAccessException>(() => _service.CreateProjectAsync(req, 1));
    }

    [Fact]
    public async Task CreateProject_ShouldCreate_WhenUserExists()
    {
        _userRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new User { UserId = 1 });
        _projectRepo.Setup(r => r.CreateAsync(It.IsAny<Project>())).ReturnsAsync(99);
        _projectRepo.Setup(r => r.GetByIdAsync(99)).ReturnsAsync(new Project { ProjectId = 99, Name = "Test", CreatedAt = System.DateTime.UtcNow });
        var req = new CreateProjectRequest { Name = "Test" };
        var ret = await _service.CreateProjectAsync(req, 1);
        Assert.Equal(99, ret.ProjectId);
    }
}
