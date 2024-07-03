using EXE201.BLL.Interfaces;
using EXE201.BLL.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EXE201.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : Controller
    {
        private readonly IDashboardServices _dashboardService;

        public DashboardController(IDashboardServices dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet("TotalRevenue")]
        public IActionResult GetTotalRevenueAllTime()
        {
            var revenue = _dashboardService.GetTotalRevenueAllTime();
            return Ok(new { TotalRevenue = revenue });
        }

        [HttpGet("MonthlyRevenue2024")]
        public IActionResult GetMonthlyRevenue2024()
        {
            var revenue = _dashboardService.GetMonthlyRevenue2024();
            return Ok(revenue);
        }

        [HttpGet("RevenueLast7Days")]
        public IActionResult GetRevenueLast7Days()
        {
            var revenue = _dashboardService.GetRevenueLast7Days();
            return Ok(revenue);
        }
    }
}
