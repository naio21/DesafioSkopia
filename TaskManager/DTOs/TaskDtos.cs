using TaskManager.Models;

namespace TaskManager.DTOs
{
    public class CreateTaskRequest
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }
        public TaskPriority Priority { get; set; }
    }

    public class UpdateTaskRequest
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }
        public Models.TaskStatus? Status { get; set; }
    }

    public class TaskResponse
    {
        public int TaskId { get; set; }
        public int ProjectId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }
        public Models.TaskStatus Status { get; set; }
        public TaskPriority Priority { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }
    }

    public class TaskHistoryResponse
    {
        public string FieldChanged { get; set; } = string.Empty;
        public DateTime ChangedAt { get; set; }
        public int ChangedBy { get; set; }
        public string? Comment { get; set; }
    }
}