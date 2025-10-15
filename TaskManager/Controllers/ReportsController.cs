using Microsoft.AspNetCore.Mvc;
using TaskManager.DTOs;
using TaskManager.Services;

namespace TaskManager.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _reportService;
        private const string USER_ID_HEADER = "X-User-Id";

        public ReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }

        private int GetUserId()
        {
            if (!Request.Headers.TryGetValue(USER_ID_HEADER, out var userIdHeader))
            {
                throw new UnauthorizedAccessException("O ID do usuário é obrigatório.");
            }

            if (!int.TryParse(userIdHeader, out int userId))
            {
                throw new UnauthorizedAccessException("ID de usuário inválido");
            }

            return userId;
        }

        [HttpGet("performance")]
        public async Task<ActionResult<IEnumerable<PerformanceReportResponse>>> GetPerformanceReport()
        {
            var userId = GetUserId();
            var report = await _reportService.GetUserPerformanceReportAsync(userId);
            return Ok(report);
        }
    }
}