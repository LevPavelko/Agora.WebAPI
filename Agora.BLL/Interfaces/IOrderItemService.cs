﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agora.BLL.DTO;

namespace Agora.BLL.Interfaces
{
    public interface IOrderItemService
    {
        Task<IQueryable<OrderItemDTO>> GetAll();
        Task<OrderItemDTO> Get(int id);
        Task Create(OrderItemDTO orderItemDTO);
        Task Update(OrderItemDTO orderItemDTO);
        Task Delete(int id);
    }
}
