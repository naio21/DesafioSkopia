using Dapper;
using Microsoft.Data.SqlClient;
using TaskManager.Models;

namespace TaskManager.Repositories.Implementations
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly string _connectionString;

        public ProjectRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<IEnumerable<Project>> GetUserProjectsAsync(int userId)
        {
            using var connection = new SqlConnection(_connectionString);
            const string sql = @"
            SELECT ProjectId, Name, Description, CreatedBy, CreatedAt 
            FROM Projects 
            WHERE CreatedBy = @UserId";

            return await connection.QueryAsync<Project>(sql, new { UserId = userId });
        }

        public async Task<Project?> GetByIdAsync(int projectId)
        {
            using var connection = new SqlConnection(_connectionString);
            const string sql = @"
            SELECT ProjectId, Name, Description, CreatedBy, CreatedAt 
            FROM Projects 
            WHERE ProjectId = @ProjectId";

            return await connection.QueryFirstOrDefaultAsync<Project>(sql, new { ProjectId = projectId });
        }

        public async Task<int> CreateAsync(Project project)
        {
            using var connection = new SqlConnection(_connectionString);
            const string sql = @"
            INSERT INTO Projects (Name, Description, CreatedBy, CreatedAt) 
            VALUES (@Name, @Description, @CreatedBy, @CreatedAt);
            SELECT CAST(SCOPE_IDENTITY() as int)";

            return await connection.QuerySingleAsync<int>(sql, project);
        }

        public async Task<bool> HasPendingTasksAsync(int projectId)
        {
            using var connection = new SqlConnection(_connectionString);
            const string sql = @"
            SELECT COUNT(1) 
            FROM Tasks 
            WHERE ProjectId = @ProjectId 
            AND Status != 'Completed'";

            var count = await connection.ExecuteScalarAsync<int>(sql, new { ProjectId = projectId });
            return count > 0;
        }

        public async Task<int> GetTaskCountAsync(int projectId)
        {
            using var connection = new SqlConnection(_connectionString);
            const string sql = "SELECT COUNT(1) FROM Tasks WHERE ProjectId = @ProjectId";

            return await connection.ExecuteScalarAsync<int>(sql, new { ProjectId = projectId });
        }
    }
}