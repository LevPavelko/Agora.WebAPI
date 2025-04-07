using Agora.BLL.DTO;
using Agora.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Agora.Controllers
{
    [Route("api/statistics")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        private readonly IStatisticsService _statisticsService;


        public StatisticsController(IStatisticsService statisticsService)

        {
            _statisticsService = statisticsService;
        }

        [HttpPost]
        public async Task<IActionResult> GetWeeklyStatistics([FromBody] int storeId)
        {
            List<WeeklyStatisticsDTO> categories = await _statisticsService.GetWeeksStatisticsBySales(storeId);
            return Ok(categories);
        }

        
        [HttpGet("revenue/current-month/{storeId}")]
        public async Task<ActionResult<List<DailyRevenueDTO>>> GetCurrentMonthRevenue(int storeId)
        {
            var result = await _statisticsService.GetCurrentMonthRevenue(storeId);
            return Ok(result);
        }

        
        [HttpGet("revenue/previous-month/{storeId}")]
        public async Task<ActionResult<List<DailyRevenueDTO>>> GetPreviousMonthRevenue(int storeId)
        {
            var result = await _statisticsService.GetPreviousMonthRevenue(storeId);
            return Ok(result);
        }
    }
}
