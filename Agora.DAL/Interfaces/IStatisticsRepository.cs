using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agora.DAL.Entities;

namespace Agora.DAL.Interfaces
{
    public interface IStatisticsRepository
    {
        Task<IQueryable<object>> GetWeeksStatisticsBySales(int storeId);
        Task<IQueryable<object>> GetPreviousMonthRevenue(int storeId);
        Task<IQueryable<object>> GetPrePreviousMonthRevenue(int storeId);
    }
}
