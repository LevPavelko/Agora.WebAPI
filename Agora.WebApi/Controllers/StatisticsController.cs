using Agora.BLL.DTO;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using System.Text.Json;

namespace Agora.Controllers
{
    [Route("api/statistics")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        private readonly IConnectionMultiplexer _redis;

        public StatisticsController(IConnectionMultiplexer redis)
        {
            _redis = redis;
        }

        [HttpGet("weekly/{storeId}")]
        public async Task<IActionResult> GetWeekly(int storeId)
        {
            var db = _redis.GetDatabase();
            var json = await db.StringGetAsync($"weekly_stats:{storeId}");

            if (json.IsNullOrEmpty)
                return NotFound("Statistics was not found or has not yet been calculated.");

            var data = JsonSerializer.Deserialize<List<WeeklyStatisticsDTO>>(json);
            return Ok(data);
        }

        [HttpGet("revenue/monthly/previous/{storeId}")]
        public async Task<IActionResult> GetPreviousMonthRevenue(int storeId)
        {
            var db = _redis.GetDatabase();
            var date = DateTime.Now.AddMonths(-1);
            var key = $"monthly_revenue:{date.Year}-{date.Month:D2}:{storeId}";

            var json = await db.StringGetAsync(key);
            if (json.IsNullOrEmpty)
                return NotFound("Revenue data for the previous month was not found or has not yet been calculated.");

            var data = JsonSerializer.Deserialize<List<DailyRevenueDTO>>(json);
            return Ok(data);
        }

        [HttpGet("revenue/monthly/before-previous/{storeId}")]
        public async Task<IActionResult> GetBeforePreviousMonthRevenue(int storeId)
        {
            var db = _redis.GetDatabase();
            var date = DateTime.Now.AddMonths(-2);
            var key = $"monthly_revenue:{date.Year}-{date.Month:D2}:{storeId}";

            var json = await db.StringGetAsync(key);
            if (json.IsNullOrEmpty)
                return NotFound("Revenue data for the preprevious month was not found or has not yet been calculated.");

            var data = JsonSerializer.Deserialize<List<DailyRevenueDTO>>(json);
            return Ok(data);
        }
    }
}
