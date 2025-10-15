using TaskManager.DTOs;
using TaskManager.Models;
using TaskManager.Repositories;

namespace TaskManager.Services.Implementations
{
    public class TaskService : ITaskService
    {
        private const int MAX_TASKS_PER_PROJECT = 20;
        private readonly ITaskRepository _taskRepository;
        private readonly ITaskHistoryRepository _taskHistoryRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IUserRepository _userRepository;

        public TaskService(
            ITaskRepository taskRepository,
            ITaskHistoryRepository taskHistoryRepository,
            IProjectRepository projectRepository,
            IUserRepository userRepository)
        {
            _taskRepository = taskRepository;
            _taskHistoryRepository = taskHistoryRepository;
            _projectRepository = projectRepository;
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<TaskResponse>> GetProjectTasksAsync(int projectId, int userId)
        {
            var project = await _projectRepository.GetByIdAsync(projectId)
                ?? throw new KeyNotFoundException("Project not found");

            var tasks = await _taskRepository.GetProjectTasksAsync(projectId);
            return tasks.Select(t => new TaskResponse
            {
                TaskId = t.TaskId,
                ProjectId = t.ProjectId,
                Title = t.Title,
                Description = t.Description,
                DueDate = t.DueDate,
                Status = t.Status,
                Priority = t.Priority,
                CreatedAt = t.CreatedAt,
                CreatedBy = t.CreatedBy
            });
        }

        public async Task<int> CreateTaskAsync(int projectId, CreateTaskRequest request, int userId)
        {
            var project = await _projectRepository.GetByIdAsync(projectId)
                ?? throw new KeyNotFoundException("Project not found");

            var taskCount = await _taskRepository.GetTaskCountForProjectAsync(projectId);
            if (taskCount >= MAX_TASKS_PER_PROJECT)
            {
                throw new InvalidOperationException($"Project has reached the maximum limit of {MAX_TASKS_PER_PROJECT} tasks");
            }

            var task = new TaskItem
            {
                ProjectId = projectId,
                Title = request.Title,
                Description = request.Description,
                DueDate = request.DueDate,
                Status = Models.TaskStatus.Pendente,
                Priority = request.Priority,
                CreatedBy = userId,
                CreatedAt = DateTime.UtcNow
            };

            var taskId = await _taskRepository.CreateAsync(task);

            await _taskHistoryRepository.CreateAsync(new TaskHistory
            {
                TaskId = taskId,
                FieldChanged = "Created",
                ChangedAt = DateTime.UtcNow,
                ChangedBy = userId
            });

            return taskId;
        }

        public async Task UpdateTaskAsync(int taskId, UpdateTaskRequest request, int userId)
        {
            var task = await _taskRepository.GetByIdAsync(taskId)
                ?? throw new KeyNotFoundException("Task not found");

            var changes = new List<string>();

            if (request.Title != null && request.Title != task.Title)
            {
                changes.Add("Title");
                task.Title = request.Title;
            }

            if (request.Description != null && request.Description != task.Description)
            {
                changes.Add("Description");
                task.Description = request.Description;
            }

            if (request.DueDate.HasValue && request.DueDate != task.DueDate)
            {
                changes.Add("DueDate");
                task.DueDate = request.DueDate;
            }

            if (request.Status.HasValue && request.Status.Value != task.Status)
            {
                changes.Add("Status");
                task.Status = request.Status.Value;
            }

            if (changes.Any())
            {
                await _taskRepository.UpdateAsync(task);

                foreach (var change in changes)
                {
                    await _taskHistoryRepository.CreateAsync(new TaskHistory
                    {
                        TaskId = taskId,
                        FieldChanged = change,
                        ChangedAt = DateTime.UtcNow,
                        ChangedBy = userId
                    });
                }
            }
        }

        public async Task DeleteTaskAsync(int taskId, int userId)
        {
            var task = await _taskRepository.GetByIdAsync(taskId)
                ?? throw new KeyNotFoundException("Task not found");

            await _taskRepository.DeleteAsync(taskId);
        }

        public async Task<IEnumerable<TaskHistoryResponse>> GetTaskHistoryAsync(int taskId, int userId)
        {
            var task = await _taskRepository.GetByIdAsync(taskId)
                ?? throw new KeyNotFoundException("Task not found");

            var history = await _taskHistoryRepository.GetTaskHistoryAsync(taskId);
            return history.Select(h => new TaskHistoryResponse
            {
                FieldChanged = h.FieldChanged,
                ChangedAt = h.ChangedAt,
                ChangedBy = h.ChangedBy,
                Comment = h.Comment
            });
        }

        public async Task AddCommentToTaskAsync(int taskId, string comment, int userId)
        {
            var task = await _taskRepository.GetByIdAsync(taskId)
                ?? throw new KeyNotFoundException("Task not found");

            await _taskHistoryRepository.CreateAsync(new TaskHistory
            {
                TaskId = taskId,
                FieldChanged = "Comment",
                ChangedAt = DateTime.UtcNow,
                ChangedBy = userId,
                Comment = comment
            });
        }
    }
}