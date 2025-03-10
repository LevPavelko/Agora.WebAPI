using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agora.BLL.DTO;

namespace Agora.BLL.Interfaces
{
    public interface IShippingService
    {
        Task<IQueryable<SippingDTO>> GetAll();
        Task<SippingDTO> Get(int id);
        Task Create(SippingDTO sippingDTO);
        Task Update(SippingDTO sippingDTO);
        Task Delete(int id);
    }
}
