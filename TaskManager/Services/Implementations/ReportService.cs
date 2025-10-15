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
                throw new UnauthorizedAccessException("Somente usuários de nível Gerente podem acessar os relatórios");
            }

            var report = await _taskRepository.GetPerformanceReportsAsync();
            return report.Select(r => new PerformanceReportResponse
            {
                UserId = r.UserId,
                UserName = r.UserName,
                CompletedTasksLast30Days = r.CompletedTasksLast30Days
            });
        }
    }
}