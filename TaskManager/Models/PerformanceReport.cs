namespace TaskManager.Models
{
    public class PerformanceReport
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public int CompletedTasksLast30Days { get; set; }
    }
}