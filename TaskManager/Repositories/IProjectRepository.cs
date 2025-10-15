using TaskManager.Models;

namespace TaskManager.Repositories
{
    public interface IProjectRepository
    {
        Task<IEnumerable<Project>> GetUserProjectsAsync(int userId);
        Task<Project?> GetByIdAsync(int projectId);
        Task<int> CreateAsync(Project project);
        Task<bool> HasPendingTasksAsync(int projectId);
        Task<int> GetTaskCountAsync(int projectId);
    }
}