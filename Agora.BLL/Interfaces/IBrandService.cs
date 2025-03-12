using Agora.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agora.BLL.Interfaces
{
    public interface IBrandService
    {
        Task<IQueryable<BrandDTO>> GetAll();
        Task<BrandDTO> Get(int id);
        Task Create(BrandDTO brandDTO);
        Task Update(BrandDTO brandDTO);
        Task Delete(int id);
    }
}
