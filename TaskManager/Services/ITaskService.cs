using TaskManager.DTOs;

namespace TaskManager.Services
{
    public interface ITaskService
    {
        Task<IEnumerable<TaskResponse>> GetProjectTasksAsync(int projectId, int userId);
        Task<int> CreateTaskAsync(int projectId, CreateTaskRequest request, int userId);
        Task UpdateTaskAsync(int taskId, UpdateTaskRequest request, int userId);
        Task DeleteTaskAsync(int taskId, int userId);
        Task<IEnumerable<TaskHistoryResponse>> GetTaskHistoryAsync(int taskId, int userId);
        Task AddCommentToTaskAsync(int taskId, string comment, int userId);
    }
}