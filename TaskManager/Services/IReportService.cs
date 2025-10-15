using TaskManager.DTOs;

namespace TaskManager.Services
{
    public interface IReportService
    {
        Task<IEnumerable<PerformanceReportResponse>> GetUserPerformanceReportAsync(int requestingUserId);
    }
}