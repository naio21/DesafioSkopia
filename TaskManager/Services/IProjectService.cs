using TaskManager.DTOs;

namespace TaskManager.Services
{
    public interface IProjectService
    {
        Task<ProjectResponse> GetProjectByIdAsync(int projectId);
        Task<IEnumerable<ProjectResponse>> GetUserProjectsAsync(int userId);
        Task<ProjectResponse> CreateProjectAsync(CreateProjectRequest request, int userId);
    }
}