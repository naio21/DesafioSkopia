using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManager.Controllers;
using TaskManager.DTOs;
using TaskManager.Services;
using Xunit;

namespace TaskManager.Tests
{
    public class ReportsControllerTests
    {
        [Fact]
        public async Task GetPerformanceReport_ReturnsOk_WithReportData()
        {
            // Arrange
            var mockService = new Mock<IReportService>();
            var reportData = new List<PerformanceReportResponse>
            {
                new PerformanceReportResponse { UserId = 1, UserName = "Ivan B. Prado", CompletedTasksLast30Days = 10 }
            };
            mockService.Setup(s => s.GetUserPerformanceReportAsync(It.IsAny<int>()))
                .ReturnsAsync(reportData);

            var controller = new ReportsController(mockService.Object);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            controller.Request.Headers["X-User-Id"] = "1";

            // Act
            var result = await controller.GetPerformanceReport();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedData = Assert.IsAssignableFrom<IEnumerable<PerformanceReportResponse>>(okResult.Value);
            Assert.Single(returnedData);
        }

        [Fact]
        public async Task GetPerformanceReport_Throws_WhenUserIdHeaderMissing()
        {
            // Arrange
            var mockService = new Mock<IReportService>();
            var controller = new ReportsController(mockService.Object);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            // Act & Assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => controller.GetPerformanceReport());
        }
    }
}