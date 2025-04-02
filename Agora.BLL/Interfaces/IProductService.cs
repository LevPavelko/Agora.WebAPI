using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agora.BLL.DTO;

namespace Agora.BLL.Interfaces
{
    public interface IProductService
    {
        Task<IQueryable<ProductDTO>> GetAll();
        Task<IEnumerable<ProductDTO>> GetFilteredByName(string filter); // to be continued
        Task<ProductDTO> Get(int id);
        Task Create(ProductDTO productDTO);
        Task Update(ProductDTO productDTO);
        Task Delete(int id);
    }
}
