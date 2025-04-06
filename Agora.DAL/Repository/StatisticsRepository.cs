using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agora.DAL.EF;
using Agora.DAL.Entities;
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

    }
}
