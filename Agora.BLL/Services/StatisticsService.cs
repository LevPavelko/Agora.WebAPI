using System.Globalization;
using System.Runtime.Serialization;
using Agora.BLL.DTO;
using Agora.BLL.Interfaces;
using Agora.DAL.Interfaces;
using AutoMapper;

namespace Agora.BLL.Services
{
    public class StatisticsService : IStatisticsService
    {
        IUnitOfWork Database { get; set; }
        IMapper _mapper;
        public StatisticsService(IUnitOfWork database, IMapper mapper)
        {
            Database = database;
            _mapper = mapper;
        }
        public async Task<List<WeeklyStatisticsDTO>> GetWeeksStatisticsBySales(int storeId)
        {
            IQueryable<object> objects = await Database.Statistics.GetWeeksStatisticsBySales(storeId);
            List<WeeklyStatisticsDTO> list = new List<WeeklyStatisticsDTO>();

            foreach (var item in objects)
            {
                WeeklyStatisticsDTO dto = new WeeklyStatisticsDTO();
                var dateProperty = item.GetType().GetProperty("Date");
                var quantityProperty = item.GetType().GetProperty("Quantity");

                if (dateProperty != null && quantityProperty != null)
                {
                    dto.Date = (DateOnly)dateProperty.GetValue(item);
                    dto.QuantityOfSales = (int)quantityProperty.GetValue(item);
                }

                list.Add(dto);
            }


            var statisticsWithSummedQuantity = GetStatisticsWithSummedQuantity(list);

            return statisticsWithSummedQuantity;
        }

        public async Task<List<WeeklyStatisticsDTO>> GetWeeksStatisticsBySalesGeneral(int sellerId)
        {
            IQueryable<object> objects = await Database.Statistics.GetWeeksStatisticsBySalesGeneral(sellerId);
            List<WeeklyStatisticsDTO> list = new List<WeeklyStatisticsDTO>();

            foreach (var item in objects)
            {
                WeeklyStatisticsDTO dto = new WeeklyStatisticsDTO();
                var dateProperty = item.GetType().GetProperty("Date");
                var quantityProperty = item.GetType().GetProperty("Quantity");

                if (dateProperty != null && quantityProperty != null)
                {
                    dto.Date = (DateOnly)dateProperty.GetValue(item);
                    dto.QuantityOfSales = (int)quantityProperty.GetValue(item);
                }

                list.Add(dto);
            }


            var statisticsWithSummedQuantity = GetStatisticsWithSummedQuantity(list);

            return statisticsWithSummedQuantity;
        }
        public List<WeeklyStatisticsDTO> GetStatisticsWithSummedQuantity(List<WeeklyStatisticsDTO> statistics)
        {
            var groupedByDate = statistics
                .GroupBy(s => s.Date)
                .Where(group => group.Count() >= 1)
                .Select(group => new WeeklyStatisticsDTO
                {
                    Date = group.Key,
                    DayOfWeek = group.Key.ToString("ddd", new CultureInfo("en-EN")),
                    QuantityOfSales = group.Sum(s => s.QuantityOfSales)
                })
                .ToList();

            return groupedByDate;
        }

        public async Task<List<DailyRevenueDTO>> GetPrePreviousMonthRevenue(int storeId)
        {
            IQueryable<object> objects = await Database.Statistics.GetPrePreviousMonthRevenue(storeId);
            return GetStatisticsWithSummedRevenues(objects);
        }

        public async Task<List<DailyRevenueDTO>> GetPreviousMonthRevenue(int storeId)
        {
            IQueryable<object> objects = await Database.Statistics.GetPreviousMonthRevenue(storeId);
            return GetStatisticsWithSummedRevenues(objects);
        }

        private List<DailyRevenueDTO> GetStatisticsWithSummedRevenues(IQueryable<object> objects)
        {
            var revenueList = new List<DailyRevenueDTO>();

            foreach (var item in objects)
            {
                var dateProperty = item.GetType().GetProperty("Date");
                var revenueProperty = item.GetType().GetProperty("Revenue");

                if (dateProperty != null && revenueProperty != null)
                {
                    var dto = new DailyRevenueDTO
                    {
                        Date = (DateOnly)dateProperty.GetValue(item),
                        Revenue = (decimal)revenueProperty.GetValue(item),
                    };
                    dto.DayOfWeek = dto.Date.DayOfWeek.ToString();

                    revenueList.Add(dto);
                }
            }

            var grouped = revenueList
                .GroupBy(r => r.Date)
                .Select(g => new DailyRevenueDTO
                {
                    Date = g.Key,
                    DayOfWeek = g.Key.DayOfWeek.ToString(),
                    Revenue = g.Sum(x => x.Revenue)
                })
                .OrderBy(r => r.Date)
                .ToList();

            return grouped;
        }

        public async Task<List<GeneralInfoAbtStoreDTO>> GetGeneralIngoAbtStore(int sellerId)
        {
            IQueryable<object> objects = await Database.Statistics.GetGeneralInfoAbtStore(sellerId);
            Dictionary<string, decimal> storePriceMap = new Dictionary<string, decimal>();
            Dictionary<string, int> storeOrdersMap = new Dictionary<string, int>();

            foreach (var item in objects)
            {
                var storeProp = item.GetType().GetProperty("Name");
                var storeName = storeProp?.GetValue(item)?.ToString();

                var priceProp = item.GetType().GetProperty("PriceAtMoment");
                var priceObj = priceProp?.GetValue(item);
                decimal price = priceObj != null ? Convert.ToDecimal(priceObj) : 0;

                if (!string.IsNullOrEmpty(storeName))
                {
                    if (storePriceMap.ContainsKey(storeName))
                        storePriceMap[storeName] += price;
                    else
                        storePriceMap[storeName] = price;

                    if (storeOrdersMap.ContainsKey(storeName))
                        storeOrdersMap[storeName] += 1;
                    else
                        storeOrdersMap[storeName] = 1;
                }
            }

            var list = storePriceMap
                .Select(kvp => new GeneralInfoAbtStoreDTO
                {
                    StoreName = kvp.Key,
                    TotalRevenue = kvp.Value,
                    TotalOrders = storeOrdersMap.ContainsKey(kvp.Key) ? storeOrdersMap[kvp.Key] : 0
                })
                .ToList();

            return list;
        }

        public async Task<List<DailyRevenueDTO>> GetPrePreviousMonthRevenueGeneral(int sellerId)
        {
            IQueryable<object> objects = await Database.Statistics.GetPrePreviousMonthRevenueGeneral(sellerId);
            return GetStatisticsWithSummedRevenues(objects);
        }

        public async Task<List<DailyRevenueDTO>> GetPreviousMonthRevenueGeneral(int sellerId)
        {
            IQueryable<object> objects = await Database.Statistics.GetPreviousMonthRevenueGeneral(sellerId);
            return GetStatisticsWithSummedRevenues(objects);
        }

    }
}
