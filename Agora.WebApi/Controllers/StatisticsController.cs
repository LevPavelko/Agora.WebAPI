using Agora.BLL.DTO;
using Agora.BLL.Interfaces;
using Agora.BLL.Services;
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
        private readonly IStatisticsService _statisticsService;

        public StatisticsController(IConnectionMultiplexer redis, IStatisticsService statisticsService)
        {
            _redis = redis;
            _statisticsService = statisticsService;
        }

        [HttpGet("weekly-by-sales/{storeId}")]
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

        [HttpGet("info-abt-stores/{sellerId}")]
        public async Task<ActionResult<List<GeneralInfoAbtStoreDTO>>> GetInfoAbtStores(int sellerId)
        {
            List<GeneralInfoAbtStoreDTO> list = await _statisticsService.GetGeneralIngoAbtStore(sellerId);
            return list;
        }


        [HttpGet("weekly-by-sales-general/{sellerId}")]
        public async Task<IActionResult> GetWeeklyGeneral(int sellerId)
        {
            var db = _redis.GetDatabase();
            var json = await db.StringGetAsync($"weekly_general_stats:{sellerId}");

            if (json.IsNullOrEmpty)
                return NotFound("Statistics was not found or has not yet been calculated.");

            var data = JsonSerializer.Deserialize<List<WeeklyStatisticsDTO>>(json);
            return Ok(data);
        }

        [HttpGet("top-products/{storeId}")]
        public async Task<IActionResult> GetTopProducts(int storeId)
        {
            var db = _redis.GetDatabase();
            var json = await db.StringGetAsync($"top_products:{storeId}");

            if (json.IsNullOrEmpty)
                return NotFound("Top products were not found or have not yet been calculated.");

            var data = JsonSerializer.Deserialize<List<TopProductDTO>>(json);
            return Ok(data);
        }

        [HttpGet("total-statistics/{storeId}")]
        public async Task<IActionResult> GetStoreTotals(int storeId)
        {
            var db = _redis.GetDatabase();
            var json = await db.StringGetAsync($"store_total_stats:{storeId}");

            if (json.IsNullOrEmpty)
                return NotFound("Store totals data not found or not yet cached.");

            var data = JsonSerializer.Deserialize<StoreTotalStatisticsDTO>(json);
            return Ok(data);
        }
    }
}
