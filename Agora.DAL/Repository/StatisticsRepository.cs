using Agora.DAL.EF;
using Agora.DAL.Interfaces;

namespace Agora.DAL.Repository
{
    public class StatisticsRepository : IStatisticsRepository
    {
        private AgoraContext db;
        public StatisticsRepository(AgoraContext context)
        {
            this.db = context;
        }

        public Task<IQueryable<object>> GetWeeksStatisticsBySales(int storeId)
        {
            Console.WriteLine("!!! Check work of  UpdateRedisCache - WeeksStatisticsBySales ");

            var dateNow = DateOnly.FromDateTime(DateTime.Now);
           
            var weekAgo = dateNow.AddDays(-7);
            
            var query = db.OrderItems
                .Where(o => o.Product.Store.Id == storeId)
                .Where(o => o.Status == Enums.OrderStatus.Completed)
                .Where(o => o.Date >= weekAgo)
                .Select(o => new
                {
                    o.Date,
                    o.Quantity
                   
                });
            
            return Task.FromResult(query.Cast<object>());
        }
        
        public Task<IQueryable<object>> GetPreviousMonthRevenue(int storeId)
        {
            Console.WriteLine("!!! Check work of  UpdateRedisCache - PreviousMonthRevenue ");

            var now = DateTime.Now;
            var firstDayOfThisMonth = new DateOnly(now.Year, now.Month, 1);
            var firstDayOfPreviousMonth = firstDayOfThisMonth.AddMonths(-1);
            var lastDayOfPreviousMonth = firstDayOfThisMonth.AddDays(-1);

            var query = db.OrderItems
                .Where(o => o.Product.Store.Id == storeId)
                .Where(o => o.Status == Enums.OrderStatus.Completed)
                .Where(o => o.Date >= firstDayOfPreviousMonth && o.Date <= lastDayOfPreviousMonth)
                .Select(o => new
                {
                    o.Date,
                    Revenue = o.PriceAtMoment * o.Quantity
                });

            return Task.FromResult(query.Cast<object>());
        }
        public Task<IQueryable<object>> GetPrePreviousMonthRevenue(int storeId)
        {
            Console.WriteLine("!!! Check work of  UpdateRedisCache - PrePreviousMonthRevenue ");

            var now = DateTime.Now;
            var firstDayOfThisMonth = new DateOnly(now.Year, now.Month, 1);
            var firstDayOfPrePreviousMonth = firstDayOfThisMonth.AddMonths(-2);
            var firstDayOfPreviousMonth = firstDayOfThisMonth.AddMonths(-1);
            var lastDayOfPrePreviousMonth = firstDayOfPreviousMonth.AddDays(-1);

            var query = db.OrderItems
                .Where(o => o.Product.Store.Id == storeId)
                .Where(o => o.Status == Enums.OrderStatus.Completed)
                .Where(o => o.Date >= firstDayOfPrePreviousMonth && o.Date <= lastDayOfPrePreviousMonth)
                .Select(o => new
                {
                    o.Date,
                    Revenue = o.PriceAtMoment * o.Quantity
                });

            return Task.FromResult(query.Cast<object>());
        }
    }
}
