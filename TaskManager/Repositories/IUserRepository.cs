using TaskManager.Models;

namespace TaskManager.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(int userId);
        Task<bool> IsManagerAsync(int userId);
    }
}