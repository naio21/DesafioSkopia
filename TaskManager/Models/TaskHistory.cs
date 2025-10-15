namespace TaskManager.Models
{
    public class TaskHistory
    {
        public int HistoryId { get; set; }
        public int TaskId { get; set; }
        public string FieldChanged { get; set; } = string.Empty;
        public DateTime ChangedAt { get; set; }
        public int ChangedBy { get; set; }
        public string? Comment { get; set; }
    }
}