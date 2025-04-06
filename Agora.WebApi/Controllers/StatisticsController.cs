using Agora.BLL.DTO;
using Agora.BLL.Interfaces;
using Microsoft.AspNetCore.Http;
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
    }
}
