using Agora.BLL.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StackExchange.Redis;
using System.Text.Json;


namespace Agora.BLL.Services
{
    public class StatisticsCacheService : IHostedService, IDisposable
    {
        private Timer _timer;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IConnectionMultiplexer _redis;

        public StatisticsCacheService(IServiceScopeFactory scopeFactory, IConnectionMultiplexer redis)
        {
            _scopeFactory = scopeFactory;
            _redis = redis;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(UpdateRedisCache, null, TimeSpan.Zero, TimeSpan.FromDays(1));
            return Task.CompletedTask;
        }
        
        private static bool _firstRun = true;

        private async void UpdateRedisCache(object state)
        {
            Console.WriteLine("UpdateRedisCache started...");

            using var scope = _scopeFactory.CreateScope();
            var statsService = scope.ServiceProvider.GetRequiredService<IStatisticsService>();
            var db = _redis.GetDatabase();

            var today = DateOnly.FromDateTime(DateTime.Now);
            var storeService = scope.ServiceProvider.GetRequiredService<IStoreService>();
            var storeIds = await storeService.GetAllStoreIds();

            // set cache for all stores in db:
            foreach (var storeId in storeIds)
            {
                // UpdateRedisCache for first run or 1 time for week
                if (_firstRun || today.DayOfWeek == DayOfWeek.Monday)
                {
                    var weeklyStats = await statsService.GetWeeksStatisticsBySales(storeId);
                    var json = JsonSerializer.Serialize(weeklyStats);
                    // 7 - lifetime of cache in Redis for  weekly stetistics
                    await db.StringSetAsync($"weekly_stats:{storeId}", json, TimeSpan.FromDays(7));
                }

                // UpdateRedisCache for first run or 1 time for month
                if (_firstRun || today.Day == 1)
                {        
                    await CachePreviousMonthRevenueAsync(db, statsService, storeId);
                    await CachePrePreviousMonthRevenueAsync(db, statsService, storeId);
                }
            }
            
            _firstRun = false;
        }

        private async Task CachePreviousMonthRevenueAsync(IDatabase db, IStatisticsService statsService, int storeId)
        {
            var date = DateTime.Now.AddMonths(-1);
            var key = $"monthly_revenue:{date.Year}-{date.Month:D2}:{storeId}";

            var revenue = await statsService.GetPreviousMonthRevenue(storeId);
            var json = JsonSerializer.Serialize(revenue);

            await db.StringSetAsync(key, json, GetCacheLifetimeForMonth(date));
        }

        private async Task CachePrePreviousMonthRevenueAsync(IDatabase db, IStatisticsService statsService, int storeId)
        {
            var date = DateTime.Now.AddMonths(-2);
            var key = $"monthly_revenue:{date.Year}-{date.Month:D2}:{storeId}";

            var revenue = await statsService.GetPrePreviousMonthRevenue(storeId);
            var json = JsonSerializer.Serialize(revenue);
            // GetCacheLifetimeForMonth(date) - lifetime of cache in Redis (28, 29, 30 or 31 days) 
            await db.StringSetAsync(key, json, GetCacheLifetimeForMonth(date));
        }

        private TimeSpan GetCacheLifetimeForMonth(DateTime date)
        {
            var daysInMonth = DateTime.DaysInMonth(date.Year, date.Month);
            return TimeSpan.FromDays(daysInMonth);
        }
        
        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Dispose();
            return Task.CompletedTask;
        }

        public void Dispose() => _timer?.Dispose();
    }
}
