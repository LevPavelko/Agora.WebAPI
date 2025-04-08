using Agora.BLL.DTO;

namespace Agora.BLL.Interfaces
{
    public interface IStatisticsService
    {
        Task<List<WeeklyStatisticsDTO>> GetWeeksStatisticsBySales(int storeId);
        Task<List<DailyRevenueDTO>> GetPrePreviousMonthRevenue(int storeId);
        Task<List<DailyRevenueDTO>> GetPreviousMonthRevenue(int storeId);
    }
}
