namespace TaskManager.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsManager { get; set; }
    }
}