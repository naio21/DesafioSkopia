using TaskManager.Models;

namespace TaskManager.Repositories
{
    public interface ITaskHistoryRepository
    {
        Task<IEnumerable<TaskHistory>> GetTaskHistoryAsync(int taskId);
        Task CreateAsync(TaskHistory history);
    }
}