using Dapper;
using Microsoft.Data.SqlClient;
using TaskManager.Models;

namespace TaskManager.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;

        public UserRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<User?> GetByIdAsync(int userId)
        {
            using var connection = new SqlConnection(_connectionString);
            const string sql = "SELECT UserId, Name, IsManager FROM Users WHERE UserId = @UserId";

            return await connection.QueryFirstOrDefaultAsync<User>(sql, new { UserId = userId });
        }

        public async Task<bool> IsManagerAsync(int userId)
        {
            using var connection = new SqlConnection(_connectionString);
            const string sql = "SELECT IsManager FROM Users WHERE UserId = @UserId";

            return await connection.ExecuteScalarAsync<bool>(sql, new { UserId = userId });
        }
    }
}