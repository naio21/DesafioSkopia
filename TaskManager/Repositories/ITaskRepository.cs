using TaskManager.Models;

namespace TaskManager.Repositories
{
    public interface ITaskRepository
    {
        Task<IEnumerable<TaskItem>> GetProjectTasksAsync(int projectId);
        Task<TaskItem?> GetByIdAsync(int taskId);
        Task<int> CreateAsync(TaskItem task);
        Task<bool> UpdateAsync(TaskItem task);
        Task<bool> DeleteAsync(int taskId);
        Task<int> GetTaskCountForProjectAsync(int projectId);
        Task<IEnumerable<PerformanceReport>> GetPerformanceReportsAsync();
    }
}