using Dapper;
using Microsoft.Data.SqlClient;
using TaskManager.Models;

namespace TaskManager.Repositories.Implementations
{
    public class TaskHistoryRepository : ITaskHistoryRepository
    {
        private readonly string _connectionString;

        public TaskHistoryRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<IEnumerable<TaskHistory>> GetTaskHistoryAsync(int taskId)
        {
            using var connection = new SqlConnection(_connectionString);
            const string sql = @"
            SELECT HistoryId, TaskId, FieldChanged, ChangedAt, ChangedBy, Comment 
            FROM TaskHistory 
            WHERE TaskId = @TaskId 
            ORDER BY ChangedAt DESC";

            return await connection.QueryAsync<TaskHistory>(sql, new { TaskId = taskId });
        }

        public async Task CreateAsync(TaskHistory history)
        {
            using var connection = new SqlConnection(_connectionString);
            const string sql = @"
            INSERT INTO TaskHistory (TaskId, FieldChanged, ChangedAt, ChangedBy, Comment) 
            VALUES (@TaskId, @FieldChanged, @ChangedAt, @ChangedBy, @Comment)";

            await connection.ExecuteAsync(sql, history);
        }
    }
}