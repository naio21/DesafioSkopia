using TaskManager.DTOs;
using TaskManager.Repositories;

namespace TaskManager.Services.Implementations
{
    public class ReportService : IReportService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITaskRepository _taskRepository;

        public ReportService(IUserRepository userRepository, ITaskRepository taskRepository)
        {
            _userRepository = userRepository;
            _taskRepository = taskRepository;
        }

        public async Task<IEnumerable<PerformanceReportResponse>> GetUserPerformanceReportAsync(int requestingUserId)
        {
            var isManager = await _userRepository.IsManagerAsync(requestingUserId);
            if (!isManager)
            {
                throw new UnauthorizedAccessException("Only managers can access performance reports");
            }

            // Note: This would need to be implemented in the TaskRepository
            // For now, we'll return an empty list
            return new List<PerformanceReportResponse>();
        }
    }
}