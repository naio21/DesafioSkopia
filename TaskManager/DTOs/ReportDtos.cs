namespace TaskManager.DTOs
{
    public class PerformanceReportResponse
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public int CompletedTasksLast30Days { get; set; }
        public double AverageTasksPerDay { get; set; }
    }
}