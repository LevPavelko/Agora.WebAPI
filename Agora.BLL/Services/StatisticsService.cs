using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using Agora.BLL.DTO;
using Agora.BLL.Interfaces;
using Agora.DAL.Interfaces;
using AutoMapper;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

        public List<WeeklyStatisticsDTO> GetStatisticsWithSummedQuantity(List<WeeklyStatisticsDTO> statistics)
        {
            var groupedByDate = statistics
                .GroupBy(s => s.Date)  
                .Where(group => group.Count() >= 1)
                .Select(group => new WeeklyStatisticsDTO
                {
                    Date = group.Key,
                    DayOfWeek = group.Key.DayOfWeek.ToString(),
                    QuantityOfSales = group.Sum(s => s.QuantityOfSales)
                })
                .ToList();

            return groupedByDate;
        }


    }
}
