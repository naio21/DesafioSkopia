using Dapper;
using Microsoft.Data.SqlClient;

namespace TaskManager.Repositories.Implementations
{
    public class TaskRepository : ITaskRepository
    {
        private readonly string _connectionString;

        public TaskRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<IEnumerable<Models.TaskItem>> GetProjectTasksAsync(int projectId)
        {
            using var connection = new SqlConnection(_connectionString);
            const string sql = @"
            SELECT TaskId, ProjectId, Title, Description, DueDate, Status, Priority, CreatedBy, CreatedAt 
            FROM Tasks 
            WHERE ProjectId = @ProjectId";

            return await connection.QueryAsync<Models.TaskItem>(sql, new { ProjectId = projectId });
        }

        public async Task<Models.TaskItem?> GetByIdAsync(int taskId)
        {
            using var connection = new SqlConnection(_connectionString);
            const string sql = @"
            SELECT TaskId, ProjectId, Title, Description, DueDate, Status, Priority, CreatedBy, CreatedAt 
            FROM Tasks 
            WHERE TaskId = @TaskId";

            return await connection.QueryFirstOrDefaultAsync<Models.TaskItem>(sql, new { TaskId = taskId });
        }

        public async Task<int> CreateAsync(Models.TaskItem task)
        {
            using var connection = new SqlConnection(_connectionString);
            const string sql = @"
            INSERT INTO Tasks (ProjectId, Title, Description, DueDate, Status, Priority, CreatedBy, CreatedAt) 
            VALUES (@ProjectId, @Title, @Description, @DueDate, @Status, @Priority, @CreatedBy, @CreatedAt);
            SELECT CAST(SCOPE_IDENTITY() as int)";

            return await connection.QuerySingleAsync<int>(sql, task);
        }

        public async Task<bool> UpdateAsync(Models.TaskItem task)
        {
            using var connection = new SqlConnection(_connectionString);
            const string sql = @"
            UPDATE Tasks 
            SET Title = @Title,
                Description = @Description,
                DueDate = @DueDate,
                Status = @Status
            WHERE TaskId = @TaskId";

            var result = await connection.ExecuteAsync(sql, task);
            return result > 0;
        }

        public async Task<bool> DeleteAsync(int taskId)
        {
            using var connection = new SqlConnection(_connectionString);
            const string sql = "DELETE FROM Tasks WHERE TaskId = @TaskId";

            var result = await connection.ExecuteAsync(sql, new { TaskId = taskId });
            return result > 0;
        }

        public async Task<int> GetTaskCountForProjectAsync(int projectId)
        {
            using var connection = new SqlConnection(_connectionString);
            const string sql = "SELECT COUNT(1) FROM Tasks WHERE ProjectId = @ProjectId";

            return await connection.ExecuteScalarAsync<int>(sql, new { ProjectId = projectId });
        }

        public async Task<IEnumerable<Models.PerformanceReport>> GetPerformanceReportsAsync()
        {
            using var connection = new SqlConnection(_connectionString);
            const string sql = @"
            SELECT
	            SUM(T.TaskId) AS CompletedTasksLast30Days,
	            U.UserId,
	            U.Name AS UserName
            FROM Tasks T
            INNER JOIN Users U ON T.CreatedBy = U.UserId
            WHERE T.Status = 2 AND DATEDIFF(day, T.CreatedAt, GETDATE()) <= 30
            GROUP BY U.UserId, U.Name";
 
            return await connection.QueryAsync<Models.PerformanceReport>(sql);
        }
    }
}